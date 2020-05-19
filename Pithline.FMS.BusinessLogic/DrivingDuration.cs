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
    public class DrivingDuration : ValidatableBindableBase
    {

        private string caseNumber;
        [RestorableState]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { SetProperty(ref caseNumber, value); }
        }

        private long vehicleInsRecID;
        [PrimaryKey]
        public long VehicleInsRecID
        {
            get { return vehicleInsRecID; }
            set { SetProperty(ref vehicleInsRecID, value); }
        }

        private DateTime startDateTime;
   
        public DateTime StartDateTime
        {
            get { return startDateTime; }
            set { SetProperty(ref startDateTime, value); }
        }

        private DateTime stopDateTime;
  
        public DateTime StopDateTime
        {
            get { return stopDateTime; }
            set { SetProperty(ref stopDateTime, value); }
        }
    }
}
