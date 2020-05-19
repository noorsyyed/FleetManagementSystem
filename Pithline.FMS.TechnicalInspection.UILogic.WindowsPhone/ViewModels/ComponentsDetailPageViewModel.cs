using Pithline.FMS.BusinessLogic.Portable.TIModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.ApplicationModel.Activation;
using Windows.Storage.Pickers;
using System.Linq;
using Windows.Storage;
using Pithline.FMS.BusinessLogic.Portable;
using System.Runtime.Serialization;

namespace Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.ViewModels
{
    public class ComponentsDetailPageViewModel : ViewModel
    {
        private SnapshotsViewer _snapShotsPopup;
        public INavigationService _navigationService;
        public IEventAggregator _eventAggregator;
        private Dictionary<long, MaintenanceRepair> MaintenanceRepairKVPair;
        public ComponentsDetailPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            MaintenanceRepairKVPair = new Dictionary<long, MaintenanceRepair>();
            TakeSnapshotCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                FileOpenPicker openPicker = new FileOpenPicker();

                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;

                openPicker.FileTypeFilter.Add(".bmp");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".jpg");

                await Util.WriteToDiskAsync<MaintenanceRepair>(this.SelectedMaintenanceRepair, "SelectedMaintenanceRepair");

                openPicker.PickSingleFileAndContinue();

                this._eventAggregator.GetEvent<ImageCaptureTranEvent>().Subscribe(async (imageCaptured) =>
                {
                    this.SelectedMaintenanceRepair = await Util.ReadFromDiskAsync<MaintenanceRepair>("SelectedMaintenanceRepair");

                    if (selectedMaintenanceRepair.IsMajorPivot)
                    {
                        imageCaptured.Component = this.SelectedMaintenanceRepair.MajorComponent;
                        imageCaptured.RepairId = this.SelectedMaintenanceRepair.Repairid;

                        this.SelectedMaintenanceRepair.MajorComponentImgList.Add(imageCaptured);
                    }
                    else
                    {
                        imageCaptured.Component = this.SelectedMaintenanceRepair.SubComponent;
                        imageCaptured.RepairId = this.SelectedMaintenanceRepair.Repairid;

                        this.SelectedMaintenanceRepair.SubComponentImgList.Add(imageCaptured);
                    }

                    foreach (var item in this.SelectedMaintenanceRepair.MajorComponentImgList)
                    {
                        item.ImageBitmap = Util.FromBase64(item.ImageData);
                    }
                    foreach (var item in this.SelectedMaintenanceRepair.SubComponentImgList)
                    {
                        item.ImageBitmap = Util.FromBase64(item.ImageData);
                    }

                });
            });

            PreviousCommand = new DelegateCommand(() =>
            {
                navigationService.Navigate("TechnicalInspection", string.Empty);
            });

        }


        async public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            if (this._snapShotsPopup != null)
            {
                this._snapShotsPopup.Hide();
            }
            await CacheImagesOnNavigationAsync();
            base.OnNavigatedFrom(viewModelState, suspending);
        }

        async public System.Threading.Tasks.Task CacheImagesOnNavigationAsync()
        {
            try
            {
                await Util.WriteToDiskAsync<MaintenanceRepair>(this.SelectedMaintenanceRepair, "SelectedMaintenanceRepair");

                MaintenanceRepairKVPair = await Util.ReadFromDiskAsync<Dictionary<long, MaintenanceRepair>>("MaintenanceRepairKVPair");
                if (MaintenanceRepairKVPair == null)
                {
                    MaintenanceRepairKVPair = new Dictionary<long, MaintenanceRepair>();
                }

                if (!MaintenanceRepairKVPair.ContainsKey(this.SelectedMaintenanceRepair.Repairid))
                {
                    MaintenanceRepairKVPair.Add(this.SelectedMaintenanceRepair.Repairid, this.SelectedMaintenanceRepair);
                }
                else
                {
                    MaintenanceRepairKVPair[this.SelectedMaintenanceRepair.Repairid] = this.SelectedMaintenanceRepair;
                }

                await Util.WriteToDiskAsync<Dictionary<long, MaintenanceRepair>>(this.MaintenanceRepairKVPair, "MaintenanceRepairKVPair");

            }
            catch (Exception)
            {

            }

        }
        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

                this.SelectedMaintenanceRepair = JsonConvert.DeserializeObject<MaintenanceRepair>(navigationParameter.ToString());

                MaintenanceRepairKVPair = await Util.ReadFromDiskAsync<Dictionary<long, MaintenanceRepair>>("MaintenanceRepairKVPair");
                if (MaintenanceRepairKVPair != null && MaintenanceRepairKVPair.Any())
                {
                    this.SelectedMaintenanceRepair = MaintenanceRepairKVPair.Values.FirstOrDefault(f => f.Repairid == this.SelectedMaintenanceRepair.Repairid);
                }
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.SELECTEDTASK))
                {
                    this.SelectedTask = JsonConvert.DeserializeObject<Pithline.FMS.BusinessLogic.Portable.TIModels.TITask>(ApplicationData.Current.RoamingSettings.Values[Constants.SELECTEDTASK].ToString());
                }

                foreach (var item in this.SelectedMaintenanceRepair.MajorComponentImgList)
                {
                    item.ImageBitmap = Util.FromBase64(item.ImageData);
                }
                foreach (var item in this.SelectedMaintenanceRepair.SubComponentImgList)
                {
                    item.ImageBitmap = Util.FromBase64(item.ImageData);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private MaintenanceRepair selectedMaintenanceRepair;
        public MaintenanceRepair SelectedMaintenanceRepair
        {
            get { return selectedMaintenanceRepair; }
            set { SetProperty(ref selectedMaintenanceRepair, value); }
        }
        public Pithline.FMS.BusinessLogic.Portable.TIModels.TITask SelectedTask { get; set; }
        public ICommand TakeSnapshotCommand { get; set; }

        public ICommand PreviousCommand { get; set; }

        public ICommand OpenSnapshotViewerCommand { get; set; }
    }
}
