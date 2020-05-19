using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.ServiceSchedule
{
    public class SupplierSelection : ValidatableBindableBase
    {
        public SupplierSelection()
        {
            this.Suppliers = new List<Supplier>();
            this.Countries = new List<Country>();
            this.Provinces = new List<Province>();
            this.Cities = new List<City>();
            this.Suburbs = new List<Suburb>();
            this.SelectedCity = new City();
            this.SelectedCountry = new Country();
            this.Selectedprovince = new Province();
            this.SelectedSuburb = new Suburb();
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

        private List<Region> regions;

        public List<Region> Regions
        {
            get { return regions; }
            set { SetProperty(ref regions, value); }
        }

        private List<Supplier> suppliers;

        public List<Supplier> Suppliers
        {
            get { return suppliers; }
            set { SetProperty(ref suppliers, value); }
        }

        private Supplier selectedSupplier;
        [Required(ErrorMessage = "Please select a supplier")]
        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set { SetProperty(ref selectedSupplier, value); }
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
        
    }
}