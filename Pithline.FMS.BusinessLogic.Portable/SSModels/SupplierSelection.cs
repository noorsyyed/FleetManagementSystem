using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Portable.SSModels
{
    public class SupplierSelection
    {
        public String CaseNumber { get; set; }

        public Int64 CaseServiceRecID { get; set; }

        public List<Country> Countries { get; set; }

        public List<Province> Provinces { get; set; }

        public List<City> Cities { get; set; }

        public List<Suburb> Suburbs { get; set; }

        public List<Region> Regions { get; set; }

        public List<Supplier> Suppliers { get; set; }

        public Supplier SelectedSupplier { get; set; }

        public string SelectedCountry { get; set; }

        public string Selectedprovince { get; set; }

        public string SelectedCity { get; set; }

        public string SelectedSuburb { get; set; }

        public string SelectedRegion { get; set; }

    }
}
