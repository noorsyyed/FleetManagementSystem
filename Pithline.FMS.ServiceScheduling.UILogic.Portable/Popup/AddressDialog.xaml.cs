using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.FMS.ServiceScheduling
{
    public sealed partial class AddressDialog : ContentDialog
    {
        private ILocationService _locationService;
        private IEventAggregator _eventAggregator;
        public AddressDialog(ILocationService locationService, IEventAggregator eventAggregator, Address address)
        {
            this._locationService = locationService;
            this._eventAggregator = eventAggregator;
            this.Address = address;
            this.InitializeComponent();
            this.Loaded += AddressDialog_Loaded;

            var b = Window.Current.Bounds;
            this.scrlAdd.MaxHeight = b.Height - 100;
        }

        async private void AddressDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
            {
                this.UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
            }

            this.DataContext = this.Address;
            this.Address.ProgressVisibility = Visibility.Visible;
            this.Address.Countries = await _locationService.GetCountryList(this.UserInfo);
            this.Address.ProgressVisibility = Visibility.Collapsed;

        }

        private void OK_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                _eventAggregator.GetEvent<AddressFilterEvent>().Publish(this.Address);
            }
            catch (Exception ex)
            {

            }

        }

        private void Cancel_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Address.SelectedCountry != null)
            {
                this.Address.ProgressVisibility = Visibility.Visible;

                this.Address.Provinces = await _locationService.GetProvinceList(this.Address.SelectedCountry.Id, this.UserInfo);
                this.Address.ProgressVisibility = Visibility.Collapsed;
            }
        }

        private async void Provinces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Address.Selectedprovince != null)
            {
                this.Address.ProgressVisibility = Visibility.Visible;
                this.Address.Cities = await _locationService.GetCityList(this.Address.SelectedCountry.Id, this.Address.Selectedprovince.Id, this.UserInfo);
                this.Address.Postcodes = await _locationService.GetZipcodeList(this.Address.SelectedCountry.Id, this.Address.Selectedprovince.Id, this.UserInfo);
                this.Address.ProgressVisibility = Visibility.Collapsed;
            }
        }

        private async void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Address.Selectedprovince != null)
            {
                this.Address.ProgressVisibility = Visibility.Visible;

                this.Address.Suburbs = await _locationService.GetSuburbList(this.Address.SelectedCountry.Id, this.Address.Selectedprovince.Id, this.UserInfo);
                this.Address.ProgressVisibility = Visibility.Collapsed;
            }
        }

        private async void suburb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Address.Selectedprovince != null)
            {
                this.Address.ProgressVisibility = Visibility.Visible;
                this.Address.Region = await _locationService.GetRegionList(this.Address.SelectedCountry.Id, this.Address.Selectedprovince.Id, this.UserInfo);

                this.Address.ProgressVisibility = Visibility.Collapsed;
            }
        }

        private void region_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public UserInfo UserInfo { get; set; }
        public Address Address { get; set; }
    }
}
