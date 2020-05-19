using Pithline.FMS.TechnicalInspection.Common;
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
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Pithline.FMS.TechnicalInspection.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class InspectionDetailsPage : VisualStateAwarePage
    {

        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Windows.Storage.StorageFolder temporaryFolder = ApplicationData.Current.TemporaryFolder;
        private bool isCached;
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public InspectionDetailsPage()
        {
            this.InitializeComponent();
            this.SizeChanged += InspectionDetailsPage_SizeChanged;
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
            this.detailsGrid.ItemsSource = (await Util.ReadFromDiskAsync<Pithline.FMS.BusinessLogic.Task>("DetailsItemsSourceFile.txt")).Where(x => x.CaseCategory.Contains(args.QueryText) ||
                 x.CaseNumber.Contains(args.QueryText) ||
                 x.CaseType.ToString().Contains(args.QueryText) ||
                 x.CustomerName.Contains(args.QueryText) ||
                 x.RegistrationNumber.Contains(args.QueryText) ||
                 x.Status.ToString().Contains(args.QueryText));
        }

        async private void filterBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            if (!string.IsNullOrEmpty(args.QueryText))
            {
                if (!isCached)
                {
                    await Util.WriteToDiskAsync(JsonConvert.SerializeObject(this.detailsGrid.ItemsSource), "DetailsItemsSourceFile.txt");
                    isCached = true;
                }

                var searchSuggestionList = new List<string>();
                foreach (var task in await Util.ReadFromDiskAsync<Pithline.FMS.BusinessLogic.Task>("DetailsItemsSourceFile.txt"))
                {
                    foreach (var propInfo in task.GetType().GetRuntimeProperties())
                    {
                        if (propInfo.PropertyType.Name.Equals(typeof(System.Boolean).Name) || propInfo.PropertyType.Name.Equals(typeof(BindableValidator).Name) || propInfo.Name.Equals("Address"))
                            continue;
                        var propVal = Convert.ToString( propInfo.GetValue(task));
                        if (propVal.ToLowerInvariant().Contains(args.QueryText))
                        {
                            searchSuggestionList.Add(propVal);
                        }
                    }
                }
                args.Request.SearchSuggestionCollection.AppendQuerySuggestions(searchSuggestionList);
            }
            deferral.Complete();

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
