using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.DataProvider.AX.SSModels
{
    public class Address
    {

        public String Street { get; set; }

        public List<string> Postcodes { get; set; }

        public List<Country> Countries { get; set; }

        public List<Province> Provinces { get; set; }

        public List<City> Cities { get; set; }

        public List<Suburb> Suburbs { get; set; }

        public List<Region> Region { get; set; }

        public string SelectedCountry { get; set; }

        public string Selectedprovince { get; set; }

        public string SelectedCity { get; set; }

        public string SelectedSuburb { get; set; }

        public string SelectedZip { get; set; }

    }
}
