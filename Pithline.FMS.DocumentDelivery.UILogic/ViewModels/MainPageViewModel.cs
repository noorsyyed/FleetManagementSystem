using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.DeliveryModel;
using Pithline.FMS.BusinessLogic.DocumentDelivery;
using Pithline.FMS.BusinessLogic.Enums;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.DocumentDelivery.UILogic.AifServices;
using Pithline.FMS.DocumentDelivery.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;

namespace Pithline.FMS.DocumentDelivery.UILogic.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        IEventAggregator _eventAggregator;
        public MainPageViewModel(IEventAggregator eventAggregator)
        {
            this.PoolofTasks = new ObservableCollection<CollectDeliveryTask>();
            this.Appointments = new ScheduleAppointmentCollection();
            _eventAggregator = eventAggregator;

            this.SyncCommand = new DelegateCommand(() =>
            {
                if (AppSettings.Instance.IsSynchronizing == 0)
                {

                    DDServiceProxyHelper.Instance.Synchronize(async () =>
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            AppSettings.Instance.ErrorMessage = string.Empty;
                            AppSettings.Instance.IsSynchronizing = 1;
                        });

                        await DDServiceProxyHelper.Instance.SynchronizeAllAsync();
                        await DDServiceProxyHelper.Instance.SyncTasksFromSvcAsync();
                        _eventAggregator.GetEvent<Pithline.FMS.BusinessLogic.DocumentDelivery.TasksFetchedEvent>().Publish(this.task);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            this.PoolofTasks.Clear();
                            await GetTasksFromDbAsync();
                            GetAllCount();
                            GetAppointments();
                            AppSettings.Instance.IsSynchronizing = 0;
                            AppSettings.Instance.Synced = true;
                        }
                         );

                        PersistentData.Instance.Appointments = this.Appointments;
                    });
                }
            });

        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {

                this.IsBusy = true;
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                if (this.UserInfo == null)
                {
                    this.UserInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                await GetTasksFromDbAsync();
                GetAllCount();
                GetAppointments();
                this.IsBusy = false;
                if (AppSettings.Instance.IsSynchronizing == 0 && !AppSettings.Instance.Synced)
                {
                    DDServiceProxyHelper.Instance.Synchronize(async () =>
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            AppSettings.Instance.IsSynchronizing = 1;
                        });

                        await DDServiceProxyHelper.Instance.SynchronizeAllAsync();
                        await DDServiceProxyHelper.Instance.SyncTasksFromSvcAsync();
                        _eventAggregator.GetEvent<Pithline.FMS.BusinessLogic.DocumentDelivery.TasksFetchedEvent>().Publish(this.task);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            this.PoolofTasks.Clear();
                            await GetTasksFromDbAsync();
                            GetAllCount();
                            GetAppointments();

                            AppSettings.Instance.IsSynchronizing = 0;
                            AppSettings.Instance.Synced = true;
                        });
                    });
                }

                PersistentData.Instance.Appointments = this.Appointments;

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                this.IsBusy = false;
            }
        }

        private int total;
        public int TotalCount
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }

        private int awaitingConfirmationCount;
        public int AwaitingConfirmationCount
        {
            get { return awaitingConfirmationCount; }
            set { SetProperty(ref awaitingConfirmationCount, value); }
        }

        private int myTaskCount;
        public int MyTaskCount
        {
            get { return myTaskCount; }
            set { SetProperty(ref myTaskCount, value); }
        }
        public DelegateCommand SyncCommand { get; set; }

        private CollectDeliveryTask task;
        public CollectDeliveryTask _cdTask
        {
            get { return task; }
            set
            {
                SetProperty(ref task, value);
            }
        }
        private ObservableCollection<CollectDeliveryTask> poolofTasks;
        public ObservableCollection<CollectDeliveryTask> PoolofTasks
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

        private CDUserInfo userInfo;

        public CDUserInfo UserInfo
        {
            get { return userInfo; }
            set { SetProperty(ref userInfo, value); }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        /// <summary>
        /// / testing temporary code.
        /// </summary>
        /// <returns></returns>


        private async System.Threading.Tasks.Task GetTasksFromDbAsync()
        {
            this.PoolofTasks.AddRange(await DDServiceProxyHelper.Instance.GroupTasksByCustomer());
        }

        private void GetAllCount()
        {
            this.AwaitingConfirmationCount = this.PoolofTasks.Where(x => !x.IsAssignTask).Count(x => x.Status == CDTaskStatus.AwaitCollectionDetail || x.Status == CDTaskStatus.AwaitCourierCollection || x.Status == CDTaskStatus.AwaitDriverCollection);
            this.MyTaskCount = this.PoolofTasks.Count(x => x.IsAssignTask && x.Status != CDTaskStatus.Completed);
            this.TotalCount = this.PoolofTasks.Count(x => x.IsAssignTask && x.DeliveryDate.Date.Equals(DateTime.Today));
        }
        private void GetAppointments()
        {

            foreach (var item in this.PoolofTasks.Where(x => !x.Status.Equals(CDTaskStatus.Completed)))
            {
                var startTime = new DateTime(item.DeliveryDate.Year, item.DeliveryDate.Month, item.DeliveryDate.Day, item.DeliveryDate.Hour, item.DeliveryDate.Minute,
                           item.DeliveryDate.Second);

                this.Appointments.Add(

                              new ScheduleAppointment()
                              {
                                  Subject = item.CaseNumber + Environment.NewLine + item.CustomerName,
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
    }
}
