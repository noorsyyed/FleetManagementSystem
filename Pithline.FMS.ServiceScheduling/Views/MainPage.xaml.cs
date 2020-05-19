using Pithline.FMS.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Pithline.FMS.BusinessLogic.ServiceSchedule;
using Pithline.FMS.ServiceScheduling.UILogic.ViewModels;
using Pithline.FMS.ServiceScheduling.UILogic;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Pithline.FMS.ServiceScheduling.UILogic.Helpers;
using Windows.ApplicationModel.Background;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pithline.FMS.ServiceScheduling.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : VisualStateAwarePage
    {
        private List<string> suggestLookup = new List<string>();

        public MainPage()
        {
            this.InitializeComponent();
            suggestLookup.Add("CaseNumber");
            suggestLookup.Add("StatusDueDate");
            suggestLookup.Add("Status");
            suggestLookup.Add("RegistrationNumber");
            suggestLookup.Add("Make");
            suggestLookup.Add("Model");
            suggestLookup.Add("CustomerName");
            suggestLookup.Add("ContactName");
            suggestLookup.Add("CustPhone");
        }

        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            try
            {
                if (PersistentData.Instance.PoolOfTask != null)
                {
                    var filterResult = PersistentData.Instance.PoolOfTask.Where(x => Convert.ToString(x.CaseNumber).Contains(args.QueryText) ||
                                                               Convert.ToString(x.StatusDueDate).Contains(args.QueryText) ||
                                                               Convert.ToString(x.Status).Contains(args.QueryText) ||
                                                               Convert.ToString(x.Make).Contains(args.QueryText) ||
                                                               Convert.ToString(x.Model).Contains(args.QueryText) ||
                                                               Convert.ToString(x.ContactName).Contains(args.QueryText) ||
                                                               Convert.ToString(x.CustomerName).Contains(args.QueryText) ||
                                                                Convert.ToString(x.CustPhone).Contains(args.QueryText) ||
                                                               Convert.ToString(x.RegistrationNumber).Contains(args.QueryText));

                    if (filterResult != null)
                    {
                        ((MainPageViewModel)this.DataContext).PoolofTasks.Clear();
                        foreach (var item in filterResult)
                        {
                            ((MainPageViewModel)this.DataContext).PoolofTasks.Add(item);
                        }
                    }
                    else
                    {
                        ((MainPageViewModel)this.DataContext).PoolofTasks.Clear();
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
                if (this.mainGrid.ItemsSource != null)
                {
                    var checkLookup = new List<string>();
                    var deferral = args.Request.GetDeferral();
                    var query = args.QueryText != null ? args.QueryText.Trim() : String.Empty;
                    if (!string.IsNullOrEmpty(args.QueryText))
                    {
                        foreach (var task in PersistentData.Instance.PoolOfTask)
                        {
                            foreach (var propInfo in task.GetType().GetRuntimeProperties())
                            {
                                if (this.suggestLookup.Contains(propInfo.Name))
                                {
                                    var propVal = Convert.ToString(propInfo.GetValue(task));
                                    if (propVal.ToLowerInvariant().Contains(query.ToLowerInvariant()))
                                    {
                                        if (!checkLookup.Contains(propVal))
                                        {
                                            checkLookup.Add(propVal);
                                        }

                                    }
                                }
                            }
                        }
                        args.Request.SearchSuggestionCollection.AppendQuerySuggestions(checkLookup);

                    }
                    else
                    {
                        ((MainPageViewModel)this.DataContext).PoolofTasks.Clear();
                        ((MainPageViewModel)this.DataContext).PoolofTasks.AddRange(PersistentData.Instance.PoolOfTask);
                    }
                    deferral.Complete();
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }

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

        private const string taskName = "SSBackgroundTask";
        private const string taskEntryPoint = "Pithline.FMS.WinRT.Components.BackgroundTasks.SSBackgroundTask";

    }
}
