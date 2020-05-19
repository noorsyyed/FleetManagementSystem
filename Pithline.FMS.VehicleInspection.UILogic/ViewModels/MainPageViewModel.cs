using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Commercial;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.Passenger;
using Pithline.FMS.VehicleInspection.UILogic.AifServices;
using Pithline.FMS.VehicleInspection.UILogic.VIService;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        IEventAggregator _eventAggregator;
        UserInfo _userInfo;

        public MainPageViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;

            this.PoolofTasks = new ObservableCollection<BusinessLogic.Task>();
            this.Appointments = new ScheduleAppointmentCollection();
            //this.Appointments = new ScheduleAppointmentCollection
            //{
            //    new ScheduleAppointment(){
            //        Subject = "Inspection at Peter Johnson",
            //        Notes = "some noise from engine",
            //        Location = "Cape Town",
            //        StartTime = DateTime.Now,
            //        EndTime = DateTime.Now.AddHours(2),
            //        ReadOnly = true,
            //       AppointmentBackground = new SolidColorBrush(Colors.Crimson),                   
            //        Status = new ScheduleAppointmentStatus{Status = "Tentative",Brush = new SolidColorBrush(Colors.Chocolate)}

            //    },
            //    new ScheduleAppointment(){
            //        Subject = "Inspection at Daren May",
            //        Notes = "some noise from differential",
            //        Location = "Cape Town",
            //         ReadOnly = true,
            //        StartTime =new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,8,00,00),
            //        EndTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,9,00,00),
            //        Status = new ScheduleAppointmentStatus{Brush = new SolidColorBrush(Colors.Green), Status  = "Free"},
            //    },                    
            //};

            this.BingWeatherCommand = new DelegateCommand(() =>
            {

            });

            this.SyncCommand = new DelegateCommand<string>((vehicleType) =>
            {
                if (AppSettings.Instance.IsSynchronizing == 0)
                {
                    VIServiceHelper.Instance.Synchronize(async () =>
                            {
                                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                {
                                    AppSettings.Instance.ErrorMessage = string.Empty;
                                    AppSettings.Instance.IsSynchronizing = 1;
                                });

                                await VIServiceHelper.Instance.SyncTasksFromSvcAsync();
                                _eventAggregator.GetEvent<TasksFetchedEvent>().Publish(this.PoolofTasks);

                                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                                {
                                    this.PoolofTasks.Clear();
                                    await GetTasksFromDbAsync();
                                    GetAllCount();
                                    GetAppointments();
                                    //AppSettings.Instance.IsSynchronizing = 0;
                                    AppSettings.Instance.Synced = true;
                                });

                            });
                    VIServiceHelper.Instance.Synchronize(async () =>
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                            AppSettings.Instance.IsSynchronizing = 1;
                        });

                        switch (vehicleType)
                        {
                            case "Passenger":
                                await VIServiceHelper.Instance.SyncPassengerAsync();
                                break;
                            case "Commercial": await VIServiceHelper.Instance.SyncCommercialAsync(); break;
                            case "Trailer": await VIServiceHelper.Instance.SyncTrailerAsync(); break;
                            default:
                                break;
                        }


                        await VIServiceHelper.Instance.SyncImagesAsync();
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                            AppSettings.Instance.IsSynchronizing = 0;
                            AppSettings.Instance.Synced = true;
                        });

                    });

                }
            });

        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {

            try
            {
                _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());

                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                await CreateTableAsync();
                //SyncData();
                var weather = await SqliteHelper.Storage.LoadTableAsync<WeatherInfo>();
                this.WeatherInfo = weather.FirstOrDefault();

                await GetTasksFromDbAsync();
                GetAllCount();

                GetAppointments();

                if (AppSettings.Instance.IsSynchronizing == 0 && !AppSettings.Instance.Synced)
                {
                    VIServiceHelper.Instance.Synchronize(async () =>
                       {
                           await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                           {

                               AppSettings.Instance.IsSynchronizing = 1;
                           });

                           await VIServiceHelper.Instance.SyncTasksFromSvcAsync();

                           _eventAggregator.GetEvent<TasksFetchedEvent>().Publish(this.PoolofTasks);
                           await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                                 {
                                     this.PoolofTasks.Clear();
                                     await GetTasksFromDbAsync();

                                     GetAllCount();
                                     GetAppointments();
                                 });

                           await VIServiceHelper.Instance.SyncImagesAsync();

                           await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                {
                                    AppSettings.Instance.IsSynchronizing = 0;
                                    AppSettings.Instance.Synced = true;
                                });

                       });
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        private void GetAllCount()
        {

            this.AwaitingConfirmationCount = this.PoolofTasks.Count(x => Regex.Replace(x.Status, @"\s", "").ToLower() == BusinessLogic.Helpers.TaskStatus.AwaitingInspectionConfirmation.ToLower() || x.Status == Pithline.FMS.BusinessLogic.Helpers.TaskStatus.AwaitCollectionConfirmation);
            this.MyTasksCount = this.PoolofTasks.Count(x => x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture, StringComparison.OrdinalIgnoreCase) || x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitCollectionDataCapture, StringComparison.OrdinalIgnoreCase));
            this.TotalCount = this.PoolofTasks.Count(x => DateTime.Equals(x.ConfirmedDate, DateTime.Today) && (x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture) || x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitCollectionDataCapture, StringComparison.OrdinalIgnoreCase)));
        }

        private void GetAppointments()
        {
            this.Appointments.Clear();
            foreach (var item in this.PoolofTasks.Where(x => x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture) || x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionAcceptance)))
            {

                var startTime = new DateTime(item.ConfirmedDate.Year, item.ConfirmedDate.Month, item.ConfirmedDate.Day, item.ConfirmedTime.Hour, item.ConfirmedTime.Minute,
                                            item.ConfirmedTime.Second);
                this.Appointments.Add(

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
        }

        private async System.Threading.Tasks.Task GetTasksFromDbAsync()
        {
            var list = (await SqliteHelper.Storage.LoadTableAsync<Pithline.FMS.BusinessLogic.Task>()).Where(w => w.Status != Pithline.FMS.BusinessLogic.Helpers.TaskStatus.AwaitDamageConfirmation);
            foreach (Pithline.FMS.BusinessLogic.Task item in list.Where(x => x.UserId == _userInfo.UserId))
            {
                //var cust = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(x => x.Id.Equals(item.CustomerId));
                //if (cust != null)
                //{
                //    item.CustomerName = cust.CustomerName;
                //    item.Address = cust.Address;
                //}

                //item.ConfirmedDate = DateTime.Now;
                //if (item.Status == BusinessLogic.Helpers.TaskStatus.AwaitInspectionDetail)
                //{
                //    item.ConfirmedTime = DateTime.Now.AddHours(list.IndexOf(item));
                //}
                //if (item.Status != BusinessLogic.Helpers.TaskStatus.AwaitingInspection)
                //{
                //    this.Appointments.Add(new ScheduleAppointment
                //           {
                //               Subject = "Inspection at " + item.CustomerName,
                //               StartTime = item.ConfirmedTime,
                //               EndTime = item.ConfirmedTime.AddHours(1),
                //               Location = item.Address,
                //               ReadOnly = true,
                //               Status = new ScheduleAppointmentStatus { Brush = new SolidColorBrush(Colors.LightGreen), Status = "Free" },
                //           });
                //}
                //AppSettingData.Appointments = this.Appointments;
                this.PoolofTasks.Add(item);
            }
        }
        async private void SyncData()
        {
            await BackgroundExecutionManager.RequestAccessAsync();
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.TaskEntryPoint = "Pithline.FMS.VehicleInspection.BackgroundTask.SilentSync";
            builder.SetTrigger(new TimeTrigger(15, false));
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            builder.Name = "SilentSync";
            var task = builder.Register();
            task.Completed += task_Completed;
        }

        void task_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {

        }
        async private void SyncTaskFromService()
        {
            await BackgroundExecutionManager.RequestAccessAsync();
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.TaskEntryPoint = "Pithline.FMS.VehicleInspection.UILogic.ServiceBackgroundTask";
            builder.SetTrigger(new TimeTrigger(15, false));
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            builder.Name = "ServiceBackgroundTask";
            var taskfromService = builder.Register();
            taskfromService.Completed += taskfromService_Completed;
        }
        void taskfromService_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            throw new NotImplementedException();
        }



        private WeatherInfo weatherInfo;

        public WeatherInfo WeatherInfo
        {
            get { return weatherInfo; }
            set { SetProperty(ref weatherInfo, value); }
        }

        private int total;
        [RestorableState]
        public int TotalCount
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }

        private int awaitingConfirmationCount;
        [RestorableState]
        public int AwaitingConfirmationCount
        {
            get { return awaitingConfirmationCount; }
            set { SetProperty(ref awaitingConfirmationCount, value); }
        }
        private int myTasksCount;
        [RestorableState]
        public int MyTasksCount
        {
            get { return myTasksCount; }
            set { SetProperty(ref myTasksCount, value); }
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

        private ScheduleAppointmentCollection appointments;
        public ScheduleAppointmentCollection Appointments
        {
            get { return appointments; }
            set { SetProperty(ref appointments, value); }
        }
        public DelegateCommand BingWeatherCommand { get; set; }

        public ICommand SyncCommand { get; set; }


        /// <summary>
        ///  this is  Temporary method for create tables in DB
        /// </summary>

        private async System.Threading.Tasks.Task CreateTableAsync()
        {

            ////Drop Existing tables

          
           // await SqliteHelper.Storage.DropnCreateTableAsync<Pithline.FMS.BusinessLogic.Task>();

            //await SqliteHelper.Storage.DropTableAsync<Pithline.FMS.BusinessLogic.Customer>();
            //await SqliteHelper.Storage.CreateTableAsync<Pithline.FMS.BusinessLogic.Customer>();


            //await SqliteHelper.Storage.DropTableAsync<PVehicleDetails>();
            //await SqliteHelper.Storage.DropTableAsync<PTyreCondition>();
            //await SqliteHelper.Storage.DropTableAsync<PMechanicalCond>();
            //await SqliteHelper.Storage.DropTableAsync<PInspectionProof>();
            //await SqliteHelper.Storage.DropTableAsync<PGlass>();
            //await SqliteHelper.Storage.DropTableAsync<PBodywork>();
            //await SqliteHelper.Storage.DropTableAsync<PTrimInterior>();
            //await SqliteHelper.Storage.DropTableAsync<PAccessories>();

            //await SqliteHelper.Storage.DropTableAsync<CVehicleDetails>();
            //await SqliteHelper.Storage.DropTableAsync<CTyres>();
            //await SqliteHelper.Storage.DropTableAsync<CAccessories>();
            //await SqliteHelper.Storage.DropTableAsync<CChassisBody>();
            //await SqliteHelper.Storage.DropTableAsync<CGlass>();
            //await SqliteHelper.Storage.DropTableAsync<CMechanicalCond>();
            //await SqliteHelper.Storage.DropTableAsync<CPOI>();
            //await SqliteHelper.Storage.DropTableAsync<CCabTrimInter>();
            //await SqliteHelper.Storage.DropTableAsync<DrivingDuration>();

            ////create new  tables 

            //await SqliteHelper.Storage.CreateTableAsync<PVehicleDetails>();
            //await SqliteHelper.Storage.CreateTableAsync<PTyreCondition>();
            //await SqliteHelper.Storage.CreateTableAsync<PMechanicalCond>();
            //await SqliteHelper.Storage.CreateTableAsync<PInspectionProof>();
            //await SqliteHelper.Storage.CreateTableAsync<PGlass>();
            //await SqliteHelper.Storage.CreateTableAsync<PBodywork>();
            //await SqliteHelper.Storage.CreateTableAsync<PTrimInterior>();
            //await SqliteHelper.Storage.CreateTableAsync<PAccessories>();

            //await SqliteHelper.Storage.CreateTableAsync<CVehicleDetails>();
            //await SqliteHelper.Storage.CreateTableAsync<CTyres>();
            //await SqliteHelper.Storage.CreateTableAsync<CAccessories>();
            //await SqliteHelper.Storage.CreateTableAsync<CChassisBody>();
            //await SqliteHelper.Storage.CreateTableAsync<CGlass>();
            //await SqliteHelper.Storage.CreateTableAsync<CMechanicalCond>();
            //await SqliteHelper.Storage.CreateTableAsync<CPOI>();
            //await SqliteHelper.Storage.CreateTableAsync<CCabTrimInter>();
            //await SqliteHelper.Storage.CreateTableAsync<DrivingDuration>();




            //await SqliteHelper.Storage.DropTableAsync<TVehicleDetails>();
            //await SqliteHelper.Storage.DropTableAsync<TAccessories>();
            //await SqliteHelper.Storage.DropTableAsync<TChassisBody>();
            //await SqliteHelper.Storage.DropTableAsync<TGlass>();
            //await SqliteHelper.Storage.DropTableAsync<TMechanicalCond>();
            //await SqliteHelper.Storage.DropTableAsync<TPOI>();
            //await SqliteHelper.Storage.DropTableAsync<TTyreCond>();

            //await SqliteHelper.Storage.CreateTableAsync<TVehicleDetails>();
            //await SqliteHelper.Storage.CreateTableAsync<TAccessories>();
            //await SqliteHelper.Storage.CreateTableAsync<TChassisBody>();
            //await SqliteHelper.Storage.CreateTableAsync<TGlass>();
            //await SqliteHelper.Storage.CreateTableAsync<TMechanicalCond>();
            //await SqliteHelper.Storage.CreateTableAsync<TPOI>();
            //await SqliteHelper.Storage.CreateTableAsync<TTyreCond>();

            // await SqliteHelper.Storage.DropnCreateTableAsync<ImageCapture>();

        }

    }
}
