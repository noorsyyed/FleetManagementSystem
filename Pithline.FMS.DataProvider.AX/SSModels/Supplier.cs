using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.DataProvider.AX.SSModels
{
    public class Supplier
    {
        public String AccountNum { get; set; }

        public String SupplierName { get; set; }

        public String SupplierContactName { get; set; }

        public String SupplierContactNumber { get; set; }

        public DateTime SupplierDate { get; set; }

        public DateTime SupplierTime { get; set; }

        public String Country { get; set; }

        public String Province { get; set; }

        public String City { get; set; }

        public String Suburb { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }


        public String CountryName { get; set; }

        public String ProvinceName { get; set; }

        public String CityName { get; set; }

        public String SuburbName { get; set; }


    }
}
