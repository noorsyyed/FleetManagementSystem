using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.TechnicalInspection.Common;
using Pithline.FMS.TechnicalInspection.UILogic;
using Pithline.FMS.TechnicalInspection.UILogic.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Background;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Pithline.FMS.TechnicalInspection.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : VisualStateAwarePage
    {
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private List<string> suggestLookup = new List<string>();

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public MainPage()
        {
            this.InitializeComponent();
            suggestLookup.Add("CaseNumber");
            suggestLookup.Add("CategoryType");
            suggestLookup.Add("Status");
            suggestLookup.Add("StatusDueDate");
            suggestLookup.Add("CustomerName");
            suggestLookup.Add("ContactName");
            suggestLookup.Add("ContactNumber");
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
        private const string taskEntryPoint = "Pithline.FMS.WinRT.Components.BackgroundTasks.TIBackgroundTask";

        private void UpdateVisualState()
        {
            //VisualStateManager.GoToState(this, ApplicationView.GetForCurrentView().Orientation.ToString(), true);
            //if (ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Portrait)
            //{
            //    this.FullView.Visibility = Visibility.Collapsed;
            //    this.SnapView.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    this.FullView.Visibility = Visibility.Visible;
            //    this.SnapView.Visibility = Visibility.Collapsed;
            //}
        }

        private void ProfileUserControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element != null)
            {
                FlyoutBase.ShowAttachedFlyout(element);
            }
        }
        async private void WeatherInfo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("bingweather:"));
        }
        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            try
            {
                var result = ((MainPageViewModel)this.DataContext).PoolofTasks;
                if (result != null)
                {
                    var filteredResult = result.Where(x => Convert.ToString(x.CaseNumber).Contains(args.QueryText) ||
                                                               Convert.ToString(x.CategoryType).Contains(args.QueryText) ||
                                                               Convert.ToString(x.Status).Contains(args.QueryText) ||
                                                               Convert.ToString(x.StatusDueDate).Contains(args.QueryText) ||
                                                               Convert.ToString(x.ContactName).Contains(args.QueryText) ||
                                                               Convert.ToString(x.CustomerName).Contains(args.QueryText) ||
                                                               Convert.ToString(x.ContactNumber).Contains(args.QueryText));
                    if (filteredResult != null)
                    {
                        this.mainGrid.ItemsSource = filteredResult;
                    }
                    else
                    {
                        this.mainGrid.ItemsSource = new ObservableCollection<TITask>();
                    }
                }

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        async private void filterBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(args.QueryText))
                {
                    var searchSuggestionList = new List<string>();
                    var deferral = args.Request.GetDeferral();
                    if (!string.IsNullOrEmpty(args.QueryText))
                    {
                        foreach (var task in ((MainPageViewModel)this.DataContext).PoolofTasks)
                        {
                            foreach (var propInfo in task.GetType().GetRuntimeProperties())
                            {
                                if (this.suggestLookup.Contains(propInfo.Name))
                                {
                                    var propVal = Convert.ToString(propInfo.GetValue(task));
                                    if (propVal.ToLowerInvariant().Contains(args.QueryText.ToLowerInvariant()))
                                    {
                                        if (!searchSuggestionList.Contains(propVal))
                                        {
                                            searchSuggestionList.Add(propVal);
                                        }

                                    }

                                }
                            }
                        }
                        args.Request.SearchSuggestionCollection.AppendQuerySuggestions(searchSuggestionList);

                    }
                    else
                    {
                        this.mainGrid.ItemsSource = ((MainPageViewModel)this.DataContext).PoolofTasks;
                    }
                    deferral.Complete();
                }

                else
                {
                    this.mainGrid.ItemsSource = ((MainPageViewModel)this.DataContext).PoolofTasks;
                }

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

    }
}
