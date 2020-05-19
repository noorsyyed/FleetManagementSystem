using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic
{
    public class VehicleDetails : ValidatableBindableBase
    {
        private string registrationNumber;

        [PrimaryKey]
        [RestorableState]
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
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

    }
}
