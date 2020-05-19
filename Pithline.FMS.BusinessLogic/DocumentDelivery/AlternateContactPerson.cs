using Pithline.FMS.BusinessLogic.Base;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.DocumentDelivery
{
    public class AlternateContactPerson : ValidatableBindableBase
    {
        private string userId;
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { SetProperty(ref firstName, value); }
        }
        private string surname;
        public string Surname
        {
            get { return surname; }
            set { SetProperty(ref surname, value); }
        }


        private string fullName;
        [Ignore]
        public string FullName
        {
            get { return (this.FirstName + String.Empty.PadLeft(2)+ this.Surname); }
            set { SetProperty(ref fullName, value); }
        }

        private string cellPhone;

        public string CellPhone
        {
            get { return cellPhone; }
            set { SetProperty(ref cellPhone, value); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        private string position;

        public string Position
        {
            get { return position; }


            set { SetProperty(ref position, value); }
        }

    }

    public class AlternateContactPersonEvent : PubSubEvent<AlternateContactPerson>
    {
    }
}
