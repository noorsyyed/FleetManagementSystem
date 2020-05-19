using Pithline.FMS.ServiceScheduling.Common;
using Pithline.FMS.ServiceScheduling.UILogic.Portable;
using Pithline.FMS.ServiceScheduling.Views;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.FMS.ServiceScheduling.WindowsPhone.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ServiceSchedulingPage : VisualStateAwarePage
    {


        public ServiceSchedulingPage()
        {
            this.InitializeComponent();

            this.SizeChanged += ServiceSchedulingPage_SizeChanged;
        }

        void ServiceSchedulingPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (DisplayInformation.GetForCurrentView().CurrentOrientation == DisplayOrientations.Landscape ||

                DisplayInformation.GetForCurrentView().CurrentOrientation == DisplayOrientations.LandscapeFlipped)
            {
                ((ServiceSchedulingPageViewModel)this.DataContext).BoundWidth = Window.Current.Bounds.Width - 180;
                ((ServiceSchedulingPageViewModel)this.DataContext).BoundMinWidth = Window.Current.Bounds.Width - 240;
            }
            else
            {
                ((ServiceSchedulingPageViewModel)this.DataContext).BoundWidth = Window.Current.Bounds.Width - 30;
                ((ServiceSchedulingPageViewModel)this.DataContext).BoundMinWidth = Window.Current.Bounds.Width - 70;

            }

        }
        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        private async void Calendar_Click(object sender, RoutedEventArgs e)
        {
            await AppointmentManager.ShowTimeFrameAsync(DateTime.Today, TimeSpan.FromDays(7));
        }

        async private void ddLocationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = this.DataContext as ServiceSchedulingPageViewModel;
            try
            {

                vm._busyIndicator.Open("Please wait,loading destination types ...");
                if (vm != null)
                {
                    if (vm.Model.SelectedLocationType.LocType == "Other")
                    {
                        vm.IsEnabledDesType = false;
                        vm.DestinationTypes = new System.Collections.ObjectModel.ObservableCollection<BusinessLogic.Portable.SSModels.DestinationType>();
                        vm.AddVisibility = Visibility.Visible;
                        vm.Model.Address = string.Empty;
                    }
                    else
                    {
                        vm.IsEnabledDesType = true;
                        vm.AddVisibility = Visibility.Collapsed;

                        if (vm.DestinationTypes != null)
                        {
                            try
                            {
                                vm.Model.SelectedDestinationType = new BusinessLogic.Portable.SSModels.DestinationType();
                                vm.DestinationTypes = new System.Collections.ObjectModel.ObservableCollection<BusinessLogic.Portable.SSModels.DestinationType>();
                            }
                            catch (Exception)
                            {
                             
                            }
                        }
                        vm.Model.Address = string.Empty;

                        if (vm.SelectedTask != null && vm.Model.SelectedLocationType != null)
                        {
                            vm.DestinationTypes = await vm._serviceDetailService.GetDestinationTypeList(vm.Model.SelectedLocationType.LocType, vm.SelectedTask.CustomerId, vm.UserInfo);
                        }

                    }
                }
                vm._busyIndicator.Close();
            }
            catch (Exception)
            {
                vm._busyIndicator.Close();

            }
        }

        private void ddDestinationTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = this.DataContext as ServiceSchedulingPageViewModel;
            if (vm != null)
            {
                vm.Model.Address = vm.Model.SelectedDestinationType.Address;
            }

        }

    }
}
