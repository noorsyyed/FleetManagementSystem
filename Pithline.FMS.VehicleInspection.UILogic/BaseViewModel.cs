using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.VehicleInspection.UILogic.AifServices;
using Pithline.FMS.VehicleInspection.UILogic.Popups;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Input;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using System.Linq;

namespace Pithline.FMS.VehicleInspection.UILogic
{
    [DataContract]
    public class BaseViewModel : ViewModel
    {
        SnapshotsViewer _snapShotsPopup;
        INavigationService _navigationService;


        public BaseViewModel(IEventAggregator eventAggregator)
        {
            TakeSnapshotCommand = DelegateCommand<Tuple<object, object>>.FromAsyncHandler(async (param) =>
            {
                await TakeSnapshotAsync(param.Item1 as ObservableCollection<ImageCapture>, param.Item2.ToString());
            });

            TakePictureCommand = DelegateCommand<Tuple<object, object>>.FromAsyncHandler(async (param) =>
                {
                    await TakePictureAsync(param.Item1 as ImageCapture, param.Item2.ToString());
                });

            this.OpenSnapshotViewerCommand = new DelegateCommand<dynamic>((param) =>
            {
                OpenPopup(param);
            });

            this.SaveModelCommand = new DelegateCommand<object>(async (param) =>
                {
                    BaseModel baseModel = param as BaseModel;
                    long vehicleInsRecId = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecId"].ToString());
                    int successFlag = 0;
                    try
                    {
                        if (baseModel.ValidateModel())
                        {
                            var viobj = await baseModel.GetDataAsync(vehicleInsRecId);
                            if (viobj != null)
                            {
                                successFlag = await SqliteHelper.Storage.UpdateSingleRecordAsync(baseModel);
                            }
                            else
                            {
                                baseModel.VehicleInsRecID = vehicleInsRecId;
                                successFlag = await SqliteHelper.Storage.InsertSingleRecordAsync(baseModel);
                            }

                            if (successFlag != 0)
                            {
                                baseModel.ShouldSave = false;
                                await VIServiceHelper.Instance.SyncFromSvcAsync(baseModel);
                            }
                        }
                        else
                        {
                            eventAggregator.GetEvent<ErrorsRaisedEvent>().Publish(baseModel.Errors);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                });
        }
        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.GoHomeCommand = new DelegateCommand(() =>
            {
                _navigationService.ClearHistory();
                _navigationService.Navigate("Main", string.Empty);
            });
        }

        private Object model;
        [RestorableState]
        public Object Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        public DelegateCommand<object> SaveModelCommand { get; set; }

        public DelegateCommand GoHomeCommand { get; set; }

        public ICommand TakeSnapshotCommand { get; set; }

        public ICommand TakePictureCommand { get; set; }

        public DelegateCommand<object> OpenSnapshotViewerCommand { get; set; }
        protected async System.Threading.Tasks.Task TakeSnapshotAsync<T>(T list, string fieldName) where T : ObservableCollection<ImageCapture>
        {
            try
            {
                CameraCaptureUI ccui = new CameraCaptureUI();
                var storagefile = await ccui.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (storagefile != null)
                {

                    var ms = await RenderDataStampOnSnap.RenderStaticTextToBitmap(storagefile);
                    var msrandom = new MemoryRandomAccessStream(ms);
                    Byte[] bytes = new Byte[ms.Length];
                    await ms.ReadAsync(bytes, 0, (int)ms.Length);
                    // StorageFile file = await KnownFolders.PicturesLibrary.CreateFileAsync("Image.png", Windows.Storage.CreationCollisionOption.GenerateUniqueName);

                    StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(DateTime.Now.Ticks.ToString() + storagefile.Name, CreationCollisionOption.ReplaceExisting);
                    using (var strm = await file.OpenStreamForWriteAsync())
                    {
                        await strm.WriteAsync(bytes, 0, bytes.Length);
                        strm.Flush();
                    }

                    var ic = new ImageCapture
                    {
                        ImagePath = file.Path,
                        ImageBinary = Convert.ToBase64String(bytes),
                        CaseServiceRecId = ((BaseModel)this.Model).VehicleInsRecID,
                        FileName = string.Format("{0}_{1}", fieldName, list.Count + 1)
                    };
                    list.Add(ic);

                    var imageTable = await SqliteHelper.Storage.LoadTableAsync<ImageCapture>();
                    var dbIC = imageTable.SingleOrDefault(x => x.CaseServiceRecId == ic.CaseServiceRecId && x.FileName == string.Format("{0}_{1}", fieldName, list.Count + 1));
                    if (dbIC == null)
                    {
                        await SqliteHelper.Storage.InsertSingleRecordAsync<ImageCapture>(ic);
                    }
                    else
                    {
                        dbIC.ImagePath = ic.ImagePath;
                        dbIC.ImageBinary = ic.ImageBinary;
                        await SqliteHelper.Storage.UpdateSingleRecordAsync<ImageCapture>(dbIC);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        async public virtual System.Threading.Tasks.Task TakePictureAsync(ImageCapture param, string fieldName)
        {
            try
            {
                CameraCaptureUI cam = new CameraCaptureUI();

                var storagefile = await cam.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (storagefile != null)
                {

                    using (MemoryStream ms = await RenderDataStampOnSnap.RenderStaticTextToBitmap(storagefile))
                    {
                        using (var msrandom = new MemoryRandomAccessStream(ms))
                        {
                            Byte[] bytes = new Byte[ms.Length];
                            await ms.ReadAsync(bytes, 0, (int)ms.Length);
                            // StorageFile file = await KnownFolders.PicturesLibrary.CreateFileAsync("Image.png", Windows.Storage.CreationCollisionOption.GenerateUniqueName);

                            StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(DateTime.Now.Ticks.ToString() + storagefile.Name, CreationCollisionOption.ReplaceExisting);
                            using (var strm = await file.OpenStreamForWriteAsync())
                            {
                                await strm.WriteAsync(bytes, 0, bytes.Length);
                                strm.Flush();
                            }

                            param.ImagePath = file.Path;
                            param.ImageBinary = Convert.ToBase64String(bytes);
                            param.CaseServiceRecId = ((BaseModel)this.Model).VehicleInsRecID;
                            param.FileName = fieldName;

                            await UpdateImageAsync(param);
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async System.Threading.Tasks.Task UpdateImageAsync(ImageCapture param)
        {
            var imageTable = await SqliteHelper.Storage.LoadTableAsync<ImageCapture>();
            var dbIC = imageTable.SingleOrDefault(x => x.CaseServiceRecId == param.CaseServiceRecId && x.FileName == param.FileName);

            if (dbIC == null)
            {
                await SqliteHelper.Storage.InsertSingleRecordAsync<ImageCapture>(param);
            }
            else
            {
                dbIC.ImagePath = param.ImagePath;
                dbIC.ImageBinary = param.ImageBinary;
                await SqliteHelper.Storage.UpdateSingleRecordAsync<ImageCapture>(dbIC);
            }
        }
        public void OpenPopup(dynamic dc)
        {
            CoreWindow currentWindow = Window.Current.CoreWindow;
            Popup popup = new Popup();
            popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            popup.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            if (_snapShotsPopup == null)
            {
                _snapShotsPopup = new SnapshotsViewer();

            }
            else
            {
                _snapShotsPopup = null;
                this._snapShotsPopup = new SnapshotsViewer();
            }
            _snapShotsPopup.DataContext = dc;


            popup.Child = _snapShotsPopup;
            this._snapShotsPopup.Tag = popup;

            this._snapShotsPopup.Height = currentWindow.Bounds.Height;
            this._snapShotsPopup.Width = currentWindow.Bounds.Width;

            popup.IsOpen = true;

        }
        public virtual System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            return null;
        }


        /// <summary>
        /// /  This metod is only for testing suspension ,later we can remove it
        /// </summary>
        /// <param name="viewModelState"></param>
        /// <param name="suspending"></param>
        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);

        }
        /// <summary>
        /// This metod is only for testing suspension, later we can remove it
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

    }
}
