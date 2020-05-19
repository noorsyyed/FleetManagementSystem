using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.ServiceSchedulingModel
{
    public class ConfirmationServiceScheduling : ValidatableBindableBase
    {
        private decimal odoReading;
        public decimal ODOReading
        {
            get { return odoReading; }
            set { SetProperty(ref odoReading, value); }
        }

        private DateTime odoReadingDate;

        public DateTime ODOReadingDate
        {
            get { return odoReadingDate; }
            set { SetProperty(ref odoReadingDate, value); }
        }

        private string serviceType;

        public string ServiceType
        {
            get { return serviceType; }
            set { SetProperty(ref serviceType, value); }
        }

        private string deliveryOption;

        public string DeliveryOption
        {
            get { return deliveryOption; }
            set { SetProperty(ref deliveryOption, value); }
        }

        private string locationType;

        public string LocationType
        {
            get { return locationType; }
            set { SetProperty(ref locationType, value); }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private string additionalWork;

        public string AdditionalWork
        {
            get { return additionalWork; }
            set { SetProperty(ref additionalWork, value); }
        }

        private string supplierName;

        public string SupplierName
        {
            get { return supplierName; }
            set { SetProperty(ref supplierName, value); }
        }

        private string supplierContactName;

        public string SupplierContactName
        {
            get { return supplierContactName; }
            set { SetProperty(ref supplierContactName, value); }
        }

        private string supplierContactNumber;

        public string SupplierContactNumber
        {
            get { return supplierContactNumber; }
            set { SetProperty(ref  supplierContactNumber, value); }
        }

        private DateTime supplierDate;

        public DateTime SupplierDate
        {
            get { return supplierDate; }
            set { SetProperty(ref supplierDate, value); }
        }

        private DateTime supplierTime;

        public DateTime SupplierTime
        {
            get { return supplierTime; }
            set { SetProperty(ref supplierTime, value); }
        }
        private DateTime serviceDateOption1;

        public DateTime ServiceDateOption1
        {
            get { return serviceDateOption1; }
            set { SetProperty(ref serviceDateOption1, value); }
        }

        private DateTime serviceDateOption2;

        public DateTime ServiceDateOption2
        {
            get { return serviceDateOption2; }
            set { SetProperty(ref serviceDateOption2, value); }
        }

        private string eventDesc;

        public string EventDesc
        {
            get { return eventDesc; }
            set { SetProperty(ref eventDesc, value); }
        }

        private string contactPersonName;

        public string ContactPersonName
        {
            get { return contactPersonName; }
            set { SetProperty(ref contactPersonName, value); }
        }

        private string contactPersonPhone;

        public string ContactPersonPhone
        {
            get { return contactPersonPhone; }
            set { SetProperty(ref contactPersonPhone, value); }
        }
        
    }
}
