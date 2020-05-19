using Pithline.FMS.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.ServiceSchedule
{
    public class Address : ValidatableBindableBase
    {
        public Address()
        {
            this.Countries = new List<Country>();
            this.Provinces = new List<Province>();
            this.Cities = new List<City>();
            this.Suburbs = new List<Suburb>();
            this.Regions = new List<Region>();
        }

        private long entityRecId;

        public long EntityRecId
        {
            get { return entityRecId; }
            set { SetProperty(ref entityRecId, value); }
        }

        private string street;
        public string Street
        {
            get { return street; }
            set { SetProperty(ref street, value); }
        }
        private List<string> postcodes;
        public List<string> Postcodes
        {
            get { return postcodes; }
            set { SetProperty(ref postcodes, value); }
        }

        private List<Country> countries;
        public List<Country> Countries
        {
            get { return countries; }
            set { SetProperty(ref countries, value); }
        }

        private List<Province> provinces;

        public List<Province> Provinces
        {
            get { return provinces; }
            set { SetProperty(ref provinces, value); }
        }
        private List<City> cities;

        public List<City> Cities
        {
            get { return cities; }
            set { SetProperty(ref cities, value); }
        }

        private List<Suburb> suburbs;

        public List<Suburb> Suburbs
        {
            get { return suburbs; }
            set { SetProperty(ref suburbs, value); }
        }

        private List<Region> region;

        public List<Region> Regions
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

        public Province SelectedProvince
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

        private string selectedZip;
        public string SelectedZip
        {
            get { return selectedZip; }
            set { SetProperty(ref selectedZip, value); }
        }

        private Region selectedRegion;

        public Region SelectedRegion
        {
            get { return selectedRegion; }
            set { SetProperty(ref selectedRegion, value); }
        }


    }
    public class AddressEvent : PubSubEvent<Address>
    {
    }
}
