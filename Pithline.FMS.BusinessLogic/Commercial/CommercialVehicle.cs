//using Pithline.FMS.BusinessLogic.Base;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Pithline.FMS.BusinessLogic.Commercial
//{
//    public class CommercialVehicle : BaseModel
//    {
//        public override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
//        {
//            throw new NotImplementedException();
//        }

//        private string color;

//        public string Color
//        {
//            get { return color; }
//            set { SetProperty(ref color, value); }
//        }
//        private string odoReading;

//        public string ODOReading
//        {
//            get { return odoReading; }
//            set { SetProperty(ref odoReading, value); }
//        }

//        private string registrationNumber;

//        public string RegistrationNumber
//        {
//            get { return registrationNumber; }
//            set { SetProperty(ref registrationNumber, value); }
//        }

//        private string engineNumber;

//        public string EngineNumber
//        {
//            get { return engineNumber; }
//            set { SetProperty(ref engineNumber, value); }
//        }

//        private bool isLicenseDiscCurrent;
      
//        public bool IsLicenseDiscCurrent
//        {
//            get { return isLicenseDiscCurrent; }
//            set { SetProperty(ref isLicenseDiscCurrent, value); }
//        }

//        private DateTime licenseDiscExpireDate;
       
//        public DateTime LicenseDiscExpireDate
//        {
//            get { return licenseDiscExpireDate; }
//            set { SetProperty(ref licenseDiscExpireDate, value); }
//        }

//        private string chassisNumber;

//        public string ChassisNumber
//        {
//            get { return chassisNumber; }
//            set { SetProperty(ref chassisNumber, value); }
//        }

//        private string make;

//        public string Make
//        {
//            get { return make; }
//            set { SetProperty(ref make, value); }
//        }
//        private string year;

//        public string Year
//        {
//            get { return year; }
//            set { SetProperty(ref year, value); }
//        }
//    }
//}
