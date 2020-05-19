using System;
using System.Collections.Generic;
namespace Pithline.FMS.BusinessLogic.Portable.TIModels
{
    public class CustomerDetails
    {
        public String Id { get; set; }

        public String CustomerName { get; set; }
        public String ContactName { get; set; }
        public Int64 VehicleInsRecId { get; set; }
        public String ContactNumber { get; set; }
        public String Address { get; set; }
        public String EmailId { get; set; }
        public List<string> Appointments { get; set; }
        public String CaseNumber { get; set; }
        public string CaseType { get; set; }
        public DateTime StatusDueDate { get; set; }
        public String Status { get; set; }
        public String AllocatedTo { get; set; }
        public String CategoryType { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }
}

