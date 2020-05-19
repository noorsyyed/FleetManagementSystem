using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.ServiceSchedule;
using Pithline.FMS.ServiceScheduling.UILogic.AifServices;
using Pithline.FMS.ServiceScheduling.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;
using System.Linq;

namespace Pithline.FMS.ServiceScheduling.UILogic.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IEventAggregator _eventAggregator;
        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            this.PoolofTasks = new ObservableCollection<DriverTask>();
            this.Appointments = new ScheduleAppointmentCollection();
            this.CustomerDetails = new CustomerDetails();
            this.CustomerDetails.Appointments = new ScheduleAppointmentCollection();
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

            this.SyncCommand = new DelegateCommand(async () =>
            {
                if (AppSettings.Instance.IsSynchronizing == 0)
                {
                    AppSettings.Instance.ErrorMessage = string.Empty;

                    AppSettings.Instance.IsSynchronizing = 1;

                    await GetTasksFromDbAsync();
                    AppSettings.Instance.IsSynchronizing = 0;
                    AppSettings.Instance.Synced = true;

                }
            });

            this.StartSchedulingCommand = new DelegateCommand<object>((obj) =>
            {
                try
                {
                    GetCustomerDetailsAsync();
                    PersistentData.RefreshInstance();//Here only setting data in new instance, and  getting data in every page.
                    PersistentData.Instance.DriverTask = this.InspectionTask;
                    PersistentData.Instance.CustomerDetails = this.CustomerDetails;

                    if (this.InspectionTask.Status == DriverTaskStatus.AwaitServiceBookingConfirmation || this.InspectionTask.Status == DriverTaskStatus.AwaitJobCardCapture)
                    {
                        _navigationService.Navigate("Confirmation", string.Empty);
                    }

                    if (this.InspectionTask.Status == DriverTaskStatus.AwaitSupplierSelection)
                    {
                        _navigationService.Navigate("SupplierSelection", string.Empty);
                    }
                    if (this.InspectionTask.Status == DriverTaskStatus.AwaitServiceBookingDetail)
                    {
                        _navigationService.Navigate("ServiceScheduling", string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }
            });
        }

        private async System.Threading.Tasks.Task GetTasksFromDbAsync()
        {
            var list = await SSProxyHelper.Instance.GetTasksFromSvcAsync();
            this.PoolofTasks.Clear();
            try
            {
                if (list != null)
                {
                    foreach (Pithline.FMS.BusinessLogic.ServiceSchedule.DriverTask item in list.Where(x => x.Status != TaskStatus.Completed))
                    {
                        if (item != null)
                        {
                            this.PoolofTasks.Add(item);
                            GetAppointments(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.IsBusy = true;
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            _navigationService.ClearHistory();

            await GetTasksFromDbAsync();
            this.IsBusy = false;
            PersistentData.Instance.PoolOfTask = new ObservableCollection<DriverTask>();
            PersistentData.Instance.PoolOfTask.AddRange(this.PoolofTasks);

        }
        private void GetAppointments(DriverTask task)
        {
            try
            {
                if (!String.IsNullOrEmpty(task.ConfirmationDate))
                {

                    var startTime = new DateTime(DateTime.Parse(task.ConfirmationDate).Year, DateTime.Parse(task.ConfirmationDate).Month, DateTime.Parse(task.ConfirmationDate).Day);

                    this.Appointments.Add(
                    new ScheduleAppointment()
                    {
                        Subject = task.CaseNumber,
                        Location = task.Address,
                        StartTime = startTime,
                        EndTime = startTime.AddHours(12),
                        ReadOnly = true,
                        AppointmentBackground = new SolidColorBrush(Colors.Crimson),
                        AllDay = true,
                        Status = new ScheduleAppointmentStatus { Status = task.Status, Brush = new SolidColorBrush(Colors.Chocolate) }

                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private DriverTask task;
        public DriverTask InspectionTask
        {
            get { return task; }
            set
            {
                SetProperty(ref task, value);
            }
        }

        private ObservableCollection<DriverTask> poolofTasks;
        public ObservableCollection<DriverTask> PoolofTasks
        {
            get { return poolofTasks; }
            set
            {
                SetProperty(ref poolofTasks, value);
            }
        }

        public DelegateCommand SyncCommand { get; set; }

        private ScheduleAppointmentCollection appointments;
        public ScheduleAppointmentCollection Appointments
        {
            get { return appointments; }
            set { SetProperty(ref appointments, value); }
        }
        public DelegateCommand BingWeatherCommand { get; set; }
        public DelegateCommand<object> StartSchedulingCommand { get; set; }

        private CustomerDetails customerDetails;
        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }
        private void GetCustomerDetailsAsync()
        {
            try
            {
                if (this.InspectionTask != null)
                {
                    this.CustomerDetails.ContactNumber = this.InspectionTask.CustPhone;
                    this.CustomerDetails.CaseNumber = this.InspectionTask.CaseNumber;
                    this.CustomerDetails.VehicleInsRecId = this.InspectionTask.VehicleInsRecId;
                    this.CustomerDetails.Status = this.InspectionTask.Status;
                    this.CustomerDetails.StatusDueDate = this.InspectionTask.StatusDueDate;
                    this.CustomerDetails.Address = this.InspectionTask.Address;
                    this.CustomerDetails.AllocatedTo = this.InspectionTask.AllocatedTo;
                    this.CustomerDetails.CustomerName = this.InspectionTask.CustomerName;
                    this.CustomerDetails.ContactName = this.InspectionTask.CustomerName;
                    this.CustomerDetails.EmailId = this.InspectionTask.CusEmailId;
                    this.CustomerDetails.CategoryType = this.InspectionTask.CaseCategory;
                    this.CustomerDetails.Appointments = this.Appointments;
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
    }
}
