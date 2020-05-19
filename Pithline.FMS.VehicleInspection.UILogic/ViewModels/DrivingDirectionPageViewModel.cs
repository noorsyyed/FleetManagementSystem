using Bing.Maps;
using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.VehicleInspection.UILogic.AifServices;
using Pithline.FMS.VehicleInspection.UILogic.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
    public class DrivingDirectionPageViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private Pithline.FMS.BusinessLogic.Task _task;
        private IEventAggregator _eventAggregator;

        public DrivingDirectionPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            this.CustomerDetails = new BusinessLogic.CustomerDetails();
         

            GetDirectionsCommand = DelegateCommand<Location>.FromAsyncHandler(async (location) =>
            {
                var stringBuilder = new StringBuilder("bingmaps:?rtp=pos.");
                stringBuilder.Append(location.Latitude);
                stringBuilder.Append("_");
                stringBuilder.Append(location.Longitude);
                stringBuilder.Append("~adr." + Regex.Replace(this.CustomerDetails.Address, "\n", ","));
                await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
            });

            this.GoToVehicleInspectionCommand = new DelegateCommand(() =>
            {                
                string JsoninspectionTask = JsonConvert.SerializeObject(this._task);                
                _navigationService.Navigate("VehicleInspection", JsoninspectionTask);

            });

            this.StartDrivingCommand = new DelegateCommand(async () =>
            {
                await SqliteHelper.Storage.InsertSingleRecordAsync(new DrivingDuration { StartDateTime = DateTime.Now, VehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString()) });
                this.IsStartDriving = false;
                this.IsArrived = true;
            });
            this.ArrivedCommand = new DelegateCommand(async () =>
            {
                if (this._task != null)
                {
                    var vehicleInsRecId = Int64.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecId"].ToString());

                    var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.VehicleInsRecID.Equals(vehicleInsRecId));
                    dd.StopDateTime = DateTime.Now;
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(dd);
                }
                this.IsStartInspection = true;
                this.IsStartDriving = false;
                this.IsArrived = false;
            });


        }

        private void LoadAppointments()
        {
            var startTime = new DateTime(this._task.ConfirmedDate.Year, this._task.ConfirmedDate.Month, this._task.ConfirmedDate.Day, this._task.ConfirmedTime.Hour, this._task.ConfirmedTime.Minute,
                             this._task.ConfirmedTime.Second);
            this.CustomerDetails.Appointments = new ScheduleAppointmentCollection
            {
                new ScheduleAppointment(){
                    Subject = this._task.CaseNumber,                    
                    Location =this._task.Address,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(1),
                    ReadOnly = true,
                    AppointmentBackground = new SolidColorBrush(Colors.Crimson),                   
                    Status = new ScheduleAppointmentStatus{Status = this._task.Status,Brush = new SolidColorBrush(Colors.Chocolate)}

                },
                               
            };
        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            _task = JsonConvert.DeserializeObject<Pithline.FMS.BusinessLogic.Task>(navigationParameter.ToString());
            await GetCustomerDetailsAsync();
            LoadAppointments();
            var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.VehicleInsRecID == _task.VehicleInsRecId);
            if (dd != null)
            {
                this.IsArrived = dd.StopDateTime == DateTime.MinValue;
                this.IsStartInspection = !this.IsArrived;
            }
            else
            {
                this.IsStartDriving = true;
                this.IsArrived = false;
                this.IsStartInspection = false;
            }
            _eventAggregator.GetEvent<CustFetchedEvent>().Subscribe(async b =>
            {
                await GetCustomerDetailsAsync();
            });
        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

        public ICommand GetDirectionsCommand { get; set; }
        public DelegateCommand GoToVehicleInspectionCommand { get; set; }
        public DelegateCommand StartDrivingCommand { get; set; }
        public DelegateCommand ArrivedCommand { get; set; }

        private bool isStartInspection;
        public bool IsStartInspection
        {
            get { return isStartInspection; }
            set { SetProperty(ref isStartInspection, value); }
        }

        private bool isStartDriving;
        public bool IsStartDriving
        {
            get { return isStartDriving; }
            set { SetProperty(ref isStartDriving, value); }
        }

        private bool isArrived;
        public bool IsArrived
        {
            get { return isArrived; }
            set { SetProperty(ref isArrived, value); }
        }


        private Customer customer;

        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
        }

        async private System.Threading.Tasks.Task GetCustomerDetailsAsync()
        {
            try
            {
                if (this._task != null)
                {
                    AppSettings.Instance.IsSyncingCustDetails = 0;
                    this.CustomerDetails.ContactNumber = this._task.ContactNumber;
                    this.CustomerDetails.CaseNumber = this._task.CaseNumber;
                    this.CustomerDetails.VehicleInsRecId = this._task.VehicleInsRecId;
                    this.CustomerDetails.Status = this._task.Status;
                    this.CustomerDetails.StatusDueDate = this._task.StatusDueDate;
                    this.CustomerDetails.Address = this._task.Address;
                    this.CustomerDetails.AllocatedTo = this._task.AllocatedTo;
                    this.CustomerDetails.CustomerName = this._task.CustomerName;
                    this.CustomerDetails.ContactName = this._task.ContactName;
                    this.CustomerDetails.CategoryType = this._task.CategoryType;
                    this.CustomerDetails.EmailId = this._task.Email;

                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
