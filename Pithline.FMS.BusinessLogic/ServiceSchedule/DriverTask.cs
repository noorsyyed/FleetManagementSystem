using Pithline.FMS.BusinessLogic.Enums;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.ServiceSchedule
{
    public class DriverTask : ValidatableBindableBase
    {
        public DriverTask()
        {
            this.CaseType = new CaseTypeEnum();
        }
        private DateTime modelYear;
        public DateTime ModelYear
        {
            get { return modelYear; }
            set { SetProperty(ref modelYear, value); }
        }

        private string caseNumber;

        [RestorableState]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { caseNumber = value; }

        }

        private string cusEmailId;

        public string CusEmailId
        {
            get { return cusEmailId; }
            set { SetProperty(ref cusEmailId, value); }
        }
        
        private string caseCategory;

        public string CaseCategory
        {
            get { return caseCategory; }
            set { SetProperty(ref caseCategory, value); }
        }

        private string userId;

        public string UserId
        {
            get { return userId; }
            set { SetProperty(ref userId, value); }
        }
        
        private string address;
        [RestorableState]
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
        private DateTime confirmedTime;

        public DateTime ConfirmedTime
        {
            get { return confirmedTime; }
            set { SetProperty(ref confirmedTime, value); }
        }

        private CaseTypeEnum caseType;
        public CaseTypeEnum CaseType
        {
            get { return caseType; }
            set { SetProperty(ref caseType, value); }
        }

        private DateTime confirmedDate;

        public DateTime ConfirmedDate
        {
            get { return confirmedDate; }
            set { SetProperty(ref confirmedDate, value); }
        }
        private long vehicleInsRecId;
        [PrimaryKey]
        public long VehicleInsRecId
        {
            get { return vehicleInsRecId; }
            set { SetProperty(ref vehicleInsRecId, value); }

        }

        private DateTime statusDueDate;

        public DateTime StatusDueDate
        {
            get { return statusDueDate; }
            set { SetProperty(ref statusDueDate, value); }
        }

        private string status;

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

        private string confirmationDate;

        public string ConfirmationDate
        {
            get { return confirmationDate; }
            set { SetProperty(ref confirmationDate, value); }
        }

        private DateTime scheduledTime;

        public DateTime ScheduledTime
        {
            get { return scheduledTime; }
            set { SetProperty(ref scheduledTime, value); }
        }

        private string customerId;
        [RestorableState]
        public string CustomerId
        {
            get { return customerId; }
            set { SetProperty(ref customerId, value); }
        }

        private string contactName;
        [RestorableState]
        public string ContactName
        {
            get { return contactName; }
            set { SetProperty(ref contactName, value); }
        }
        

        private string registrationNumber;
        [RestorableState]
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }

        private string customerName;
        [RestorableState]
        public string CustomerName
        {
            get { return customerName; }
            set { SetProperty(ref customerName, value); }
        }

        private long caseServiceRecID;

        public long CaseServiceRecID
        {
            get { return caseServiceRecID; }
            set { SetProperty(ref caseServiceRecID, value); }
        }
        private string description;
        [RestorableState]
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private string make;
        [RestorableState]
        public string Make
        {
            get { return make; }
            set { SetProperty(ref make, value); }
        }

        private long collectionRecID;
        public long CollectionRecID
        {
            get { return collectionRecID; }
            set { SetProperty(ref collectionRecID, value); }
        }

        private string custPhone;

        public string CustPhone
        {
            get { return custPhone; }
            set { SetProperty(ref custPhone, value); }
        }

        private long serviceRecID;

        public long ServiceRecID
        {
            get { return serviceRecID; }
            set { SetProperty(ref serviceRecID, value); }
        }


        private string driverFirstName;

        public string DriverFirstName
        {
            get { return driverFirstName; }
            set { SetProperty(ref driverFirstName, value); }
        }

        private string driverLastName;

        public string DriverLastName
        {
            get { return driverLastName; }
            set { SetProperty(ref driverLastName, value); }
        }

        private string driverPhone;

        public string DriverPhone
        {
            get { return driverPhone; }
            set { SetProperty(ref driverPhone, value); }
        }

        private string model;

        public string Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private string vehicleClassId;

        public string VehicleClassId
        {
            get { return vehicleClassId; }
            set { SetProperty(ref vehicleClassId , value); }
        }

        private string vehicleSubClassId;

        public string VehicleSubClassId
        {
            get { return vehicleSubClassId; }
            set { SetProperty(ref vehicleSubClassId , value); }
        }

        
    }
    public class DriverTaskFetchedEvent : PubSubEvent<DriverTask>
    {

    }
}
