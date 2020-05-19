using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Enums;
using Pithline.FMS.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Pithline.FMS.BusinessLogic.DocumentDelivery
{
    public class CollectDeliveryDetail : ValidatableBindableBase
    {
        private string caseNumber;
        [PrimaryKey]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { SetProperty(ref caseNumber, value); }
        }

        private long caseServiceRecId;
        public long CaseServiceRecId
        {
            get { return caseServiceRecId; }
            set { SetProperty(ref caseServiceRecId, value); }
        }

        private string selectedCollectedFrom;
        public string SelectedCollectedFrom
        {
            get { return selectedCollectedFrom; }
            set { SetProperty(ref selectedCollectedFrom, value); }
        }

        private string deliveryPersonName;
        public string DeliveryPersonName
        {
            get { return deliveryPersonName; }
            set { SetProperty(ref deliveryPersonName, value); }
        }


        private string crSignFileName;

        public string CRSignFileName
        {
            get { return crSignFileName; }
            set { SetProperty(ref crSignFileName, value); }
        }

        private string deliveredAt;
        public string DeliveredAt
        {
            get { return deliveredAt; }
            set { SetProperty(ref deliveredAt, value); }
        }

        private string collectedAt;

        public string CollectedAt
        {
            get { return collectedAt; }
            set { SetProperty(ref collectedAt, value); }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
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

        private string receivedBy;

        public string ReceivedBy
        {
            get { return receivedBy; }
            set { SetProperty(ref receivedBy, value); }
        }

        private string referenceNo;

        public string ReferenceNo
        {
            get { return referenceNo; }
            set { SetProperty(ref referenceNo, value); }
        }

        private DateTime receivedDate;

        public DateTime ReceivedDate
        {
            get { return receivedDate; }
            set { SetProperty(ref receivedDate, value); }
        }

        private string phone;

        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }
        private DateTime deliveryDate;

        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set { SetProperty(ref deliveryDate, value); }
        }

        private bool isColletedByCustomer;

        public bool IsColletedByCustomer
        {
            get { return isColletedByCustomer; }
            set { SetProperty(ref isColletedByCustomer, value); }
        }

    }
}
