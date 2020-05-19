using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.DocumentDelivery;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.DocumentDelivery.Common;
using Pithline.FMS.DocumentDelivery.UILogic;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Pithline.FMS.DocumentDelivery.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : VisualStateAwarePage
    {
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private bool isCached;
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public MainPage()
        {
            this.InitializeComponent();


        }

        public void Dispose()
        {
            this.mainGrid.ItemsSource = null;
            this.mainGrid.Dispose();
            (this.DataContext as IDisposable).Dispose();
            this.DataContext = null;

        }
        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            try
            {
                var result = await Util.ReadFromDiskAsync<CollectDeliveryTask>("CDTaskFile.json");
                if (result != null)
                {
                    this.mainGrid.ItemsSource = result.Where(x => x.CustomerName.Equals(args.QueryText) ||
                                      Convert.ToString(x.DocumentCount).Equals(args.QueryText) || x.AllocatedTo.Equals(args.QueryText) ||
                                     Convert.ToString(x.TaskType).Equals(args.QueryText) || Convert.ToString(x.ConfirmedDate).Equals(args.QueryText) ||
                                     Convert.ToString(x.StatusDueDate).Equals(args.QueryText) || x.Status.Equals(args.QueryText) ||
                                     Convert.ToString(x.DeliveryDate).Equals(args.QueryText) || Convert.ToString(x.Address).Equals(args.QueryText) |
                                      Convert.ToString(x.CaseNumber).Equals(args.QueryText) || Convert.ToString(x.CDTaskStatus).Equals(args.QueryText)
                                      || Convert.ToString(x.CustomerId).Equals(args.QueryText) || Convert.ToString(x.ContactName).Equals(args.QueryText)
                                      || Convert.ToString(x.CustPartyId).Equals(args.QueryText) || Convert.ToString(x.EmailId).Equals(args.QueryText)
                                     );


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
                    var deferral = args.Request.GetDeferral();
                    if (!string.IsNullOrEmpty(args.QueryText))
                    {
                        if (!isCached)
                        {
                            await Util.WriteToDiskAsync(JsonConvert.SerializeObject(this.mainGrid.ItemsSource), "CDTaskFile.json");
                            isCached = true;
                        }

                        var searchSuggestionList = new List<string>();
                        foreach (var task in await Util.ReadFromDiskAsync<CollectDeliveryTask>("CDTaskFile.json"))
                        {
                            foreach (var propInfo in task.GetType().GetRuntimeProperties())
                            {
                                if (propInfo.PropertyType.Name.Equals(typeof(System.Boolean).Name) ||
                                    propInfo.PropertyType.Name.Equals(typeof(BindableValidator).Name))
                                    continue;

                                var propVal = Convert.ToString(propInfo.GetValue(task));
                                if (propVal.ToLowerInvariant().Contains(args.QueryText.ToLowerInvariant()))
                                {
                                    if (!searchSuggestionList.Contains(propVal))
                                        searchSuggestionList.Add(propVal);
                                }
                            }
                        }
                        args.Request.SearchSuggestionCollection.AppendQuerySuggestions(searchSuggestionList);
                    }
                    deferral.Complete();
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;

            }

        }
        private void ProfileUserControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element != null)
            {
                FlyoutBase.ShowAttachedFlyout(element);
            }
        }
    }
}
