using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.TIModels;
using Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;

namespace Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.ViewModels
{
    public class TechnicalInspectionPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ITaskService _taskService;
        private Dictionary<long, MaintenanceRepair> MaintenanceRepairKVPair;
        private DetailsDialog dd;
        public TechnicalInspectionPageViewModel(INavigationService navigationService, ITaskService taskService)
        {
            _navigationService = navigationService;
            this._taskService = taskService;
            MaintenanceRepairKVPair = new Dictionary<long, MaintenanceRepair>();
            this.MaintenanceRepairList = new ObservableCollection<MaintenanceRepair>();

            this.NextCommand = new DelegateCommand(async () =>
            {
                try
                {
                    await UpdateCacheOnCaptureImageAsync();

                    List<Pithline.FMS.BusinessLogic.Portable.TIModels.ImageCapture> imageCaptureList = new List<Pithline.FMS.BusinessLogic.Portable.TIModels.ImageCapture>();
                    foreach (var item in this.MaintenanceRepairList)
                    {
                        if (item.MajorComponentImgList.Any())
                        {
                            imageCaptureList.AddRange(item.MajorComponentImgList);
                        }
                        if (item.SubComponentImgList.Any())
                        {
                            imageCaptureList.AddRange(item.SubComponentImgList);
                        }

                    }
                    await Util.WriteToDiskAsync<List<Pithline.FMS.BusinessLogic.Portable.TIModels.ImageCapture>>(imageCaptureList, "ImageCaptureList");


                    _navigationService.Navigate("InspectionDetail", string.Empty);

                }
                catch (Exception ex)
                {
                }
            }, () =>
            {
                return this.SelectedTask.ComponentList != null && this.SelectedTask.ComponentList.Any();

            });

            this.DetailCommand = new DelegateCommand(async () =>
            {
                dd = new DetailsDialog();
                dd.DataContext = this.SelectedTask;
                await dd.ShowAsync();
            });


        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
                {
                    this.UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
                }

                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.SELECTEDTASK))
                {
                    this.SelectedTask = JsonConvert.DeserializeObject<Pithline.FMS.BusinessLogic.Portable.TIModels.TITask>(ApplicationData.Current.RoamingSettings.Values[Constants.SELECTEDTASK].ToString());
                }
                await UpdateCacheOnCaptureImageAsync();
            }
            catch (Exception)
            {

            }
        }

        async private System.Threading.Tasks.Task UpdateCacheOnCaptureImageAsync()
        {
            MaintenanceRepairKVPair = await Util.ReadFromDiskAsync<Dictionary<long, MaintenanceRepair>>("MaintenanceRepairKVPair");
            if (MaintenanceRepairKVPair != null)
            {
                ObservableCollection<MaintenanceRepair> mRepairList = new ObservableCollection<MaintenanceRepair>();
                foreach (var repair in MaintenanceRepairKVPair.Values)
                {
                    mRepairList.Add(repair);

                }
                this.MaintenanceRepairList = mRepairList;
            }
        }

        private ObservableCollection<MaintenanceRepair> maintenanceRepairList;
        public ObservableCollection<MaintenanceRepair> MaintenanceRepairList
        {
            get { return maintenanceRepairList; }
            set { SetProperty(ref maintenanceRepairList, value); }
        }

        public Pithline.FMS.BusinessLogic.Portable.TIModels.TITask SelectedTask { get; set; }
        public Pithline.FMS.BusinessLogic.Portable.TIModels.UserInfo UserInfo { get; set; }
        public DelegateCommand NextCommand { get; set; }

        public DelegateCommand DetailCommand { get; set; }
    }
}
