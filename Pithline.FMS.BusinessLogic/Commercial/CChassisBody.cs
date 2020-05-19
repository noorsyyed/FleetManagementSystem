using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Commercial
{
    public class CChassisBody : BaseModel
    {
        public CChassisBody()
        {
            this.DoorsImgList = new ObservableCollection<ImageCapture>();
            this.ChassisImgList = new ObservableCollection<ImageCapture>();
            this.FloorImgList = new ObservableCollection<ImageCapture>();
            this.HeadboardImgList = new ObservableCollection<ImageCapture>();
            this.DropSideLeftImgList = new ObservableCollection<ImageCapture>();
            this.DropSideRightImgList = new ObservableCollection<ImageCapture>();
            this.DropSideFrontImgList = new ObservableCollection<ImageCapture>();
            this.DropSideRearImgList = new ObservableCollection<ImageCapture>();
            this.FuelTankImgList = new ObservableCollection<ImageCapture>();
            this.SpareWheelCarrierImgList = new ObservableCollection<ImageCapture>();
            this.UnderRunBumperImgList = new ObservableCollection<ImageCapture>();
            this.ChevronImgList = new ObservableCollection<ImageCapture>();
            this.LandingLegsImgList = new ObservableCollection<ImageCapture>();

        }

        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CChassisBody>(x => x.VehicleInsRecID == vehicleInsRecID);
        }

        private string doorsComment;

        public string DoorsComment
        {
            get { return doorsComment; }

            set { SetProperty(ref  doorsComment, value); }
        }
        private bool isDoorsDmg;

        public bool IsDoorsDmg
        {
            get { return isDoorsDmg; }

            set
            {
                if (SetProperty(ref  isDoorsDmg, value) && !this.IsDoorsDmg)
                {
                    this.DoorsImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> doorsImgList;

        [Ignore, DamageSnapshotRequired("Doors snapshot(s) required", "IsDoorsDmg")]
        public ObservableCollection<ImageCapture> DoorsImgList
        {
            get { return doorsImgList; }

            set { SetProperty(ref  doorsImgList, value); }
        }

        private string chassisComment;

        public string ChassisComment
        {
            get { return chassisComment; }

            set { SetProperty(ref  chassisComment, value); }
        }
        private bool isChassisDmg;

        public bool IsChassisDmg
        {
            get { return isChassisDmg; }

            set
            {
                if (SetProperty(ref  isChassisDmg, value) && !this.IsChassisDmg)
                {
                    this.ChassisImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> chassisImgList;

        [Ignore, DamageSnapshotRequired("Chassis snapshot(s) required", "IsChassisDmg")]
        public ObservableCollection<ImageCapture> ChassisImgList
        {
            get { return chassisImgList; }

            set { SetProperty(ref  chassisImgList, value); }
        }


        private string floorComment;

        public string FloorComment
        {
            get { return floorComment; }

            set { SetProperty(ref  floorComment, value); }
        }
        private bool isFloorDmg;

        public bool IsFloorDmg
        {
            get { return isFloorDmg; }

            set
            {
                if (SetProperty(ref  isFloorDmg, value) && !this.IsFloorDmg)
                {
                    this.FloorImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> floorImgList;

        [Ignore, DamageSnapshotRequired("Floor snapshot(s) required", "IsFloorDmg")]
        public ObservableCollection<ImageCapture> FloorImgList
        {
            get { return floorImgList; }

            set { SetProperty(ref  floorImgList, value); }
        }

        private string headboardComment;

        public string HeadboardComment
        {
            get { return headboardComment; }

            set { SetProperty(ref  headboardComment, value); }
        }
        private bool isHeadboardDmg;

        public bool IsHeadboardDmg
        {
            get { return isHeadboardDmg; }

            set
            {
                if (SetProperty(ref  isHeadboardDmg, value) && !this.IsHeadboardDmg)
                {
                    this.HeadboardImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> headboardImgList;

        [Ignore, DamageSnapshotRequired("Head board snapshot(s) required", "IsHeadboardDmg")]
        public ObservableCollection<ImageCapture> HeadboardImgList
        {
            get { return headboardImgList; }

            set { SetProperty(ref  headboardImgList, value); }
        }

        private string dropSideLeftComment;
        public string DropSideLeftComment
        {
            get { return dropSideLeftComment; }

            set { SetProperty(ref  dropSideLeftComment, value); }
        }
        private bool isDropSideLeftDmg;

        public bool IsDropSideLeftDmg
        {
            get { return isDropSideLeftDmg; }

            set
            {
                if (SetProperty(ref  isDropSideLeftDmg, value) && !this.IsDropSideLeftDmg)
                {
                    this.DropSideLeftImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> dropSideLeftImgList;

        [Ignore, DamageSnapshotRequired("Drop side left snapshot(s) required", "IsDropSideLeftDmg")]
        public ObservableCollection<ImageCapture> DropSideLeftImgList
        {
            get { return dropSideLeftImgList; }

            set { SetProperty(ref  dropSideLeftImgList, value); }
        }

        private string dropSideRightComment;

        public string DropSideRightComment
        {
            get { return dropSideRightComment; }

            set { SetProperty(ref  dropSideRightComment, value); }
        }
        private bool isDropSideRightDmg;

        public bool IsDropSideRightDmg
        {
            get { return isDropSideRightDmg; }

            set
            {
                if (SetProperty(ref  isDropSideRightDmg, value) && !this.IsDropSideRightDmg)
                {
                    this.DropSideRightImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> dropSideRightImgList;

        [Ignore, DamageSnapshotRequired("Drop side right snapshot(s) required", "IsDropSideRightDmg")]
        public ObservableCollection<ImageCapture> DropSideRightImgList
        {
            get { return dropSideRightImgList; }

            set { SetProperty(ref  dropSideRightImgList, value); }
        }

        private string dropSideFrontComment;

        public string DropSideFrontComment
        {
            get { return dropSideFrontComment; }

            set { SetProperty(ref  dropSideFrontComment, value); }
        }
        private bool isDropSideFrontDmg;

        public bool IsDropSideFrontDmg
        {
            get { return isDropSideFrontDmg; }

            set
            {
                if (SetProperty(ref  isDropSideFrontDmg, value) && !this.IsDropSideFrontDmg)
                {
                    this.DropSideFrontImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> dropSideFrontImgList;

        [Ignore, DamageSnapshotRequired("Drop side front snapshot(s) required", "IsDropSideFrontDmg")]
        public ObservableCollection<ImageCapture> DropSideFrontImgList
        {
            get { return dropSideFrontImgList; }

            set { SetProperty(ref  dropSideFrontImgList, value); }
        }

        private string dropSideRearComment;

        public string DropSideRearComment
        {
            get { return dropSideRearComment; }

            set { SetProperty(ref  dropSideRearComment, value); }
        }
        private bool isDropSideRearDmg;

        public bool IsDropSideRearDmg
        {
            get { return isDropSideRearDmg; }

            set
            {
                if (SetProperty(ref  isDropSideRearDmg, value) && !this.IsDropSideRearDmg)
                {
                    this.DropSideRearImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> dropSideRearImgList;

        [Ignore, DamageSnapshotRequired("Drop side rear snapshot(s) required", "IsDropSideRearDmg")]
        public ObservableCollection<ImageCapture> DropSideRearImgList
        {
            get { return dropSideRearImgList; }

            set { SetProperty(ref  dropSideRearImgList, value); }
        }

        private string fuelTankComment;

        public string FuelTankComment
        {
            get { return fuelTankComment; }

            set { SetProperty(ref  fuelTankComment, value); }
        }
        private bool isFuelTankDmg;

        public bool IsFuelTankDmg
        {
            get { return isFuelTankDmg; }

            set
            {
                if (SetProperty(ref  isFuelTankDmg, value) && !this.IsFuelTankDmg)
                {
                    this.FuelTankImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> fuelTankImgList;

        [Ignore, DamageSnapshotRequired("Fuel tank snapshot(s) required", "IsFuelTankDmg")]
        public ObservableCollection<ImageCapture> FuelTankImgList
        {
            get { return fuelTankImgList; }

            set { SetProperty(ref  fuelTankImgList, value); }
        }

        private string spareWheelCarrierComment;

        public string SpareWheelCarrierComment
        {
            get { return spareWheelCarrierComment; }

            set { SetProperty(ref  spareWheelCarrierComment, value); }
        }
        private bool isSpareWheelCarrierDmg;

        public bool IsSpareWheelCarrierDmg
        {
            get { return isSpareWheelCarrierDmg; }

            set
            {
                if (SetProperty(ref  isSpareWheelCarrierDmg, value) && !this.IsSpareWheelCarrierDmg)
                {
                    this.SpareWheelCarrierImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> spareWheelCarrierImgList;

        [Ignore, DamageSnapshotRequired("Spare wheel snapshot(s) required", "IsSpareWheelCarrierDmg")]
        public ObservableCollection<ImageCapture> SpareWheelCarrierImgList
        {
            get { return spareWheelCarrierImgList; }

            set { SetProperty(ref  spareWheelCarrierImgList, value); }
        }

        private string underRunBumperComment;

        public string UnderRunBumperComment
        {
            get { return underRunBumperComment; }

            set { SetProperty(ref  underRunBumperComment, value); }
        }
        private bool isUnderRunBumperDmg;

        public bool IsUnderRunBumperDmg
        {
            get { return isUnderRunBumperDmg; }

            set
            {
                if (SetProperty(ref  isUnderRunBumperDmg, value) && !this.IsUnderRunBumperDmg)
                {
                    this.UnderRunBumperImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> underRunBumperImgList;

        [Ignore, DamageSnapshotRequired("Under run bumper snapshot(s) required", "IsUnderRunBumperDmg")]
        public ObservableCollection<ImageCapture> UnderRunBumperImgList
        {
            get { return underRunBumperImgList; }

            set { SetProperty(ref  underRunBumperImgList, value); }
        }

        private string chevronComment;

        public string ChevronComment
        {
            get { return chevronComment; }

            set { SetProperty(ref  chevronComment, value); }
        }
        private bool isChevronDmg;

        public bool IsChevronDmg
        {
            get { return isChevronDmg; }

            set
            {
                if (SetProperty(ref  isChevronDmg, value) && !this.IsChevronDmg)
                {
                    this.ChevronImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> chevronImgList;

        [Ignore, DamageSnapshotRequired("Chevron snapshot(s) required", "IsChevronDmg")]
        public ObservableCollection<ImageCapture> ChevronImgList
        {
            get { return chevronImgList; }

            set { SetProperty(ref  chevronImgList, value); }
        }

        private string landingLegsComment;

        public string LandingLegsComment
        {
            get { return landingLegsComment; }

            set { SetProperty(ref  landingLegsComment, value); }
        }
        private bool isLandingLegsDmg;

        public bool IsLandingLegsDmg
        {
            get { return isLandingLegsDmg; }

            set
            {
                if (SetProperty(ref  isLandingLegsDmg, value) && !this.IsLandingLegsDmg)
                {
                    this.LandingLegsImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> landingLegsImgList;

        [Ignore, DamageSnapshotRequired("Landing legs snapshot(s) required", "IsLandingLegsDmg")]
        public ObservableCollection<ImageCapture> LandingLegsImgList
        {
            get { return landingLegsImgList; }

            set { SetProperty(ref  landingLegsImgList, value); }
        }


        public string doorsImgPathList;
        public string DoorsImgPathList
        {
            get { return string.Join("~", DoorsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref doorsImgPathList, value); }
        }

        public string chassisImgPathList;
        public string ChassisImgPathList
        {
            get { return string.Join("~", ChassisImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref chassisImgPathList, value); }
        }

        public string floorImgPathList;
        public string FloorImgPathList
        {
            get { return string.Join("~", FloorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref floorImgPathList, value); }
        }

        public string headboardImgPathList;
        public string HeadboardImgPathList
        {
            get { return string.Join("~", HeadboardImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref headboardImgPathList, value); }
        }

        public string dropSideLeftImgPathList;
        public string DropSideLeftImgPathList
        {
            get { return string.Join("~", DropSideLeftImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref dropSideLeftImgPathList, value); }
        }

        public string dropSideRightImgPathList;
        public string DropSideRightImgPathList
        {
            get { return string.Join("~", DropSideRightImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref dropSideRightImgPathList, value); }
        }

        public string dropSideFrontImgPathList;
        public string DropSideFrontImgPathList
        {
            get { return string.Join("~", DropSideFrontImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref dropSideFrontImgPathList, value); }
        }

        public string dropSideRearImgPathList;
        public string DropSideRearImgPathList
        {
            get { return string.Join("~", DropSideRearImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref dropSideRearImgPathList, value); }
        }

        public string fuelTankImgPathList;
        public string FuelTankImgPathList
        {
            get { return string.Join("~", FuelTankImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref fuelTankImgPathList, value); }
        }

        public string spareWheelCarrierImgPathList;
        public string SpareWheelCarrierImgPathList
        {
            get { return string.Join("~", SpareWheelCarrierImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref spareWheelCarrierImgPathList, value); }
        }

        public string underRunBumperImgPathList;
        public string UnderRunBumperImgPathList
        {
            get { return string.Join("~", UnderRunBumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref underRunBumperImgPathList, value); }
        }

        public string chevronImgPathList;
        public string ChevronImgPathList
        {
            get { return string.Join("~", ChevronImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref chevronImgPathList, value); }
        }

        public string landingLegsImgPathList;
        public string LandingLegsImgPathList
        {
            get { return string.Join("~", LandingLegsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref landingLegsImgPathList, value); }
        }
    }
}
