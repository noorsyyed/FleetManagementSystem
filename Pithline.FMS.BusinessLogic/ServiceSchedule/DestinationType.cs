using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.ServiceSchedule
{
    public class DestinationType : ValidatableBindableBase
    {
        private string contactName;
        public string ContactName
        {
            get { return contactName; }
            set { SetProperty(ref contactName, value); }
        }
        private string id;
        public string Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private long recID;
        public long RecID
        {
            get { return recID; }
            set { SetProperty(ref recID, value); }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

    }
}
