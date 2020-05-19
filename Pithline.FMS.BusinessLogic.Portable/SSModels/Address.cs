using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Pithline.FMS.BusinessLogic.Portable.SSModels
{
    public class SupplierFilterEvent : PubSubEvent<ObservableCollection<Supplier>>
    {

    }

    public class AddressFilterEvent : PubSubEvent<Address>
    {

    }
    public class Address : BindableBase
    {

        public Address()
        {
            this.Countries = new ObservableCollection<Country>();
            this.Provinces = new ObservableCollection<Province>();
            this.Cities = new ObservableCollection<City>();
            this.Region = new ObservableCollection<Region>();
        }

        private Visibility progressVisibility;

        public Visibility ProgressVisibility
        {
            get { return progressVisibility; }
            set { SetProperty(ref progressVisibility, value); }
        }

        public ObservableCollection<string> Postcodes { get; set; }


        private ObservableCollection<Country> countries;
        public ObservableCollection<Country> Countries
        {
            get { return countries; }
            set { SetProperty(ref countries, value); }
        }


        private ObservableCollection<Province> provinces;
        public ObservableCollection<Province> Provinces
        {
            get { return provinces; }
            set { SetProperty(ref provinces, value); }
        }

        private ObservableCollection<City> cities;

        public ObservableCollection<City> Cities
        {
            get { return cities; }
            set { SetProperty(ref cities, value); }
        }

        public ObservableCollection<Suburb> Suburbs { get; set; }


        private ObservableCollection<Region> region;
        public ObservableCollection<Region> Region
        {
            get { return region; }
            set { SetProperty(ref region, value); }
        }

        private Country selectedCountry;

        public Country SelectedCountry
        {
            get { return selectedCountry; }
            set { SetProperty(ref selectedCountry, value); }
        }

        private Province selectedprovince;
        public Province Selectedprovince
        {
            get { return selectedprovince; }
            set { SetProperty(ref selectedprovince, value); }
        }

        private City selectedCity;

        public City SelectedCity
        {
            get { return selectedCity; }
            set { SetProperty(ref selectedCity, value); }
        }
        private Suburb selectedSuburb;
        public Suburb SelectedSuburb
        {
            get { return selectedSuburb; }
            set { SetProperty(ref selectedSuburb, value); }
        }
        private Region selectedRegion;

        public Region SelectedRegion
        {
            get { return selectedRegion; }
            set { SetProperty(ref selectedRegion, value); }
        }

        private string selectedZip;
        public string SelectedZip
        {
            get { return selectedZip; }
            set { SetProperty(ref selectedZip, value); }
        }


        private string street;
        public string Street
        {
            get { return street; }
            set { SetProperty(ref street, value); }
        }

    }
}
