using Pithline.FMS.BusinessLogic.Enums;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
namespace Pithline.FMS.BusinessLogic
{
     
    public class CustomerDetails : ValidatableBindableBase
    {
        private string customerName;
        [RestorableState]
        public string CustomerName
        {
            get { return customerName; }
            set { SetProperty(ref customerName, value); }
        }

        private string contactName;
        [RestorableState]
        public string ContactName
        {
            get { return contactName; }
            set { SetProperty(ref contactName, value); }
        }

        private long vehicleInsRecId;
        [PrimaryKey]
        public long VehicleInsRecId
        {
            get { return vehicleInsRecId; }
            set { SetProperty(ref vehicleInsRecId, value); }

        }

        private string contactNumber;
        [RestorableState]
        public string ContactNumber
        {
            get { return contactNumber; }
            set { SetProperty(ref contactNumber, value); }
        }

        private string address;
        [RestorableState]
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private string emailId;
        [RestorableState]
        public string EmailId
        {
            get { return emailId; }
            set { SetProperty(ref emailId, value); }
        }

        private ScheduleAppointmentCollection appointments;
        [Ignore]

        public ScheduleAppointmentCollection Appointments
        {
            get { return appointments; }
            set { SetProperty(ref appointments, value); }
        }

        private string caseNumber;
        [RestorableState]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { SetProperty(ref caseNumber, value); }
        }

        private CaseTypeEnum caseType;

        public CaseTypeEnum CaseType
        {
            get { return caseType; }
            set { SetProperty(ref caseType, value); }
        }
        
        private DateTime statusDueDate;
        [RestorableState]
        public DateTime StatusDueDate
        {
            get { return statusDueDate; }
            set { SetProperty(ref statusDueDate, value); }
        }

        private string status;
        [RestorableState]
        public string Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

        private string allocatedTo;
        [RestorableState]
        public string AllocatedTo
        {
            get { return allocatedTo; }
            set { SetProperty(ref allocatedTo, value); }
        }

        private string categoryType;
        [RestorableState]
        public string CategoryType
        {
            get { return categoryType; }
            set { SetProperty(ref categoryType, value); }
        }


    }
}
