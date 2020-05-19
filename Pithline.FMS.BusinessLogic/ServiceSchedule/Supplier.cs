using Pithline.FMS.BusinessLogic.Common;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.ServiceSchedule
{
    public class Supplier : ValidatableBindableBase
    {
        public Supplier()
        {
            this.SupplierDate = DateTime.Now;
            this.SupplierTime = DateTime.Now;
        }

        private string accountNum;

        public string AccountNum
        {
            get { return accountNum; }
            set { SetProperty(ref accountNum, value); }
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
            set
            {
                SetProperty(ref supplierDate, value);
            }
        }
        private DateTime supplierTime;
        public DateTime SupplierTime
        {
            get { return supplierTime; }
            set
            {
                SetProperty(ref supplierTime, value);
            }
        }
        private string country;
        public string Country
        {
            get { return country; }
            set { SetProperty(ref country, value); }
        }

        private string province;
        public string Province
        {
            get { return province; }
            set { SetProperty(ref province, value); }
        }

        private string city;
        public string City
        {
            get { return city; }
            set { SetProperty(ref city, value); }
        }

        private string suburb;
        public string Suburb
        {
            get { return suburb; }
            set { SetProperty(ref suburb, value); }
        }
    }
}
