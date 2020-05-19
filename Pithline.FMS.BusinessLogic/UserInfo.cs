using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic
{
    public class UserInfo : ValidatableBindableBase
    {
        private string userId;
        [RestorableState]
        public string UserId
        {
            get { return userId; }
            set { SetProperty(ref userId, value); }
        }

        private string name;
        [RestorableState]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string emailId;
        [RestorableState]
        public string EmailId
        {
            get { return emailId; }
            set { SetProperty(ref emailId, value); }
        }

        private string companyId;

        [RestorableState]
        public string CompanyId
        {
            get { return companyId; }
            set { SetProperty(ref companyId, value); }
        }

        private string companyName;

        [RestorableState]
        public string CompanyName
        {
            get { return companyName; }
            set { SetProperty(ref companyName, value); }
        }

    }
}
