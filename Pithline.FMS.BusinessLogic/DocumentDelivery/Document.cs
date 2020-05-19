using Pithline.FMS.BusinessLogic.Base;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.DocumentDelivery
{
    public class Document : ValidatableBindableBase
    {

        private long caseCategoryRecID;
        [PrimaryKey]
        public long CaseCategoryRecID
        {
            get { return caseCategoryRecID; }
            set { SetProperty(ref caseCategoryRecID, value); }
        }

        private bool isMarked;
        [Ignore]
        public bool IsMarked
        {
            get { return isMarked; }
            set { SetProperty(ref isMarked, value); }
        }

        private string caseNumber;
        [Ignore]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { SetProperty(ref caseNumber, value); }
        }

        private string documentType;

        public string DocumentType
        {
            get { return documentType; }
            set { SetProperty(ref documentType, value); }
        }

        private string serialNumber;

        public string SerialNumber
        {
            get { return serialNumber; }
            set { SetProperty(ref serialNumber, value); }
        }

        private string makeModel;
        public string MakeModel
        {
            get { return makeModel; }
            set { SetProperty(ref makeModel, value); }
        }

        private string registrationNumber;
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }

        private string docName;

        public string DocumentName
        {
            get { return docName; }
            set { SetProperty(ref docName, value); }
        }

        private DateTime actionDate;

        public DateTime ActionDate
        {
            get { return actionDate; }
            set { SetProperty(ref actionDate, value); }
        }

        private DateTime actionTime;

        public DateTime ActionTime
        {
            get { return actionTime; }
            set { SetProperty(ref actionTime, value); }
        }

        private DateTime statusDueDate;

        public DateTime StatusDueDate
        {
            get { return statusDueDate; }
            set { SetProperty(ref statusDueDate, value); }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }
        
    }
}
