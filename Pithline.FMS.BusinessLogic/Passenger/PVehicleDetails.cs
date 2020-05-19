using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
//using Syncfusion.CompoundFile.XlsIO.Native;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Passenger
{
    public class PVehicleDetails : BaseModel
    {

        public PVehicleDetails()
        {
            this.LeftSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/car_left.png" };
            this.BackSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/car_back.png" };
            this.RightSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/car_right.png" };
            this.FrontSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/car_front.png" };
            this.TopSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/car_top.png" };
            this.LicenseDiscSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/license_disc.png" };
            this.ODOReadingSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/ODO_meter.png" };
        }

        public async override System.Threading.Tasks.Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            try
            {
                return await SqliteHelper.Storage.GetSingleRecordAsync<PVehicleDetails>(x => x.VehicleInsRecID.Equals(vehicleInsRecID));
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private bool isLicenseDiscCurrent;

        public bool IsLicenseDiscCurrent
        {
            get { return isLicenseDiscCurrent; }
            set { SetProperty(ref isLicenseDiscCurrent, value); }
        }

        private string licenseDiscExpiryDate;

        public string LicenseDiscExpireDate
        {
            get { return licenseDiscExpiryDate; }
            set { SetProperty(ref licenseDiscExpiryDate, value); }
        }


        private bool isSpareKeysShown;


        public bool IsSpareKeysShown
        {
            get { return isSpareKeysShown; }
            set { SetProperty(ref isSpareKeysShown, value); }
        }

        private bool isSpareKeysTested;

        public bool IsSpareKeysTested
        {
            get { return isSpareKeysTested; }
            set { SetProperty(ref isSpareKeysTested, value); }
        }

        private ImageCapture licenseDiscSnapshot;

        [RestorableState, Ignore]
        public ImageCapture LicenseDiscSnapshot
        {
            get { return licenseDiscSnapshot; }
            set { SetProperty(ref licenseDiscSnapshot, value); }
        }

        private ImageCapture odoReadingSnapshot;

        [RestorableState, Ignore]
        public ImageCapture ODOReadingSnapshot
        {
            get { return odoReadingSnapshot; }
            set { SetProperty(ref odoReadingSnapshot, value); }
        }


        private ImageCapture leftSnapshot;

        [RestorableState, Ignore]
        public ImageCapture LeftSnapshot
        {
            get { return leftSnapshot; }
            set { SetProperty(ref leftSnapshot, value); }
        }

        private ImageCapture backSnapshot;

        [RestorableState, Ignore]
        public ImageCapture BackSnapshot
        {
            get { return backSnapshot; }
            set { SetProperty(ref backSnapshot, value); }
        }

        private ImageCapture rightSnapshot;

        [RestorableState, Ignore]
        public ImageCapture RightSnapshot
        {
            get { return rightSnapshot; }
            set { SetProperty(ref rightSnapshot, value); }
        }

        private ImageCapture frontSnapshot;

        [RestorableState, Ignore]
        public ImageCapture FrontSnapshot
        {
            get { return frontSnapshot; }
            set { SetProperty(ref frontSnapshot, value); }
        }

        private ImageCapture topSnapshot;

        [RestorableState, Ignore]
        public ImageCapture TopSnapshot
        {
            get { return topSnapshot; }
            set { SetProperty(ref topSnapshot, value); }
        }

        public string leftSnapshotPath;
        public string LeftSnapshotPath
        {
            get { return LeftSnapshot.ImagePath; }
            set { SetProperty(ref leftSnapshotPath, value); }
        }

        public string backSnapshotPath;
        public string BackSnapshotPath
        {
            get { return BackSnapshot.ImagePath; }
            set { SetProperty(ref backSnapshotPath, value); }
        }

        public string rightSnapshotPath;
        public string RightSnapshotPath
        {
            get { return RightSnapshot.ImagePath; }
            set { SetProperty(ref rightSnapshotPath, value); }
        }

        public string frontSnapshotPath;
        public string FrontSnapshotPath
        {
            get { return FrontSnapshot.ImagePath; }
            set { SetProperty(ref frontSnapshotPath, value); }
        }

        public string topSnapshotPath;
        public string TopSnapshotPath
        {
            get { return TopSnapshot.ImagePath; }
            set { SetProperty(ref topSnapshotPath, value); }
        }

        public string licenseDiscSnapshotPath;
        public string LicenseDiscSnapshotPath
        {
            get { return LicenseDiscSnapshot.ImagePath; }
            set { SetProperty(ref licenseDiscSnapshotPath, value); }
        }

        public string oDOReadingSnapshotPath;
        public string ODOReadingSnapshotPath
        {
            get { return ODOReadingSnapshot.ImagePath; }
            set { SetProperty(ref oDOReadingSnapshotPath, value); }
        }

        private string color;

        public string Color
        {
            get { return color; }
            set { SetProperty(ref color, value); }
        }

        private string odoReading;

        public string ODOReading
        {
            get { return odoReading; }
            set { SetProperty(ref odoReading, value); }
        }

        private string registrationNumber;

        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }

        private string engineNumber;

        public string EngineNumber
        {
            get { return engineNumber; }
            set { SetProperty(ref engineNumber, value); }
        }

        private string chassisNumber;
        public string ChassisNumber
        {
            get { return chassisNumber; }
            set { SetProperty(ref chassisNumber, value); }
        }

        private string make;

        public string Make
        {
            get { return make; }
            set { SetProperty(ref make, value); }
        }

        private string year;

        public string Year
        {
            get { return year; }
            set { SetProperty(ref year, value); }
        }


    }
}
