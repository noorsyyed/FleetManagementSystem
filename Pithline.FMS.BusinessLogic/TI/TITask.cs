using Pithline.FMS.BusinessLogic.Enums;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.Prism.PubSubEvents;
namespace Pithline.FMS.BusinessLogic
{
    public class TITask : ValidatableBindableBase
    {

        private string caseNumber;

        [RestorableState]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { caseNumber = value; }

        }

        private long vehicleInsRecId;
        [PrimaryKey]
        public long VehicleInsRecId
        {
            get { return vehicleInsRecId; }
            set { SetProperty(ref vehicleInsRecId, value); }

        }

        private CaseTypeEnum caseType;
        public CaseTypeEnum CaseType
        {
            get { return caseType; }
            set { SetProperty(ref caseType, value); }
        }

        private string caseCategory;
        [RestorableState]
        public string CaseCategory
        {
            get { return caseCategory; }
            set { SetProperty(ref caseCategory, value); }
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

        private DateTime confirmedDate;

        public DateTime ConfirmedDate
        {
            get { return confirmedDate; }
            set { SetProperty(ref confirmedDate, value); }
        }

        private string customerId;
        [RestorableState]
        public string CustomerId
        {
            get { return customerId; }
            set { SetProperty(ref customerId, value); }
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


        private long caseServiceRecID;

        public long CaseServiceRecID
        {
            get { return caseServiceRecID; }
            set { SetProperty(ref caseServiceRecID, value); }
        }

        private string categoryType;
        public string CategoryType
        {
            get { return categoryType; }
            set { SetProperty(ref categoryType, value); }
        }

        private long collectionRecID;
        public long CollectionRecID
        {
            get { return collectionRecID; }
            set { SetProperty(ref collectionRecID, value); }
        }

        private long processStepRecID;

        public long ProcessStepRecID
        {
            get { return processStepRecID; }
            set { SetProperty(ref processStepRecID, value); }
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

        private string processStep;

        public string ProcessStep
        {
            get { return processStep; }
            set { SetProperty(ref  processStep, value); }
        }

        private string userId;

        public string UserId
        {
            get { return userId; }
            set { SetProperty(ref userId, value); }
        }

        private VehicleTypeEnum vehicleType;
        public VehicleTypeEnum VehicleType
        {
            get { return vehicleType; }
            set { SetProperty(ref vehicleType, value); }
        }

        private string contactName;
        [RestorableState]
        public string ContactName
        {
            get { return contactName; }
            set { SetProperty(ref contactName, value); }
        }

        private string contactNumber;
        [RestorableState]
        public string ContactNumber
        {
            get { return contactNumber; }
            set { SetProperty(ref contactNumber, value); }
        }

        private bool allowEditing;

        public bool AllowEditing
        {
            get { return allowEditing; }
            set { SetProperty(ref allowEditing, value); }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }


    }

    public class TITaskFetchedEvent : PubSubEvent<TITask>
    {

    }
}
