using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Portable.SSModels
{
    public class Task
    {

        public String CaseNumber { get; set; }
    
        public String CusEmailId { get; set; }

        public String CaseCategory { get; set; }

        public String UserId { get; set; }

        public String Address { get; set; }

        public DateTime ConfirmedTime { get; set; }

        public String CaseType { get; set; }
        public String ContactName { get; set; }

        public string ConfirmedDate { get; set; }
        public Int64 VehicleInsRecId { get; set; }

        public string StatusDueDate { get; set; }

        public String Status { get; set; }

        public String AllocatedTo { get; set; }

        public String CustomerId { get; set; }

        public String RegistrationNumber { get; set; }

        public String CustomerName { get; set; }

        public Int64 CaseServiceRecID { get; set; }

        public String Description { get; set; }

        public String Make { get; set; }

        public Int64 CollectionRecID { get; set; }

        public String CustPhone { get; set; }

        public Int64 ServiceRecID { get; set; }

        public String DriverFirstName { get; set; }

        public String DriverLastName { get; set; }

        public String DriverPhone { get; set; }

        public String Model { get; set; }

        public DateTime AppointmentStart { get; set; }
        public DateTime AppointmentEnd { get; set; }

        public string VehicleClassId { get; set; }

        public string VehicleSubClassId { get; set; }
       
    }
}
