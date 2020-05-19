using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Common;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.ServiceSchedule
{
    public class ServiceSchedulingDetail : ValidatableBindableBase
    {
        public ServiceSchedulingDetail()
        {
            this.ODOReadingSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/ODO_meter.png" };
            this.DestinationTypes = new ObservableCollection<DestinationType>();
            this.LocationTypes = new List<LocationType>();
            this.IsValidationEnabled = false;
        }
        public new bool ValidateProperties()
        {
            bool isValid = base.ValidateProperties();
            List<string> hiddenFields = new List<string>();
            hiddenFields.Add("SelectedLocationType");
            hiddenFields.Add("SelectedDestinationType");
            hiddenFields.Add("Address");

            if (!this.IsLiftRequired && !this.Errors.Errors.Keys.Except(hiddenFields).Any())
            {
                return true;
            }
           
            return isValid;
        }
        private ImageCapture odoReadingSnapshot;
        [RestorableState]
        public ImageCapture ODOReadingSnapshot
        {
            get { return odoReadingSnapshot; }
            set { SetProperty(ref odoReadingSnapshot, value); }
        }

        private string odoReading;
        [Required(ErrorMessage = "ODO Reading required.")]
        public string ODOReading
        {
            get { return odoReading; }
            set { SetProperty(ref odoReading, value); }
        }

        private DateTime odoReadingDate;
        [Required(ErrorMessage = "ODO Reading Date required.")]
        public DateTime ODOReadingDate
        {
            get { return odoReadingDate; }
            set
            {
                SetProperty(ref odoReadingDate, value);
            }
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

        private ObservableCollection<DestinationType> destinationTypes;

        public ObservableCollection<DestinationType> DestinationTypes
        {
            get { return destinationTypes; }
            set { SetProperty(ref destinationTypes, value); }
        }


        private List<LocationType> locationTypes;

        public List<LocationType> LocationTypes
        {
            get { return locationTypes; }
            set { SetProperty(ref locationTypes, value); }

        }



        private long selectedlocRecId;

        public long SelectedLocRecId
        {
            get { return selectedlocRecId; }
            set { SetProperty(ref selectedlocRecId, value); }

        }

        //private LocationType selectedlocType;

        //public LocationType SelectedLocType
        //{
        //    get { return selectedlocType; }
        //    set { SetProperty(ref selectedlocType, value); }
        //}


        private string address;
        [Required(ErrorMessage = "Address required")]
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

        private DateTime serviceDateOption1;

        public DateTime ServiceDateOption1
        {
            get { return serviceDateOption1; }
            set
            {
                SetProperty(ref serviceDateOption1, value);
            }
        }

        private DateTime serviceDateOption2;
        public DateTime ServiceDateOption2
        {
            get { return serviceDateOption2; }
            set
            {
                SetProperty(ref serviceDateOption2, value);
            }
        }

        private string confirmedDate;

        public string ConfirmedDate
        {
            get { return confirmedDate; }
            set { SetProperty(ref confirmedDate , value); }
        }


        private DateTime supplierDateTime;
        public DateTime SupplierDateTime
        {
            get { return supplierDateTime; }
            set
            {
                SetProperty(ref supplierDateTime, value);
            }
        }

        private bool isLiftRequired;
        public bool IsLiftRequired
        {
            get { return isLiftRequired; }
            set { SetProperty(ref isLiftRequired, value); }
        }

        private LocationType selectedLocationType;
        [Required(ErrorMessage = "Location type required")]
        public LocationType SelectedLocationType
        {
            get { return selectedLocationType; }
            set { SetProperty(ref selectedLocationType, value); }
        }

        private DestinationType selectedDestinationType;
        [Required(ErrorMessage = "Destination type required")]
        public DestinationType SelectedDestinationType
        {
            get { return selectedDestinationType; }
            set { SetProperty(ref selectedDestinationType, value); }
        }

        private string selectedDestinationId;

        public string SelectedDestinationId
        {
            get { return selectedDestinationId; }
            set { SetProperty(ref selectedDestinationId, value); }
        }


        private string selectedServiceType;
        [Required(ErrorMessage = "Service type required")]
        public string SelectedServiceType
        {
            get { return selectedServiceType; }
            set { SetProperty(ref selectedServiceType, value); }
        }
    }
}
