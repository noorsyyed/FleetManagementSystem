using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Commercial;
using Pithline.FMS.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using Pithline.FMS.BusinessLogic.Helpers;
namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
    public class ImageMarkupPageViewModel : ViewModel
    {
        public ImageMarkupPageViewModel()
        {
            this.Snapshots = new ObservableCollection<ImageCapture>();

            DoneCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                try
                {
                    var caseNumber = ApplicationData.Current.LocalSettings.Values["CaseNumber"].ToString();
                    foreach (var item in this.Snapshots)
                    {
                        var image = await StorageFile.GetFileFromPathAsync(item.ImagePath);
                        var curMarkupImageFile = await ApplicationData.Current.RoamingFolder.TryGetItemAsync("markupimage_" + caseNumber + this.Snapshots.IndexOf(item)) as StorageFile;
                        if (image != null && curMarkupImageFile != null)
                        {
                            var ms = await RenderDataStampOnSnap.InterpolateImageMarkup(image, curMarkupImageFile);
                            var msrandom = new MemoryRandomAccessStream(ms);
                            Byte[] bytes = new Byte[ms.Length];
                            await ms.ReadAsync(bytes, 0, (int)ms.Length);
                            // StorageFile file = await KnownFolders.PicturesLibrary.CreateFileAsync("Image.png", Windows.Storage.CreationCollisionOption.GenerateUniqueName);

                            StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(DateTime.Now.Ticks.ToString() + curMarkupImageFile.Name, CreationCollisionOption.ReplaceExisting);
                            using (var strm = await file.OpenStreamForWriteAsync())
                            {
                                await strm.WriteAsync(bytes, 0, bytes.Length);
                                strm.Flush();
                            }

                          await  UpdateImageAsync(new ImageCapture
                            {
                                CaseServiceRecId = Convert.ToInt64(ApplicationData.Current.LocalSettings.Values["VehicleInsRecId"] ),
                                ImageBinary = Convert.ToBase64String(bytes),
                                 ImagePath = file.Path,
                                  FileName = item.FileName+"Marked"
                            });

                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            });
        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            if (navigationParameter is PVehicleDetails)
            {
                var model = (PVehicleDetails)navigationParameter;
                this.Snapshots.Add(model.FrontSnapshot);
                this.Snapshots.Add(model.BackSnapshot);
                this.Snapshots.Add(model.LeftSnapshot);
                this.Snapshots.Add(model.RightSnapshot);
                this.Snapshots.Add(model.TopSnapshot);
                PanelBackground = new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri(((PVehicleDetails)navigationParameter).BackSnapshot.ImagePath)),
                    Stretch = Stretch.Fill
                };
            }
            else if (navigationParameter is CVehicleDetails)
            {
                var model = (CVehicleDetails)navigationParameter;
                this.Snapshots.Add(model.FrontSnapshot);
                this.Snapshots.Add(model.RightSnapshot);
                this.Snapshots.Add(model.LeftSnapshot);
                PanelBackground = new ImageBrush()
            {
                // ImageSource = new BitmapImage(new Uri(((CVehicleDetails)navigationParameter).BackSnapshot.ImagePath)),
                Stretch = Stretch.Fill
            };
            }

            else
            {
                var model = (TVehicleDetails)navigationParameter;
                this.Snapshots.Add(model.FrontSnapshot);
                this.Snapshots.Add(model.BackSnapshot);
                this.Snapshots.Add(model.LeftSnapshot);
                this.Snapshots.Add(model.RightSnapshot);

                PanelBackground = new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri(((TVehicleDetails)navigationParameter).BackSnapshot.ImagePath)),
                    Stretch = Stretch.Fill
                };
            }



        }
        private ObservableCollection<ImageCapture> snapshots;

        public ObservableCollection<ImageCapture> Snapshots
        {
            get { return snapshots; }
            set { SetProperty(ref snapshots, value); }
        }

        private ImageBrush panelBackground;
        public ImageBrush PanelBackground
        {
            get { return panelBackground; }
            set { SetProperty(ref panelBackground, value); }
        }

        private async System.Threading.Tasks.Task UpdateImageAsync(ImageCapture param)
        {
            var imageTable = await Pithline.FMS.BusinessLogic.Helpers.SqliteHelper.Storage.LoadTableAsync<ImageCapture>();
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

        public ICommand DoneCommand { get; set; }

    }
}
