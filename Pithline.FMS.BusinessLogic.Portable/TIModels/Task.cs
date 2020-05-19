using System;
namespace Pithline.FMS.BusinessLogic.Portable.TIModels
{
    public class Task
    {

        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecId { get; set; }
        public string CaseType { get; set; }
        public String CaseCategory { get; set; }
        public string StatusDueDate { get; set; }
        public String Status { get; set; }
        public String AllocatedTo { get; set; }
        public DateTime ConfirmedDate { get; set; }
        public string ConfirmedStDate { get; set; }
        public String CustomerId { get; set; }
        public String RegistrationNumber { get; set; }
        public String CustomerName { get; set; }
        public String Address { get; set; }
        public DateTime ConfirmedTime { get; set; }
        public Int64 CaseServiceRecID { get; set; }
        public String CategoryType { get; set; }
        public Int64 CollectionRecID { get; set; }
        public Int64 ProcessStepRecID { get; set; }
        public String CustPhone { get; set; }
        public Int64 ServiceRecID { get; set; }
        public String ProcessStep { get; set; }
        public String UserId { get; set; }
        public string VehicleType { get; set; }
        public String ContactName { get; set; }
        public String ContactNumber { get; set; }
        public Boolean AllowEditing { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

