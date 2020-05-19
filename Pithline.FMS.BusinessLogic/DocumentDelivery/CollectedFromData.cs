using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.DocumentDelivery
{
    public class CollectedFromData : ValidatableBindableBase
    {
        private string userID;
        [PrimaryKey]
        public string UserID
        {
            get { return userID; }
            set { SetProperty(ref userID, value); }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

    }
}
