using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Pithline.FMS.ServiceScheduling.UILogic.Portable
{
    public class SubmittedDetailPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private IServiceDetailService _serviceDetailService;
        private ITaskService _taskService;
        public SubmittedDetailPageViewModel(INavigationService navigationService, IServiceDetailService serviceDetailService, ITaskService taskService)
        {
            this._navigationService = navigationService;
            this._taskService = taskService;
            this._serviceDetailService = serviceDetailService;
            this.Model = new ServiceSchedulingDetail();
            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
        async () =>
        {
            try
            {
                this.TaskProgressBar = Visibility.Visible;

                var respone = await this._taskService.UpdateStatusListAsync(this.SelectedTask, this.UserInfo);

                navigationService.Navigate("Main", string.Empty);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.TaskProgressBar = Visibility.Collapsed;
            }
        },

         () => { return this.Model != null; });

        }


        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.TaskProgressBar = Visibility.Visible;
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.SELECTEDTASK))
                {
                    this.SelectedTask = JsonConvert.DeserializeObject<Pithline.FMS.BusinessLogic.Portable.SSModels.Task>(ApplicationData.Current.RoamingSettings.Values[Constants.SELECTEDTASK].ToString());
                }
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
                {
                    this.UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
                }

                this.Model = await _serviceDetailService.GetServiceDetailAsync(this.SelectedTask.CaseNumber, this.SelectedTask.CaseServiceRecID, this.SelectedTask.ServiceRecID, this.UserInfo);
                this.TaskProgressBar = Visibility.Collapsed;
            }
            catch (Exception)
            {
                this.TaskProgressBar = Visibility.Collapsed;
            }
        }
        private ServiceSchedulingDetail model;
        public ServiceSchedulingDetail Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
        private Visibility taskProgressBar;
        public Visibility TaskProgressBar
        {
            get { return taskProgressBar; }
            set
            {
                SetProperty(ref taskProgressBar, value);
            }
        }
        public Pithline.FMS.BusinessLogic.Portable.SSModels.Task SelectedTask { get; set; }
        public UserInfo UserInfo { get; set; }
        public DelegateCommand HomePageCommand { get; private set; }
        public DelegateCommand NextPageCommand { get; private set; }

    }
}
