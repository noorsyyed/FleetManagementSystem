using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Portable.SSModels
{
    public class ConfirmationServiceScheduling
    {
        public Decimal ODOReading { get; set; }

        public DateTime ODOReadingDate { get; set; }

        public List<string> ServiceType { get; set; }

        public String DeliveryOption { get; set; }

        public List<LocationType> LocationType { get; set; }

        public String Address { get; set; }

        public String AdditionalWork { get; set; }

        public String SupplierName { get; set; }

        public String SupplierContactName { get; set; }

        public String SupplierContactNumber { get; set; }

        public DateTime SupplierDate { get; set; }

        public DateTime SupplierTime { get; set; }

        public DateTime ServiceDateOption1 { get; set; }

        public DateTime ServiceDateOption2 { get; set; }

        public String EventDesc { get; set; }

        public String ContactPersonName { get; set; }

        public String ContactPersonPhone { get; set; }

    }
}
