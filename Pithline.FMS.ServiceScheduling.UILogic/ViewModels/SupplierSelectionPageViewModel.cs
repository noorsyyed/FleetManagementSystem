using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.ServiceSchedule;
using Pithline.FMS.ServiceScheduling.UILogic.AifServices;
using Pithline.FMS.ServiceScheduling.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;

namespace Pithline.FMS.ServiceScheduling.UILogic.ViewModels
{
    public class SupplierSelectionPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IEventAggregator _eventAggregator;
        
        public SupplierSelectionPageViewModel(INavigationService navigationSerive, IEventAggregator eventAggregator)
            : base(navigationSerive)
        {
            _navigationService = navigationSerive;
            _eventAggregator = eventAggregator;

            this.Model = new SupplierSelection();
            this.GoToConfirmationCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.IsBusy = true;
                    if (this.Model.ValidateProperties())
                    {
                        bool isinserted = await SSProxyHelper.Instance.InsertSelectedSupplierToSvcAsync(this.Model, this.DriverTask.CaseNumber, this.DriverTask.CaseServiceRecID);
                        _navigationService.Navigate("Confirmation", string.Empty);
                    }
                    this.IsBusy = false;
                }
                catch (Exception)
                {
                    this.IsBusy = false;
                    throw;
                }
            }, () =>
            {
                return (this.SelectedSupplier != null);
            });

            this.CountryChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                try
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Visible;
                    if ((param != null) && (param is Country))
                    {
                        Country country = param as Country;
                        if (!String.IsNullOrEmpty(country.Id) && this.Model.Provinces != null && !this.Model.Provinces.Any())
                        {

                            this.Model.Provinces = await SSProxyHelper.Instance.GetProvinceListFromSvcAsync(country.Id);

                            this.Model.SelectedCountry = country;
                            this.Model.Selectedprovince = null;
                        }
                    }
                    else
                    {
                        this.Model.SelectedCountry = null;
                        this.Model.Selectedprovince = null;
                    }
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                }
                catch (Exception)
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                }
            });

            this.ProvinceChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                try
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Visible;
                    if ((param != null) && (param is Province))
                    {
                        Province province = param as Province;
                        if (!String.IsNullOrEmpty(province.Id) && this.Model.Cities != null && !this.Model.Cities.Any())
                        {

                            this.Model.Cities = await SSProxyHelper.Instance.GetCityListFromSvcAsync(this.Model.SelectedCountry.Id, province.Id);

                            this.Model.Selectedprovince = province;
                            this.Model.SelectedCity = null;
                        }
                    }
                    else
                    {
                        this.Model.Selectedprovince = null;
                        this.Model.SelectedCity = null;
                    }
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                }
                catch (Exception)
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                    throw;
                }
            });

            this.CityChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                try
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Visible;
                    if ((param != null) && (param is City))
                    {
                        City city = param as City;
                        if (!String.IsNullOrEmpty(city.Id) && this.Model.Suburbs != null && !this.Model.Suburbs.Any())
                        {

                            this.Model.Suburbs = await SSProxyHelper.Instance.GetSuburbListFromSvcAsync(this.Model.SelectedCountry.Id, city.Id);

                            this.Model.SelectedCity = city;
                            this.Model.SelectedSuburb = null;
                        }
                    }
                    else
                    {
                        this.Model.SelectedCity = null;
                        this.Model.SelectedSuburb = null;
                    }
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                }
                catch (Exception)
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                    throw;
                }

            });

            this.SuburbChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                try
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Visible;
                    if ((param != null) && (param is Suburb))
                    {
                        if (this.Model.Selectedprovince != null && this.Model.Regions != null && !this.Model.Regions.Any())
                        {

                            this.Model.Regions = await SSProxyHelper.Instance.GetRegionListFromSvcAsync(this.Model.SelectedCountry.Id, this.Model.Selectedprovince.Id);

                            this.Model.SelectedSuburb = (Suburb)param;
                        }
                    }
                    else
                    {
                        this.Model.SelectedSuburb = null;
                        this.Model.SelectedRegion = null;
                    }
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                }
                catch (Exception)
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                    throw;
                }

            });

            this.RegionChangedCommand = new DelegateCommand<object>((param) =>
            {
                try
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Visible;
                    if ((param != null) && (param is Region))
                    {
                        this.Model.SelectedRegion = (Region)param;
                    }
                    else
                    {
                        this.Model.SelectedRegion = null;
                    }
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                }
                catch (Exception)
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                    throw;
                }
            });

            this.SubmitQueryCommand = new DelegateCommand<string>((param) =>
            {

            });

            this.SupplierFilterCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.IsBusy = true;
                    string countryId = this.Model.SelectedCountry != null ? this.Model.SelectedCountry.Id : string.Empty;
                    string provinceId = this.Model.Selectedprovince != null ? this.Model.Selectedprovince.Id : string.Empty;
                    string cityId = this.Model.SelectedCity != null ? this.Model.SelectedCity.Id : string.Empty;
                    string suburbId = this.Model.SelectedSuburb != null ? this.Model.SelectedSuburb.Id : string.Empty;
                    string regionId = this.Model.SelectedRegion != null ? this.Model.SelectedRegion.Id : string.Empty;

                    this.Model.Suppliers = await SSProxyHelper.Instance.FilterSuppliersByCriteria(countryId, provinceId, cityId, suburbId, regionId);

                    this.IsBusy = false;
                }
                catch (Exception ex)
                {
                    AppSettings.Instance.ErrorMessage = ex.Message;
                    this.IsBusy = false;
                }
            }
            );
        }

        private async System.Threading.Tasks.Task DefaultSuplierOnCustomerLocation()
        {
            try
            {
                this.IsBusy = true;
                this.Model.Countries = await SSProxyHelper.Instance.GetCountryRegionListFromSvcAsync();
                if (this.Model.Countries != null)
                {
                    this.Model.SelectedCountry = this.Model.Countries.Where(w => this.CustomerDetails.Address.Contains(w.Name)).FirstOrDefault();

                    if (this.Model.SelectedCountry != null)
                    {
                        this.Model.Provinces = await SSProxyHelper.Instance.GetProvinceListFromSvcAsync(this.Model.SelectedCountry.Id);

                        this.Model.Selectedprovince = this.Model.Provinces.Where(w => this.CustomerDetails.Address.Contains(w.Name)).FirstOrDefault();

                        if (this.Model.Selectedprovince != null)
                        {
                            this.Model.Cities = await SSProxyHelper.Instance.GetCityListFromSvcAsync(this.Model.SelectedCountry.Id, this.Model.Selectedprovince.Id);

                            this.Model.SelectedCity = this.Model.Cities.Where(w => this.CustomerDetails.Address.Contains(w.Name)).FirstOrDefault();

                            this.Model.Regions = await SSProxyHelper.Instance.GetRegionListFromSvcAsync(this.Model.SelectedCountry.Id, this.Model.Selectedprovince.Id);

                            this.Model.SelectedRegion = this.Model.Regions.Where(w => this.CustomerDetails.Address.Contains(w.Name)).FirstOrDefault();
                        }

                    }
                }

                string countryId = this.Model.SelectedCountry != null ? this.Model.SelectedCountry.Id : string.Empty;
                string provinceId = this.Model.Selectedprovince != null ? this.Model.Selectedprovince.Id : string.Empty;
                string cityId = this.Model.SelectedCity != null ? this.Model.SelectedCity.Id : string.Empty;
                string suburbId = this.Model.SelectedSuburb != null ? this.Model.SelectedSuburb.Id : string.Empty;
                string regionId = this.Model.SelectedRegion != null ? this.Model.SelectedRegion.Id : string.Empty;

                this.Model.Suppliers = await SSProxyHelper.Instance.FilterSuppliersByCriteria(countryId, provinceId, cityId, suburbId, regionId);

                this.IsBusy = false;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                this.IsBusy = false;
            }
        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.IsBusy = true;
                this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this.DriverTask = PersistentData.Instance.DriverTask;
                this.CustomerDetails = PersistentData.Instance.CustomerDetails;
                var countries = await SSProxyHelper.Instance.GetCountryRegionListFromSvcAsync();
                if (countries != null)
                {
                    this.Model.Countries.AddRange(countries);
                }

                //var geolocator = new Geolocator();
                //Geoposition position = await geolocator.GetGeopositionAsync();
                //if (position.CivicAddress != null)
                //{
                //    this.Model.Suppliers = await SSProxyHelper.Instance.FilterSuppliersByCriteria(position.CivicAddress.Country, position.CivicAddress.State, position.CivicAddress.City, string.Empty, string.Empty);
                //}

                // await DefaultSuplierOnCustomerLocation();
                this.Model.Suppliers = await SSProxyHelper.Instance.GetSuppliersByClass(DriverTask.VehicleClassId);
                _navigationService.ClearHistory();

                this.IsBusy = false;
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
        private CustomerDetails customerDetails;
        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }
        private DriverTask driverTask;
        public DriverTask DriverTask
        {
            get { return driverTask; }
            set { SetProperty(ref driverTask, value); }
        }
        public DelegateCommand GoToConfirmationCommand { get; set; }
        public DelegateCommand<object> CountryChangedCommand { get; set; }
        public DelegateCommand<object> ProvinceChangedCommand { get; set; }
        public DelegateCommand<object> CityChangedCommand { get; set; }
        public DelegateCommand<object> SuburbChangedCommand { get; set; }

        public DelegateCommand<object> RegionChangedCommand { get; set; }
        public DelegateCommand<string> SubmitQueryCommand { get; set; }
        public DelegateCommand SupplierFilterCommand { get; set; }

        private SupplierSelection model;
        [RestorableState]
        public SupplierSelection Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private Visibility loadingCriteriaProgressVisibility;

        public Visibility LoadingCriteriaProgressVisibility
        {
            get { return loadingCriteriaProgressVisibility; }
            set { SetProperty(ref loadingCriteriaProgressVisibility, value); }
        }


        private Supplier selectedSupplier;
        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                if (SetProperty(ref selectedSupplier, value))
                {
                    this.GoToConfirmationCommand.RaiseCanExecuteChanged();
                    this.Model.SelectedSupplier = this.SelectedSupplier;
                }
            }
        }
    }
}
