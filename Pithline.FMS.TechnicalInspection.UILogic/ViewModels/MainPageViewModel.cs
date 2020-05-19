using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.DocumentDelivery;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.TI;
using Pithline.FMS.TechnicalInspection.UILogic.AifServices;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;

namespace Pithline.FMS.TechnicalInspection.UILogic.ViewModels
{

    public class MainPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IEventAggregator _eventAggregator;
        UserInfo _userInfo;

        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;
            this.PoolofTasks = new ObservableCollection<BusinessLogic.TITask>();
            this.Appointments = new ScheduleAppointmentCollection();
            _navigationService = navigationService;
            this._eventAggregator.GetEvent<Pithline.FMS.BusinessLogic.TITaskFetchedEvent>().Subscribe(async p =>
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await GetTasksFromDbAsync();
                    GetAllCount();
                    GetAppointments();
                });
            });
            DrivingDirectionCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                ApplicationData.Current.LocalSettings.Values["CaseNumber"] = this.InspectionTask.CaseNumber;
                ApplicationData.Current.LocalSettings.Values["CaseServiceRecID"] = this.InspectionTask.CaseServiceRecID;

                string jsonInspectionTask = JsonConvert.SerializeObject(this.InspectionTask);
                var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.VehicleInsRecID == this.InspectionTask.CaseServiceRecID);

                if (dd != null && !dd.StopDateTime.Equals(DateTime.MinValue))
                {
                    _navigationService.Navigate("TechnicalInspection", jsonInspectionTask);
                }
                else
                {
                    navigationService.Navigate("DrivingDirection", jsonInspectionTask);
                }

            }, () =>
            {
                return (this.InspectionTask != null);
                //return (this.InspectionTask != null && Regex.Replace(this.InspectionTask.Status.ToLower().Trim(), "\t", "") == Regex.Replace(Pithline.FMS.BusinessLogic.Helpers.TaskStatus.AwaitTechnicalInspection.ToLower().Trim(), "\t", ""));
            }
            );




            this.SyncCommand = new DelegateCommand(() =>
            {
                try
                {
                    if (AppSettings.Instance.IsSynchronizing == 0)
                    {
                        TIServiceHelper.Instance.Synchronize(async () =>
                        {
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                AppSettings.Instance.ErrorMessage = string.Empty;
                                AppSettings.Instance.IsSynchronizing = 1;
                            });

                            await TIServiceHelper.Instance.SyncTasksFromAXAsync();


                            TIServiceHelper.Instance.Synchronize();

                            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                //_eventAggregator.GetEvent<Pithline.FMS.BusinessLogic.TITaskFetchedEvent>().Publish(this.task);
                                await GetTasksFromDbAsync();
                                GetAllCount();
                                GetAppointments();
                                AppSettings.Instance.IsSynchronizing = 0;
                                AppSettings.Instance.Synced = true;
                            });

                        });


                        TIServiceHelper.Instance.Synchronize(async () =>
                        {
                            await TIServiceHelper.Instance.SyncImagesAsync();
                        });
                    }
                }
                catch (Exception ex)
                {
                    AppSettings.Instance.IsSynchronizing = 0;
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }
            });

        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {

            try
            {
                _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());

                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);



                await GetTasksFromDbAsync();
                GetAllCount();
                GetAppointments();


                if (AppSettings.Instance.IsSynchronizing == 0 && !AppSettings.Instance.Synced)
                {
                    TIServiceHelper.Instance.Synchronize(async () =>
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                            AppSettings.Instance.IsSynchronizing = 1;
                        });

                        await TIServiceHelper.Instance.SyncTasksFromAXAsync();

                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            await GetTasksFromDbAsync();
                            GetAllCount();
                            GetAppointments();

                            AppSettings.Instance.IsSynchronizing = 0;
                            AppSettings.Instance.Synced = true;
                        });

                    });


                    TIServiceHelper.Instance.Synchronize(async () =>
                   {
                       await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                       {

                           AppSettings.Instance.IsSynchronizing = 1;
                       });

                       await TIServiceHelper.Instance.Synchronize();

                       await TIServiceHelper.Instance.SyncImagesAsync();
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
            this.TotalCount = this.PoolofTasks.Where(x => x.ConfirmedDate.Date == DateTime.Today.Date).Count();
        }


        private void GetAppointments()
        {
            foreach (var item in this.PoolofTasks.Where(x => Regex.Replace(x.Status.ToLower().Trim(), "\t", "") == Regex.Replace(Pithline.FMS.BusinessLogic.Helpers.TaskStatus.AwaitTechnicalInspection.ToLower().Trim(), "\t", "")))
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
            PersistentData.Instance.CustomerDetails = new CustomerDetails();
            PersistentData.Instance.CustomerDetails.Appointments = this.Appointments;
        }

        private async System.Threading.Tasks.Task GetTasksFromDbAsync()
        {
            this.PoolofTasks.Clear();
            var list = (await SqliteHelper.Storage.LoadTableAsync<Pithline.FMS.BusinessLogic.TITask>()).Where(w => w.Status != Pithline.FMS.BusinessLogic.Helpers.TaskStatus.AwaitDamageConfirmation && w.Status != Pithline.FMS.BusinessLogic.Helpers.TaskStatus.Completed);
            foreach (Pithline.FMS.BusinessLogic.TITask item in list.Where(x => x.UserId == _userInfo.UserId))
            {
                this.PoolofTasks.Add(item);
            }
            this.InspectionTask = this.PoolofTasks.FirstOrDefault();
        }


        private Pithline.FMS.BusinessLogic.TITask task;
        public Pithline.FMS.BusinessLogic.TITask InspectionTask
        {
            get { return task; }
            set
            {
                if (SetProperty(ref task, value))
                {
                    DrivingDirectionCommand.RaiseCanExecuteChanged();
                }
            }
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

        private ObservableCollection<Pithline.FMS.BusinessLogic.TITask> poolofTasks;

        public ObservableCollection<Pithline.FMS.BusinessLogic.TITask> PoolofTasks
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

        public DelegateCommand SyncCommand { get; set; }

        public DelegateCommand DrivingDirectionCommand { get; set; }

    }
}

