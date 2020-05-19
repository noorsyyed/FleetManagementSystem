using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using System.Linq;
using Windows.Storage;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Pithline.FMS.BusinessLogic.Portable;
namespace Pithline.FMS.ServiceScheduling.UILogic.Portable
{
    public class MainPageViewModel : ViewModel
    {
        public INavigationService _navigationService;
        private ITaskService _taskService;
        public MainPageViewModel(INavigationService navigationService, ITaskService taskService)
        {
            this._navigationService = navigationService;
            this._taskService = taskService;

            this.PoolofTasks = new ObservableCollection<BusinessLogic.Portable.SSModels.Task>();
            this.NextPageCommand = new DelegateCommand<Task>((task) =>
             {
                 try
                 {
                     ApplicationData.Current.RoamingSettings.Values[Constants.SELECTEDTASK] = JsonConvert.SerializeObject(task);
                     if (task != null && task.Status == DriverTaskStatus.AwaitServiceBookingDetail)
                     {
                         navigationService.Navigate("ServiceScheduling", String.Empty);
                     }
                     else
                     {

                         navigationService.Navigate("PreferredSupplier", String.Empty);

                     }
                 }
                 catch (Exception ex)
                 {
                 }
                 finally
                 {

                 }
             }
                   );
            this.RefreshTaskCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                await this.FetchTasks();
            });


            this.MakeCallCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                if (!String.IsNullOrEmpty(this.InspectionTask.CustPhone))
                {
                    await Launcher.LaunchUriAsync(new Uri("callto:" + this.InspectionTask.CustPhone));
                }
                else
                {
                    await new MessageDialog("no phone number exist").ShowAsync();
                }
            }, () =>
            {
                return (this.InspectionTask != null && !string.IsNullOrEmpty(this.InspectionTask.CustPhone));
            });

            this.MailToCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                if (!String.IsNullOrEmpty(this.InspectionTask.CusEmailId))
                {
                    await Launcher.LaunchUriAsync(new Uri("mailto:" + this.InspectionTask.CusEmailId));
                }
                else
                {
                    await new MessageDialog("no mail id exist").ShowAsync();
                }
            }, () => { return (this.InspectionTask != null && !string.IsNullOrEmpty(this.InspectionTask.CusEmailId)); });


            this.LocateCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                if (!String.IsNullOrEmpty(this.InspectionTask.Address))
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri("bingmaps:?where=" + Regex.Replace(this.InspectionTask.Address, "\n", ",")));
                }
                else
                {
                    await new MessageDialog("no address exist").ShowAsync();
                }
            }, () =>
            {
                return (this.InspectionTask != null && !string.IsNullOrEmpty(this.InspectionTask.Address));
            });
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
        private Pithline.FMS.BusinessLogic.Portable.SSModels.Task task;
        public Pithline.FMS.BusinessLogic.Portable.SSModels.Task InspectionTask
        {
            get { return task; }
            set
            {
                SetProperty(ref task, value);
            }
        }

        private ObservableCollection<Pithline.FMS.BusinessLogic.Portable.SSModels.Task> poolofTasks;
        public ObservableCollection<Pithline.FMS.BusinessLogic.Portable.SSModels.Task> PoolofTasks
        {
            get { return poolofTasks; }
            set
            {
                SetProperty(ref poolofTasks, value);
            }
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

                if ((PersistentData.Instance.PoolofTasks != null && PersistentData.Instance.PoolofTasks.Any()))
                {
                    this.PoolofTasks = PersistentData.Instance.PoolofTasks;
                }
                await FetchTasks();
            }
            catch (Exception)
            {
                this.TaskProgressBar = Visibility.Collapsed;

            }

        }
        public async System.Threading.Tasks.Task FetchTasks()
        {
            try
            {
                this.TaskProgressBar = Visibility.Visible;

                var tasksResult = await this._taskService.GetTasksAsync(this.UserInfo);
                ObservableCollection<Pithline.FMS.BusinessLogic.Portable.SSModels.Task> pooltask = new ObservableCollection<Task>();
                if (tasksResult != null)
                {
                    foreach (var task in tasksResult)
                    {
                        if (task.Status==DriverTaskStatus.AwaitServiceBookingDetail)
                        {
                            if(!task.CustPhone.Contains("+"))
                            {
                                task.CustPhone = "+" + task.CustPhone;
                            }
                            if (!task.DriverPhone.Contains("+"))
                            {
                                task.DriverPhone = "+" + task.DriverPhone;
                            }
                            if (task.ConfirmedDate.Contains("1900"))
                            {
                                task.ConfirmedDate = string.Empty;
                            }
                            task.Address = Regex.Replace(task.Address, ",", "\n");
                            pooltask.Add(task); 
                        }
                    }
                }
                this.PoolofTasks = pooltask;
                this.TaskProgressBar = Visibility.Collapsed;

                PersistentData.Instance.PoolofTasks = this.PoolofTasks;
            }
            catch (Exception)
            {
                this.TaskProgressBar = Visibility.Collapsed;

            }

        }
        private void GetAppointments(Pithline.FMS.BusinessLogic.Portable.SSModels.Task task)
        {
            try
            {

                var appointment = new Windows.ApplicationModel.Appointments.Appointment();

                var date = task.AppointmentStart.Date;
                var time = task.AppointmentEnd.TimeOfDay - task.AppointmentStart.TimeOfDay;
                var timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
                var startTime = new DateTimeOffset(date.Year, date.Month, date.Day, time.Hours,
                    time.Minutes, 0, timeZoneOffset);
                appointment.StartTime = startTime;
                appointment.Subject = task.CaseNumber;
                appointment.Location = task.Address;
                appointment.Details = task.Description;
                appointment.Duration = TimeSpan.FromHours(1);
                appointment.Reminder = TimeSpan.FromMinutes(15);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public UserInfo UserInfo { get; set; }
        public DelegateCommand RefreshTaskCommand { get; set; }

        public DelegateCommand MailToCommand { get; set; }

        public DelegateCommand MakeIMCommand { get; set; }

        public DelegateCommand LocateCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public DelegateCommand MakeCallCommand { get; set; }

    }
}
