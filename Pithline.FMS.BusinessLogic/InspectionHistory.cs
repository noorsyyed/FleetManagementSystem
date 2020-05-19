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

    public class InspectionHistory : ValidatableBindableBase
    {
        private string id;
        [PrimaryKey, AutoIncrement, Indexed]
        [RestorableState]
        public string Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string customerId;
        [RestorableState]
        public string CustomerId
        {
            get { return customerId; }
            set { SetProperty(ref customerId, value); }
        }

        private DateTime inspectedOn;
        public DateTime InspectedOn
        {
            get { return inspectedOn; }
            set { SetProperty(ref inspectedOn, value); }
        }

        private string inspectedBy;
        [RestorableState]
        public string InspectedBy
        {
            get { return inspectedBy; }
            set { SetProperty(ref inspectedBy, value); }
        }

        private string registrationNumber;
        [RestorableState]
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { registrationNumber = value; }
        }

        private List<string> inspectionResult;
        public List<string> InspectionResult
        {
            get { return inspectionResult; }
            set { SetProperty(ref inspectionResult, value); }
        }

    }
}
