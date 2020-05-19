using Pithline.FMS.BusinessLogic;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Portable.SSModels
{
    public class ServiceSchedulingDetail : BindableBase
    {
        public ServiceSchedulingDetail()
        {
            this.OdoReadingImageCapture = new ImageCapture { ImagePath = "ms-appx:///Assets/ODO_meter.png", ImageBitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/ODO_meter.png")) };
            this.ODOReadingDt = DateTimeOffset.Now;
            this.ServiceDateOpt1 = DateTimeOffset.Now;
            this.serviceDateOpt2 = DateTimeOffset.Now;
        }

        private string caseNumber;

        public string CaseNumber
        {
            get { return caseNumber; }
            set { SetProperty(ref caseNumber, value); }
        }

        private string caseServiceRecID;
        public string CaseServiceRecID
        {
            get { return caseServiceRecID; }
            set { SetProperty(ref caseServiceRecID, value); }
        }

        private string oDOReadingSnapshot;
        public string ODOReadingSnapshot
        {
            get { return oDOReadingSnapshot; }
            set { SetProperty(ref oDOReadingSnapshot, value); }
        }


        private ImageCapture odoReadingImageCapture;
        public ImageCapture OdoReadingImageCapture
        {
            get { return odoReadingImageCapture; }
            set { SetProperty(ref odoReadingImageCapture, value); }
        }

        private string oDOReading;
        public string ODOReading
        {
            get { return oDOReading; }
            set { SetProperty(ref oDOReading, value); }
        }

        private string oDOReadingDate;
        public string ODOReadingDate
        {
            get { return oDOReadingDate; }
            set { SetProperty(ref oDOReadingDate, value); }
        }

        private DateTimeOffset oDOReadingDt;
        public DateTimeOffset ODOReadingDt
        {
            get { return oDOReadingDt; }
            set { SetProperty(ref oDOReadingDt, value); }
        }

        private List<string> serviceType;

        public List<string> ServiceType
        {
            get { return serviceType; }
            set { SetProperty(ref serviceType, value); }
        }

        private string contactPersonName;

        public string ContactPersonName
        {
            get { return contactPersonName; }
            set { SetProperty(ref contactPersonName, value); }
        }


        private string contactPersonPhone;

        public string ContactPersonPhone
        {
            get { return contactPersonPhone; }
            set { SetProperty(ref contactPersonPhone, value); }
        }

        private string supplierName;

        public string SupplierName
        {
            get { return supplierName; }
            set { SetProperty(ref supplierName, value); }
        }

        private string eventDesc;

        public string EventDesc
        {
            get { return eventDesc; }
            set { SetProperty(ref eventDesc, value); }
        }


        private List<DestinationType> dstinationTypes;

        public List<DestinationType> DestinationTypes
        {
            get { return dstinationTypes; }
            set { SetProperty(ref dstinationTypes, value); }
        }


        public List<LocationType> LocationTypes { get; set; }

        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private string additionalWork;

        public string AdditionalWork
        {
            get { return additionalWork; }
            set { SetProperty(ref additionalWork, value); }
        }


        private string serviceDateOption1;

        public string ServiceDateOption1
        {
            get { return serviceDateOption1; }
            set { SetProperty(ref serviceDateOption1, value); }
        }

        private string serviceDateOption2;

        public string ServiceDateOption2
        {
            get { return serviceDateOption2; }
            set { SetProperty(ref serviceDateOption2, value); }
        }


        private string supplierDateTime;
        public string SupplierDateTime
        {
            get { return supplierDateTime; }
            set { SetProperty(ref supplierDateTime, value); }
        }

        private Boolean isLiftRequired;

        public Boolean IsLiftRequired
        {
            get { return isLiftRequired; }
            set { SetProperty(ref isLiftRequired, value); }
        }


        private LocationType selectedLocType;

        public LocationType SelectedLocType
        {
            get { return selectedLocType; }
            set { SetProperty(ref selectedLocType, value); }
        }

        private LocationType selectedLocationType;
        public LocationType SelectedLocationType
        {
            get { return selectedLocationType; }
            set { 
                
                SetProperty(ref selectedLocationType, value);
             

            }
        }


        private DestinationType slectedDestinationType;

        public DestinationType SelectedDestinationType
        {
            get { return slectedDestinationType; }
            set { SetProperty(ref slectedDestinationType, value); }
        }


        private string selectedServiceType;

        public string SelectedServiceType
        {
            get { return selectedServiceType; }
            set { SetProperty(ref selectedServiceType, value); }
        }


        private string confirmedDate;
        public string ConfirmedDate
        {
            get { return confirmedDate; }
            set { SetProperty(ref confirmedDate, value); }
        }
    
        private DateTimeOffset serviceDateOpt1;
        public DateTimeOffset ServiceDateOpt1
        {
            get { return serviceDateOpt1; }
            set { SetProperty(ref serviceDateOpt1, value); }
        }


        private DateTimeOffset serviceDateOpt2;
        public DateTimeOffset ServiceDateOpt2
        {
            get { return serviceDateOpt2; }
            set { SetProperty(ref serviceDateOpt2, value); }
        }
        
    }
    public class ServiceSchedulingDetailEvent : PubSubEvent<ServiceSchedulingDetail>
    {

    }


}

