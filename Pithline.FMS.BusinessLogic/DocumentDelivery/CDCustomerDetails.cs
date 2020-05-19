using Pithline.FMS.BusinessLogic.Enums;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.DocumentDelivery
{
    public class CDCustomerDetails : ValidatableBindableBase
    {
        private string caseNumber;
        [PrimaryKey]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { SetProperty(ref caseNumber, value); }
        }
        private long caseCategoryRecID;

        public long CaseCategoryRecID
        {
            get { return caseCategoryRecID; }
            set { SetProperty(ref caseCategoryRecID, value); }

        }

        private CaseTypeEnum caseType;

        public CaseTypeEnum CaseType
        {
            get { return caseType; }
            set { SetProperty(ref caseType, value); }
        }

        private string customerName;
        public string CustomerName
        {
            get { return customerName; }
            set { SetProperty(ref customerName, value); }
        }
        private string contactName;

        public string ContactName
        {
            get { return contactName; }
            set { SetProperty(ref contactName, value); }
        }

        private string customerNumber;
        public string CustomerNumber
        {
            get { return customerNumber; }
            set { SetProperty(ref customerNumber, value); }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private string registrationNumber;
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }

        private string makeModel;
        public string MakeModel
        {
            get { return makeModel; }
            set { SetProperty(ref makeModel, value); }
        }


        private DateTime deliveryDate;
        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set { SetProperty(ref deliveryDate, value); }
        }

        private DateTime deliveryTime;
        public DateTime DeliveryTime
        {
            get { return deliveryTime; }
            set { SetProperty(ref deliveryTime, value); }
        }

        private string emailId;
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

        private string id;

        public string Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private int collectCount;
        [Ignore]
        public int CollectCount
        {
            get { return collectCount; }
            set { SetProperty(ref collectCount, value); }
        }

        private int deliverCount;
        [Ignore]
        public int DeliverCount
        {
            get { return deliverCount; }
            set { SetProperty(ref deliverCount, value); }
        }

    }

    public class CustomerDetailsEvent : PubSubEvent<CDCustomerDetails>
    {

    }
}
