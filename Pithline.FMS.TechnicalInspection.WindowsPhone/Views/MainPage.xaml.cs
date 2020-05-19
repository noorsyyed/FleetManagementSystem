using Pithline.FMS.BusinessLogic.Portable.TIModels;
using Pithline.FMS.TechnicalInspection.UILogic;
using Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.ViewModels;
using Pithline.FMS.TechnicalInspection.Views;
using Microsoft.Practices.Prism.StoreApps;
using ShakeGestures;
using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Appointments;
using Windows.ApplicationModel.Background;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.FMS.TechnicalInspection.WindowsPhone.Views
{
    public sealed partial class MainPage : VisualStateAwarePage
    {
        MainPageViewModel vm;
        DetailsDialog dd;
        UserProfile contentDialog;
        public MainPage()
        {
            this.InitializeComponent();

            ShakeGesturesHelper.Instance.ShakeGesture += new EventHandler<ShakeGestureEventArgs>(Instance_ShakeGesture);
            ShakeGesturesHelper.Instance.MinimumRequiredMovesForShake = 2;
            ShakeGesturesHelper.Instance.Active = true;

            double w;
            if (Window.Current.Bounds.Width < Window.Current.Bounds.Height)
            {
                w = (Window.Current.Bounds.Width / 2.2) - 10;
            }
            else
            {
                w = (Window.Current.Bounds.Height / 2.2) - 10;
            }

            Style style = this.Resources["GridViewItemStyle1"] as Style;
            style.Setters.Add(new Setter(GridViewItem.WidthProperty, w));
            style.Setters.Add(new Setter(GridViewItem.HeightProperty, w));
        }

        private async void Instance_ShakeGesture(object sender, ShakeGestures.ShakeGestureEventArgs e)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await vm.FetchTasksAsync();
            });
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (contentDialog != null)
            {
                contentDialog.Hide();
            }

            if (dd != null)
            {
                dd.Hide();
            }
        }
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            vm = ((MainPageViewModel)this.DataContext);
            base.OnNavigatedTo(e);
            RegisterBackgroundTask();
        }

        private async void RegisterBackgroundTask()
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        task.Value.Unregister(true);
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = taskName;
                taskBuilder.TaskEntryPoint = taskEntryPoint;
                taskBuilder.SetTrigger(new TimeTrigger(15, false));
                var registration = taskBuilder.Register();
            }
        }

        private const string taskName = "TIBackgroundTask";
        private const string taskEntryPoint = "Pithline.FMS.WinRT.Components.BackgroundTasks.WindowsPhone.TIBackgroundTask";

        async private void Message_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(vm.InspectionTask.CustPhone))
            {
                Windows.ApplicationModel.Chat.ChatMessage msg = new Windows.ApplicationModel.Chat.ChatMessage();
                msg.Body = "";
                msg.Recipients.Add(vm.InspectionTask.CustPhone);
                await Windows.ApplicationModel.Chat.ChatMessageManager.ShowComposeSmsMessageAsync(msg);
            }
            else
            {
                await new MessageDialog("No phone number exist").ShowAsync();
            }
        }

        private async void Calendar_Click(object sender, RoutedEventArgs e)
        {
            await AppointmentManager.ShowTimeFrameAsync(DateTime.Today, TimeSpan.FromDays(7));
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {

            var text = ((TextBox)sender).Text;
            if (!String.IsNullOrEmpty(text))
            {
                ObservableCollection<TITask> currentTasks;

                currentTasks = PersistentData.Instance.PoolofTasks;

                ObservableCollection<TITask> filterResult = new ObservableCollection<TITask>();
                foreach (var task in currentTasks)
                {
                    if (task.ContactName.ToLower().Contains(text.ToLower()) ||
                        task.CustomerName.ToLower().Contains(text.ToLower()) ||
                        task.RegistrationNumber.ToLower().Contains(text.ToLower()) ||
                        task.CaseNumber.ToLower().Contains(text.ToLower()))
                    {
                        filterResult.Add(task);
                    }
                }

                vm.PoolofTasks = filterResult;

            }
            else
            {
                vm.PoolofTasks = PersistentData.Instance.PoolofTasks;

            }
        }

        private void Flyout_Closed(object sender, object e)
        {
            FlyoutBase.ShowAttachedFlyout(filter);
        }

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(filter);
        }

        private void contextmenu_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            vm.InspectionTask = (TITask)senderElement.DataContext;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            vm.NextPageCommand.Execute(e.ClickedItem);
        }

        private void phone_Click(object sender, RoutedEventArgs e)
        {
            vm.MakeCallCommand.Execute();
        }

        private void mail_Click(object sender, RoutedEventArgs e)
        {
            vm.MailToCommand.Execute();
        }

        private void map_Click(object sender, RoutedEventArgs e)
        {
            vm.LocateCommand.Execute();
        }

        async private void Details_Click(object sender, RoutedEventArgs e)
        {
            dd = new DetailsDialog();
            dd.DataContext = this.vm.InspectionTask;
            await dd.ShowAsync();
        }

        private async void Profile_Click(object sender, RoutedEventArgs e)
        {
            contentDialog = new UserProfile(vm._navigationService);
            await contentDialog.ShowAsync();

        }

    }
}
