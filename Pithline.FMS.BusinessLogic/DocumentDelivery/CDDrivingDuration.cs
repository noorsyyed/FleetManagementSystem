using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.DocumentDelivery
{
    public class CDDrivingDuration : ValidatableBindableBase
    {
        private string caseNumber;
        [PrimaryKey]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { SetProperty(ref caseNumber, value); }
        }

        private long caseCategoryRecID;
       
        public long CaseCategoryRecID
        {
            get { return caseCategoryRecID; }
            set { SetProperty(ref caseCategoryRecID, value); }
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
