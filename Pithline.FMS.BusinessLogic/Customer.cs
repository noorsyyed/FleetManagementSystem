using Pithline.FMS.BusinessLogic.Base;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic
{

    public class Customer : ValidatableBindableBase
    {
        private string id;
        [RestorableState]
        [PrimaryKey]
        public string Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

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

    }
}
