using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.VehicleInspection.UILogic.AifServices;
using Pithline.FMS.VehicleInspection.UILogic.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
    public class InspectionDetailsPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IEventAggregator _eventAggregator;
        public InspectionDetailsPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            this.InspectionList = new ObservableCollection<BusinessLogic.Task>();
            this.CustomerDetails = new CustomerDetails();
            this.PoolofTasks = new ObservableCollection<BusinessLogic.Task>();
            this.Appointments = new ScheduleAppointmentCollection();
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

            DrivingDirectionCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                ApplicationData.Current.LocalSettings.Values["CaseNumber"] = this.InspectionTask.CaseNumber;
                ApplicationData.Current.LocalSettings.Values["VehicleInsRecId"] = this.InspectionTask.VehicleInsRecId;

                string jsonInspectionTask = JsonConvert.SerializeObject(this.InspectionTask);
                var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.VehicleInsRecID == this.InspectionTask.VehicleInsRecId);

                if (dd != null && !dd.StopDateTime.Equals(DateTime.MinValue))
                {
                    _navigationService.Navigate("VehicleInspection", jsonInspectionTask);
                }
                else
                {
                    navigationService.Navigate("DrivingDirection", jsonInspectionTask);
                }

            }, () =>
            {
                return (this.InspectionTask != null);
            }
            );




            this.SaveCommand = new DelegateCommand(async () =>
            {
                this.IsBusy = true;
                if (this.InspectionTask.ConfirmedDate < DateTime.Today)
                {
                    Util.ShowToast("Confirmed Date should not be less than today's date");
                }
                else
                {
                    this.InspectionTask.ShouldSync = true;
                    if (this.InspectionTask.CategoryType.Equals("Vehicle Inspection", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.InspectionTask.Status = Pithline.FMS.BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture;
                        this.InspectionTask.ProcessStep = ProcessStep.ConfirmInspectionDetails;
                    }
                    else
                    {
                        this.InspectionTask.Status = TaskStatus.AwaitCollectionDataCapture;
                        this.InspectionTask.ProcessStep = ProcessStep.ConfirmVehicleCollection;
                    }

                    await SqliteHelper.Storage.UpdateSingleRecordAsync(this.InspectionTask);
                    var startTime = new DateTime(this.InspectionTask.ConfirmedDate.Year, this.InspectionTask.ConfirmedDate.Month, this.InspectionTask.ConfirmedDate.Day, this.InspectionTask.ConfirmedTime.Hour, this.InspectionTask.ConfirmedTime.Minute,
                            this.InspectionTask.ConfirmedTime.Second);
                    this.Appointments.Add(new ScheduleAppointment
                    {
                        Subject = "Inspection at " + this.InspectionTask.CustomerName,
                        StartTime = startTime,
                        Location = this.InspectionTask.Address,
                        EndTime = startTime.AddHours(1),
                        Status = new ScheduleAppointmentStatus { Brush = new SolidColorBrush(Colors.DarkMagenta), Status = "Free" },
                    });
                    this.SaveCommand.RaiseCanExecuteChanged();
                    this.IsCommandBarOpen = false;
                    await VIServiceHelper.Instance.UpdateTaskStatusAsync();
                    navigationService.GoBack();
                }
                IsBusy = false;
            }
            , () =>
            {
                return (this.InspectionTask != null);
            }
            );
        }
        #region Overrides
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

                //foreach (var t in tasks)
                //{
                //    var cust = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(x => x.Id == t.CustomerId);
                //    if (cust != null)
                //    {
                //        t.CustomerName = cust.CustomerName;
                //        t.Address = cust.Address;
                //    }
                //}

                _eventAggregator.GetEvent<TasksFetchedEvent>().Subscribe(async o =>
                    {

                        await ShowTasksAsync(navigationParameter);

                    }, ThreadOption.UIThread);


                await ShowTasksAsync(navigationParameter);
            }
            catch (SQLite.SQLiteException)
            {
            }
        }

        private async System.Threading.Tasks.Task ShowTasksAsync(object navigationParameter)
        {
            var _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
            var list = EnumerateTasks(navigationParameter, (await SqliteHelper.Storage.LoadTableAsync<Pithline.FMS.BusinessLogic.Task>()).Where(x => x.UserId == _userInfo.UserId));
            this.InspectionList.Clear();
            foreach (var item in list)
            {
                this.InspectionList.Add(item);
            }
            this.InspectionTask = this.InspectionList.FirstOrDefault();
            _eventAggregator.GetEvent<CustFetchedEvent>().Subscribe(async b =>
            {
                await GetCustomerDetailsAsync(b);
            });
        }

        private IEnumerable<BusinessLogic.Task> EnumerateTasks(object navigationParameter, IEnumerable<BusinessLogic.Task> tasks)
        {
            try
            {
                IEnumerable<Pithline.FMS.BusinessLogic.Task> list = null;
                if (navigationParameter.Equals("AwaitConfirmation"))
                {
                    NavigationMode = Syncfusion.UI.Xaml.Grid.NavigationMode.Cell;
                    this.AllowEditing = true;
                    list = (tasks).Where(x => Regex.Replace(x.Status, @"\s", "").ToLower() == BusinessLogic.Helpers.TaskStatus.AwaitingInspectionConfirmation.ToLower()
                        || x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitCollectionConfirmation));
                    list.AsParallel().ForAll(x => x.AllowEditing = true);
                    //|| x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitCollectionDetail));
                }
                if (navigationParameter.Equals("Total"))
                {
                    NavigationMode = Syncfusion.UI.Xaml.Grid.NavigationMode.Row;
                    this.AllowEditing = false;
                    list = (tasks).Where(x => DateTime.Equals(x.ConfirmedDate, DateTime.Today) && (x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture, StringComparison.OrdinalIgnoreCase) || x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitCollectionDataCapture, StringComparison.OrdinalIgnoreCase)));
                    list.AsParallel().ForAll(x => x.AllowEditing = false);
                }
                if (navigationParameter.Equals("MyTasks"))
                {
                    NavigationMode = Syncfusion.UI.Xaml.Grid.NavigationMode.Row;
                    this.AllowEditing = false;
                    list = (tasks).Where(x => x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture, StringComparison.OrdinalIgnoreCase) || x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitCollectionDataCapture, StringComparison.OrdinalIgnoreCase));
                    list.AsParallel().ForAll(x => x.AllowEditing = false);
                }
                this.CustomerDetails.Appointments = new ScheduleAppointmentCollection();
                foreach (var item in tasks.Where(x => x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture) || x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionAcceptance)))
                {

                    var startTime = new DateTime(item.ConfirmedDate.Year, item.ConfirmedDate.Month, item.ConfirmedDate.Day, item.ConfirmedTime.Hour, item.ConfirmedTime.Minute,
                                         item.ConfirmedTime.Second);
                    this.CustomerDetails.Appointments.Add(

                                  new ScheduleAppointment()
                                  {
                                      Subject = item.CaseNumber,
                                      Location = item.Address,
                                      StartTime = startTime,
                                      EndTime = startTime.AddHours(1),
                                      ReadOnly = true,
                                      AppointmentBackground = new SolidColorBrush(Colors.Crimson),
                                      Status = new ScheduleAppointmentStatus { Status = item.Status, Brush = new SolidColorBrush(Colors.Chocolate) }

                                  }
                             );

                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Properties
        private bool allowEditing;
        [RestorableState]
        public bool AllowEditing
        {
            get { return allowEditing; }
            set { SetProperty(ref allowEditing, value); }
        }

        private ObservableCollection<Pithline.FMS.BusinessLogic.Task> inspectionList;

        public ObservableCollection<Pithline.FMS.BusinessLogic.Task> InspectionList
        {
            get { return inspectionList; }
            set { SetProperty(ref inspectionList, value); }
        }



        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }


        private NavigationMode navigationMode;

        public NavigationMode NavigationMode
        {
            get { return navigationMode; }
            set { SetProperty(ref navigationMode, value); }
        }


        private bool isCommandBarOpen;
        [RestorableState]
        public bool IsCommandBarOpen
        {
            get { return isCommandBarOpen; }
            set { SetProperty(ref isCommandBarOpen, value); }
        }

        private ObservableCollection<Pithline.FMS.BusinessLogic.Task> poolofTasks;

        public ObservableCollection<Pithline.FMS.BusinessLogic.Task> PoolofTasks
        {
            get { return poolofTasks; }
            set
            {
                SetProperty(ref poolofTasks, value);
            }
        }

        private Customer customer;

        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
        }

        private ScheduleAppointmentCollection appointments;

        public ScheduleAppointmentCollection Appointments
        {
            get { return appointments; }
            set { SetProperty(ref appointments, value); }
        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }
        private Pithline.FMS.BusinessLogic.Task task;
        public Pithline.FMS.BusinessLogic.Task InspectionTask
        {
            get { return task; }
            set
            {
                if (SetProperty(ref task, value))
                {
                    SaveCommand.RaiseCanExecuteChanged();
                    DrivingDirectionCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand DrivingDirectionCommand { get; set; }

        public DelegateCommand SaveCommand { get; set; }

        #endregion

        #region Methods
        async public System.Threading.Tasks.Task GetCustomerDetailsAsync(bool isAppBarOpen)
        {
            try
            {
                if (this.InspectionTask != null)
                {
                    AppSettings.Instance.IsSyncingCustDetails = 0;
                    this.CustomerDetails.ContactNumber = this.InspectionTask.ContactNumber;
                    this.CustomerDetails.CaseNumber = this.InspectionTask.CaseNumber;
                    this.CustomerDetails.VehicleInsRecId = this.InspectionTask.VehicleInsRecId;
                    this.CustomerDetails.Status = this.InspectionTask.Status;
                    this.CustomerDetails.StatusDueDate = this.InspectionTask.StatusDueDate;
                    this.CustomerDetails.Address = this.InspectionTask.Address;
                    this.CustomerDetails.AllocatedTo = this.InspectionTask.AllocatedTo;
                    this.CustomerDetails.CustomerName = this.InspectionTask.CustomerName;
                    this.CustomerDetails.ContactName = this.InspectionTask.ContactName;
                    this.CustomerDetails.CategoryType = this.InspectionTask.CategoryType;
                    this.CustomerDetails.EmailId = this.InspectionTask.Email;

                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

    }
}
