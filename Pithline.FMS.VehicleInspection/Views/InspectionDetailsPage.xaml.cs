using Pithline.FMS.VehicleInspection.Common;
using Pithline.FMS.VehicleInspection.UILogic.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic;
using Syncfusion.UI.Xaml.Grid;
using Pithline.FMS.VehicleInspection.UILogic;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Pithline.FMS.VehicleInspection.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class InspectionDetailsPage : VisualStateAwarePage
    {

        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Windows.Storage.StorageFolder temporaryFolder = ApplicationData.Current.TemporaryFolder;
        private bool isCached;
        private List<string> suggestLookup = new List<string>();
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public InspectionDetailsPage()
        {
            this.InitializeComponent();
            this.SizeChanged += InspectionDetailsPage_SizeChanged;
            suggestLookup.Add("CaseNumber");
            suggestLookup.Add("CategoryType");
            suggestLookup.Add("Status");
            suggestLookup.Add("StatusDueDate");
            suggestLookup.Add("CustomerName");
            suggestLookup.Add("ContactName");
            suggestLookup.Add("ContactNumber");
        }
        void InspectionDetailsPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (e.NewSize.Width < 850.0)
            //{
            //    // VisualStateManager.GoToState(this, "SnapedLayout", true);
            //    this.detailsGrid.Visibility = Visibility.Collapsed;
            //    this.snapedListView.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    //VisualStateManager.GoToState(this, "DefaultLayout", true);
            //    this.snapedListView.Visibility = Visibility.Collapsed;
            //    this.detailsGrid.Visibility = Visibility.Visible;
            //}
        }
        async private void sfDataGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            try
            {
                var dc = (InspectionDetailsPageViewModel)this.DataContext;
                await dc.GetCustomerDetailsAsync(false);

            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message);
            }

        }
        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            var result = ((InspectionDetailsPageViewModel)this.DataContext).PoolofTasks;
            if (result != null)
            {
                try
                {
                    this.detailsGrid.ItemsSource = result.Where(x => Convert.ToString(x.CaseNumber).Contains(args.QueryText) ||
                                                               Convert.ToString(x.CategoryType).Contains(args.QueryText) ||
                                                               Convert.ToString(x.Status).Contains(args.QueryText) ||
                                                               Convert.ToString(x.StatusDueDate).Contains(args.QueryText) ||
                                                               Convert.ToString(x.ContactName).Contains(args.QueryText) ||
                                                               Convert.ToString(x.CustomerName).Contains(args.QueryText) ||
                                                               Convert.ToString(x.ContactNumber).Contains(args.QueryText));

                }
                catch (Exception ex)
                {
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }
            }
        }

        async private void filterBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            try
            {
                if (this.detailsGrid.ItemsSource != null)
                {
                    var deferral = args.Request.GetDeferral();
                    if (!string.IsNullOrEmpty(args.QueryText))
                    {
                        var searchSuggestionList = new List<string>();
                        foreach (var task in ((InspectionDetailsPageViewModel)this.DataContext).InspectionList)
                        {
                            foreach (var propInfo in task.GetType().GetRuntimeProperties())
                            {
                                if (this.suggestLookup.Contains(propInfo.Name))
                                {
                                    var propVal = Convert.ToString(propInfo.GetValue(task));
                                    if (propVal.ToLowerInvariant().Contains(args.QueryText))
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
                        this.detailsGrid.ItemsSource = ((InspectionDetailsPageViewModel)this.DataContext).InspectionList;
                    }
                    deferral.Complete();
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        async private void snapedListView_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                var dc = (InspectionDetailsPageViewModel)this.DataContext;
                await dc.GetCustomerDetailsAsync(true);

            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message);
            }

        }

    }
}
