//using Pithline.FMS.BusinessLogic.Base;
//using Microsoft.Practices.Prism.StoreApps;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Pithline.FMS.BusinessLogic.Passenger
//{
//    public class PassengerVehicle : BaseModel
//    {
//        private string color;
//        [RestorableState]
//        public string Color
//        {
//            get { return color; }
//            set { SetProperty(ref color, value); }
//        }

//        private string odoReading;
//        [RestorableState]
//        public string ODOReading
//        {
//            get { return odoReading; }
//            set { SetProperty(ref odoReading, value); }
//        }

//        private string registrationNumber;
//        [RestorableState]
//        public string RegistrationNumber
//        {
//            get { return registrationNumber; }
//            set { SetProperty(ref registrationNumber, value); }
//        }

//        private string engineNumber;
//        [RestorableState]
//        public string EngineNumber
//        {
//            get { return engineNumber; }
//            set { SetProperty(ref engineNumber, value); }
//        }

//        private string chassisNumber;
//        [RestorableState]
//        public string ChassisNumber
//        {
//            get { return chassisNumber; }
//            set { SetProperty(ref chassisNumber, value); }
//        }

//        private string make;
//        [RestorableState]
//        public string Make
//        {
//            get { return make; }
//            set { SetProperty(ref make, value); }
//        }

//        private string year;
//        [RestorableState]
//        public string Year
//        {
//            get { return year; }
//            set { SetProperty(ref year, value); }
//        }

//        private bool isLicenseDiscCurrent;
//        [RestorableState]
//        public bool IsLicenseDiscCurrent
//        {
//            get { return isLicenseDiscCurrent; }
//            set { SetProperty(ref isLicenseDiscCurrent, value); }
//        }

//        private string licenseDiscExpireDate;
//        [RestorableState]
//        public string LicenseDiscExpireDate
//        {
//            get { return licenseDiscExpireDate; }
//            set { SetProperty(ref licenseDiscExpireDate, value); }
//        }

//        public override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
