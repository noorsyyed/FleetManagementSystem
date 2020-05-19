using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Pithline.FMS.ServiceScheduling
{

    public sealed partial class SearchSupplierDialog : ContentDialog
    {
        private IEventAggregator _eventAggregator;
        private ILocationService _locationService;
        public ISupplierService _supplierService;
        public ObservableCollection<Supplier> PoolofSupplier;
        public SearchSupplierDialog(ILocationService locationService, IEventAggregator eventAggregator, ISupplierService supplierService)
        {
            this._eventAggregator = eventAggregator;
            this.InitializeComponent();
            this._locationService = locationService;
            this._supplierService = supplierService;
            this.Loaded += SearchSupplierPopup_Loaded;
        }

        async void SearchSupplierPopup_Loaded(object sender, RoutedEventArgs e)
        {
            this.SupplierFilter = new Address();
            this.DataContext = this.SupplierFilter;
            this.SupplierFilter.ProgressVisibility = Visibility.Visible;
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
            {
                this.UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
            }
            this.SupplierFilter.Countries = await _locationService.GetCountryList(this.UserInfo);
            this.SupplierFilter.ProgressVisibility = Visibility.Collapsed;
        }



        private async void country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SupplierFilter.SelectedCountry != null)
            {
                this.SupplierFilter.ProgressVisibility = Visibility.Visible;

                this.SupplierFilter.Provinces = await _locationService.GetProvinceList(this.SupplierFilter.SelectedCountry.Id, this.UserInfo);
                this.SupplierFilter.ProgressVisibility = Visibility.Collapsed;
            }
        }

        private async void Provinces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SupplierFilter.Selectedprovince != null)
            {
                this.SupplierFilter.ProgressVisibility = Visibility.Visible;
                this.SupplierFilter.Cities = await _locationService.GetCityList(this.SupplierFilter.SelectedCountry.Id, this.SupplierFilter.Selectedprovince.Id, this.UserInfo);
                this.SupplierFilter.ProgressVisibility = Visibility.Collapsed;
            }
        }

        private async void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SupplierFilter.Selectedprovince != null)
            {
                this.SupplierFilter.ProgressVisibility = Visibility.Visible;

                this.SupplierFilter.Suburbs = await _locationService.GetSuburbList(this.SupplierFilter.SelectedCountry.Id, this.SupplierFilter.Selectedprovince.Id, this.UserInfo);
                this.SupplierFilter.ProgressVisibility = Visibility.Collapsed;
            }
        }

        private async void suburb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SupplierFilter.Selectedprovince != null)
            {
                this.SupplierFilter.ProgressVisibility = Visibility.Visible;
                this.SupplierFilter.Region = await _locationService.GetRegionList(this.SupplierFilter.SelectedCountry.Id, this.SupplierFilter.Selectedprovince.Id, this.UserInfo);
                this.SupplierFilter.ProgressVisibility = Visibility.Collapsed;
            }
        }

        private void region_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public Address SupplierFilter { get; set; }
        public UserInfo UserInfo { get; set; }


        async private void Accept_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                string countryId = this.SupplierFilter.SelectedCountry != null ? this.SupplierFilter.SelectedCountry.Id : string.Empty;
                string provinceId = this.SupplierFilter.Selectedprovince != null ? this.SupplierFilter.Selectedprovince.Id : string.Empty;
                string cityId = this.SupplierFilter.SelectedCity != null ? this.SupplierFilter.SelectedCity.Id : string.Empty;
                string suburbId = this.SupplierFilter.SelectedSuburb != null ? this.SupplierFilter.SelectedSuburb.Id : string.Empty;
                string regionId = this.SupplierFilter.SelectedRegion != null ? this.SupplierFilter.SelectedRegion.Id : string.Empty;
                this.PoolofSupplier = await this._supplierService.SearchSupplierByLocationAsync(countryId, provinceId, cityId, suburbId, regionId, this.UserInfo);

                _eventAggregator.GetEvent<SupplierFilterEvent>().Publish(this.PoolofSupplier);

            }
            catch (Exception ex)
            {

            }
        }

        private void cancel_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }


}
