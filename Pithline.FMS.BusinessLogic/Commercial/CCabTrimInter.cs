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
    public class CCabTrimInter : BaseModel
    {
        public CCabTrimInter()
        {
            this.LeftDoorImgList = new ObservableCollection<ImageCapture>();
            this.RightDoorImgList = new ObservableCollection<ImageCapture>();
            this.LFQuaterPanelImgList = new ObservableCollection<ImageCapture>();
            this.RFQuaterPanelImgList = new ObservableCollection<ImageCapture>();
            this.LRQuaterPanelImgList = new ObservableCollection<ImageCapture>();
            this.RRQuaterPanelImgList = new ObservableCollection<ImageCapture>();
            this.FrontViewImgList = new ObservableCollection<ImageCapture>();
            this.BumperImgList = new ObservableCollection<ImageCapture>();
            this.GrillImgList = new ObservableCollection<ImageCapture>();
            this.RearMirrorImgList = new ObservableCollection<ImageCapture>();
            this.WheelArchLeftImgList = new ObservableCollection<ImageCapture>();
            this.RoofImgList = new ObservableCollection<ImageCapture>();
            this.DoorHandleLeftImgList = new ObservableCollection<ImageCapture>();
            this.WheelArchRightImgList = new ObservableCollection<ImageCapture>();
            this.DoorHandleRightImgList = new ObservableCollection<ImageCapture>();
            this.InternalTrimImgList = new ObservableCollection<ImageCapture>();
            this.WipersImgList = new ObservableCollection<ImageCapture>();
            this.MatImgList = new ObservableCollection<ImageCapture>();
            this.DriverSeatImgList = new ObservableCollection<ImageCapture>();
            this.PassengerSeatImgList = new ObservableCollection<ImageCapture>();

            this.DashboardImgList = new ObservableCollection<ImageCapture>();
            this.EmblemsImgList = new ObservableCollection<ImageCapture>();
            this.SteeringWheelsImgList = new ObservableCollection<ImageCapture>();
            this.InstrumentClusterImgList = new ObservableCollection<ImageCapture>();
            this.ControlLeversImgList = new ObservableCollection<ImageCapture>();
            this.BackUpWarningImgList = new ObservableCollection<ImageCapture>();
            this.OverheadGuardImgList = new ObservableCollection<ImageCapture>();
            this.RainGuardImgList = new ObservableCollection<ImageCapture>();
            this.MastHydraulicsImgList = new ObservableCollection<ImageCapture>();
            this.MastImgList = new ObservableCollection<ImageCapture>();
            this.ForksImgList = new ObservableCollection<ImageCapture>();
        }

        private ObservableCollection<ImageCapture> leftDoorImgList;
        [Ignore, DamageSnapshotRequired("Left door snapshot(s) required", "IsLeftDoorDmg")]
        public ObservableCollection<ImageCapture> LeftDoorImgList
        {
            get { return leftDoorImgList; }

            set { SetProperty(ref  leftDoorImgList, value); }
        }
        private ObservableCollection<ImageCapture> rightDoorImgList;
        [Ignore, DamageSnapshotRequired("Right door snapshot(s) required", "IsRightDoorDmg")]
        public ObservableCollection<ImageCapture> RightDoorImgList
        {
            get { return rightDoorImgList; }

            set { SetProperty(ref  rightDoorImgList, value); }
        }
        private ObservableCollection<ImageCapture> lfQuaterPanelImgList;
        [Ignore, DamageSnapshotRequired("LF quater panel snapshot(s) required", "IsLFQuatPanelDmg")]
        public ObservableCollection<ImageCapture> LFQuaterPanelImgList
        {
            get { return lfQuaterPanelImgList; }

            set { SetProperty(ref  lfQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> rfQuaterPanelImgList;
        [Ignore, DamageSnapshotRequired("RF quater panel snapshot(s) required", "IsRFQuatPanelDmg")]
        public ObservableCollection<ImageCapture> RFQuaterPanelImgList
        {
            get { return rfQuaterPanelImgList; }

            set { SetProperty(ref  rfQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> lrQuaterPanelImgList;
        [Ignore, DamageSnapshotRequired("LR quater panel  snapshot(s) required", "IsLRQuatPanelDmg")]
        public ObservableCollection<ImageCapture> LRQuaterPanelImgList
        {
            get { return lrQuaterPanelImgList; }

            set { SetProperty(ref  lrQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> rrQuaterPanelImgList;
        [Ignore, DamageSnapshotRequired("RR quater panel snapshot(s) required", "IsRRQuatPanelDmg")]
        public ObservableCollection<ImageCapture> RRQuaterPanelImgList
        {
            get { return rrQuaterPanelImgList; }

            set { SetProperty(ref  rrQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> frontViewImgList;
        [Ignore, DamageSnapshotRequired("Front view  snapshot(s) required", "IsFrontViewDmg")]
        public ObservableCollection<ImageCapture> FrontViewImgList
        {
            get { return frontViewImgList; }

            set { SetProperty(ref  frontViewImgList, value); }
        }
        private ObservableCollection<ImageCapture> bumperImgList;
        [Ignore, DamageSnapshotRequired("Bumper  snapshot(s) required", "IsBumperDmg")]
        public ObservableCollection<ImageCapture> BumperImgList
        {
            get { return bumperImgList; }

            set { SetProperty(ref  bumperImgList, value); }
        }
        private ObservableCollection<ImageCapture> grillImgList;
        [Ignore, DamageSnapshotRequired("Grill snapshot(s) required", "IsGrillDmg")]
        public ObservableCollection<ImageCapture> GrillImgList
        {
            get { return grillImgList; }

            set { SetProperty(ref  grillImgList, value); }
        }
        private ObservableCollection<ImageCapture> rearMirrorImgList;
        [Ignore, DamageSnapshotRequired("Rear mirror snapshot(s) required", "IsRearMirrorDmg")]
        public ObservableCollection<ImageCapture> RearMirrorImgList
        {
            get { return rearMirrorImgList; }

            set { SetProperty(ref  rearMirrorImgList, value); }
        }
        private ObservableCollection<ImageCapture> wheelArchLeftImgList;
        [Ignore, DamageSnapshotRequired("Wheel arch left snapshot(s) required", "IsWheelArchLeftDmg")]
        public ObservableCollection<ImageCapture> WheelArchLeftImgList
        {
            get { return wheelArchLeftImgList; }

            set { SetProperty(ref  wheelArchLeftImgList, value); }
        }
        private ObservableCollection<ImageCapture> wheelArchRightImgList;
        [Ignore, DamageSnapshotRequired("Wheel arch right snapshot(s) required", "IsWheelArchRightDmg")]
        public ObservableCollection<ImageCapture> WheelArchRightImgList
        {
            get { return wheelArchRightImgList; }

            set { SetProperty(ref  wheelArchRightImgList, value); }
        }
        private ObservableCollection<ImageCapture> roofImgList;
        [Ignore, DamageSnapshotRequired("Roof  snapshot(s) required", "IsRoofDmg")]
        public ObservableCollection<ImageCapture> RoofImgList
        {
            get { return roofImgList; }

            set { SetProperty(ref  roofImgList, value); }
        }
        private ObservableCollection<ImageCapture> doorHandleLeftImgList;
        [Ignore, DamageSnapshotRequired("Door handle left snapshot(s) required", "IsDoorHandleLeftDmg")]
        public ObservableCollection<ImageCapture> DoorHandleLeftImgList
        {
            get { return doorHandleLeftImgList; }

            set { SetProperty(ref  doorHandleLeftImgList, value); }
        }
        private ObservableCollection<ImageCapture> doorHandleRightImgList;
        [Ignore, DamageSnapshotRequired("Door handle right snapshot(s) required", "IsDoorHandleRightDmg")]
        public ObservableCollection<ImageCapture> DoorHandleRightImgList
        {
            get { return doorHandleRightImgList; }

            set { SetProperty(ref  doorHandleRightImgList, value); }
        }
        private ObservableCollection<ImageCapture> wipersImgList;
        [Ignore, DamageSnapshotRequired("Wipers  snapshot(s) required", "IsWipersDmg")]
        public ObservableCollection<ImageCapture> WipersImgList
        {
            get { return wipersImgList; }

            set { SetProperty(ref  wipersImgList, value); }
        }
        private ObservableCollection<ImageCapture> internalTrimImgList;
        [Ignore, DamageSnapshotRequired("Internal snapshot(s) required", "IsInternalTrimDmg")]
        public ObservableCollection<ImageCapture> InternalTrimImgList
        {
            get { return internalTrimImgList; }

            set { SetProperty(ref  internalTrimImgList, value); }
        }
        private ObservableCollection<ImageCapture> matImgList;
        [Ignore, DamageSnapshotRequired("Mat snapshot(s) required", "IsMatsDmg")]
        public ObservableCollection<ImageCapture> MatImgList
        {
            get { return matImgList; }

            set { SetProperty(ref  matImgList, value); }
        }
        private ObservableCollection<ImageCapture> driverSeatImgList;
        [Ignore, DamageSnapshotRequired("Driver seat  snapshot(s) required", "IsDriverSeatDmg")]
        public ObservableCollection<ImageCapture> DriverSeatImgList
        {
            get { return driverSeatImgList; }

            set { SetProperty(ref  driverSeatImgList, value); }
        }
        private ObservableCollection<ImageCapture> passengerSeatImgList;
        [Ignore, DamageSnapshotRequired("Passenger seat snapshot(s) required", "IsPassengerSeatDmg")]
        public ObservableCollection<ImageCapture> PassengerSeatImgList
        {
            get { return passengerSeatImgList; }

            set { SetProperty(ref  passengerSeatImgList, value); }
        }

        public string leftDoorImgPathList;
        public string LeftDoorImgPathList
        {
            get { return string.Join("~", LeftDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref leftDoorImgPathList, value); }
        }

        public string rightDoorImgPathList;
        public string RightDoorImgPathList
        {
            get { return string.Join("~", RightDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rightDoorImgPathList, value); }
        }

        public string lFQuaterPanelImgPathList;
        public string LFQuaterPanelImgPathList
        {
            get { return string.Join("~", LFQuaterPanelImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFQuaterPanelImgPathList, value); }
        }

        public string rFQuaterPanelImgPathList;
        public string RFQuaterPanelImgPathList
        {
            get { return string.Join("~", RFQuaterPanelImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFQuaterPanelImgPathList, value); }
        }

        public string lRQuaterPanelImgPathList;
        public string LRQuaterPanelImgPathList
        {
            get { return string.Join("~", LRQuaterPanelImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRQuaterPanelImgPathList, value); }
        }

        public string rRQuaterPanelImgPathList;
        public string RRQuaterPanelImgPathList
        {
            get { return string.Join("~", RRQuaterPanelImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRQuaterPanelImgPathList, value); }
        }

        public string frontViewImgPathList;
        public string FrontViewImgPathList
        {
            get { return string.Join("~", FrontViewImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref frontViewImgPathList, value); }
        }

        public string bumperImgPathList;
        public string BumperImgPathList
        {
            get { return string.Join("~", BumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref bumperImgPathList, value); }
        }

        public string grillImgPathList;
        public string GrillImgPathList
        {
            get { return string.Join("~", GrillImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref grillImgPathList, value); }
        }

        public string rearMirrorImgPathList;
        public string RearMirrorImgPathList
        {
            get { return string.Join("~", RearMirrorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rearMirrorImgPathList, value); }
        }

        public string wheelArchLeftImgPathList;
        public string WheelArchLeftImgPathList
        {
            get { return string.Join("~", WheelArchLeftImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref wheelArchLeftImgPathList, value); }
        }

        public string roofImgPathList;
        public string RoofImgPathList
        {
            get { return string.Join("~", RoofImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref roofImgPathList, value); }
        }

        public string doorHandleLeftImgPathList;
        public string DoorHandleLeftImgPathList
        {
            get { return string.Join("~", DoorHandleLeftImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref doorHandleLeftImgPathList, value); }
        }

        public string wheelArchRightImgPathList;
        public string WheelArchRightImgPathList
        {
            get { return string.Join("~", WheelArchRightImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref wheelArchRightImgPathList, value); }
        }

        public string doorHandleRightImgPathList;
        public string DoorHandleRightImgPathList
        {
            get { return string.Join("~", DoorHandleRightImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref doorHandleRightImgPathList, value); }
        }

        public string internalTrimImgPathList;
        public string InternalTrimImgPathList
        {
            get { return string.Join("~", InternalTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref internalTrimImgPathList, value); }
        }

        public string wipersImgPathList;
        public string WipersImgPathList
        {
            get { return string.Join("~", WipersImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref wipersImgPathList, value); }
        }

        public string matImgPathList;
        public string MatImgPathList
        {
            get { return string.Join("~", MatImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref matImgPathList, value); }
        }

        public string driverSeatImgPathList;
        public string DriverSeatImgPathList
        {
            get { return string.Join("~", DriverSeatImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref driverSeatImgPathList, value); }
        }

        public string passengerSeatImgPathList;
        public string PassengerSeatImgPathList
        {
            get { return string.Join("~", PassengerSeatImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref passengerSeatImgPathList, value); }
        }
        private string leftDoorComment;

        public string LeftDoorComment
        {
            get { return leftDoorComment; }

            set { SetProperty(ref  leftDoorComment, value); }
        }
        private string rightDoorComment;

        public string RightDoorComment
        {
            get { return rightDoorComment; }

            set { SetProperty(ref  rightDoorComment, value); }
        }
        private string lFQuatPanelComment;

        public string LFQuatPanelComment
        {
            get { return lFQuatPanelComment; }

            set { SetProperty(ref  lFQuatPanelComment, value); }
        }
        private string rFQuatPanelComment;

        public string RFQuatPanelComment
        {
            get { return rFQuatPanelComment; }

            set { SetProperty(ref  rFQuatPanelComment, value); }
        }
        private string lRQuatPanelComment;

        public string LRQuatPanelComment
        {
            get { return lRQuatPanelComment; }

            set { SetProperty(ref  lRQuatPanelComment, value); }
        }
        private string rRQuatPanelComment;

        public string RRQuatPanelComment
        {
            get { return rRQuatPanelComment; }

            set { SetProperty(ref  rRQuatPanelComment, value); }
        }
        private string frontViewComment;

        public string FrontViewComment
        {
            get { return frontViewComment; }

            set { SetProperty(ref  frontViewComment, value); }
        }
        private string bumperComment;

        public string BumperComment
        {
            get { return bumperComment; }

            set { SetProperty(ref  bumperComment, value); }
        }
        private string grillComment;

        public string GrillComment
        {
            get { return grillComment; }

            set { SetProperty(ref  grillComment, value); }
        }
        private string rearMirrorComment;

        public string RearMirrorComment
        {
            get { return rearMirrorComment; }

            set { SetProperty(ref  rearMirrorComment, value); }
        }
        private string wheelArchLeftComment;

        public string WheelArchLeftComment
        {
            get { return wheelArchLeftComment; }

            set { SetProperty(ref  wheelArchLeftComment, value); }
        }
        private string wheelArchRightComment;

        public string WheelArchRightComment
        {
            get { return wheelArchRightComment; }

            set { SetProperty(ref  wheelArchRightComment, value); }
        }
        private string roofComment;

        public string RoofComment
        {
            get { return roofComment; }

            set { SetProperty(ref  roofComment, value); }
        }
        private string doorHandleLeftComment;

        public string DoorHandleLeftComment
        {
            get { return doorHandleLeftComment; }

            set { SetProperty(ref  doorHandleLeftComment, value); }
        }
        private string doorHandleRightComment;

        public string DoorHandleRightComment
        {
            get { return doorHandleRightComment; }

            set { SetProperty(ref  doorHandleRightComment, value); }
        }
        private string wipersComment;

        public string WipersComment
        {
            get { return wipersComment; }

            set { SetProperty(ref  wipersComment, value); }
        }
        private string internalTrimComment;

        public string InternalTrimComment
        {
            get { return internalTrimComment; }

            set { SetProperty(ref  internalTrimComment, value); }
        }
        private string matsComment;

        public string MatsComment
        {
            get { return matsComment; }

            set { SetProperty(ref  matsComment, value); }
        }
        private string driverSeatComment;

        public string DriverSeatComment
        {
            get { return driverSeatComment; }

            set { SetProperty(ref  driverSeatComment, value); }
        }
        private string passengerSeatComment;

        public string PassengerSeatComment
        {
            get { return passengerSeatComment; }

            set { SetProperty(ref  passengerSeatComment, value); }
        }

        private bool isLeftDoorDmg;

        public bool IsLeftDoorDmg
        {
            get { return isLeftDoorDmg; }

            set
            {
                if (SetProperty(ref  isLeftDoorDmg, value) && !this.IsLeftDoorDmg)
                {
                    this.LeftDoorImgList.Clear();
                }
            }
        }
        private bool isRightDoorDmg;

        public bool IsRightDoorDmg
        {
            get { return isRightDoorDmg; }

            set
            {
                if (SetProperty(ref  isRightDoorDmg, value) && !this.IsRightDoorDmg)
                {
                    this.RightDoorImgList.Clear();
                }
            }
        }
        private bool isLFQuatPanelDmg;

        public bool IsLFQuatPanelDmg
        {
            get { return isLFQuatPanelDmg; }

            set
            {
                if (SetProperty(ref  isLFQuatPanelDmg, value) && !this.IsLFQuatPanelDmg)
                {
                    this.LFQuaterPanelImgList.Clear();
                }
            }
        }
        private bool isRFQuatPanelDmg;

        public bool IsRFQuatPanelDmg
        {
            get { return isRFQuatPanelDmg; }

            set
            {
                if (SetProperty(ref  isRFQuatPanelDmg, value) && !this.IsRFQuatPanelDmg)
                {
                    this.RFQuaterPanelImgList.Clear();
                }
            }
        }
        private bool isLRQuatPanelDmg;

        public bool IsLRQuatPanelDmg
        {
            get { return isLRQuatPanelDmg; }

            set
            {
                if (SetProperty(ref  isLRQuatPanelDmg, value) && !this.IsLRQuatPanelDmg)
                {
                    this.LRQuaterPanelImgList.Clear();
                }
            }
        }
        private bool isRRQuatPanelDmg;

        public bool IsRRQuatPanelDmg
        {
            get { return isRRQuatPanelDmg; }

            set
            {
                if (SetProperty(ref  isRRQuatPanelDmg, value) && !this.IsRRQuatPanelDmg)
                {
                    this.RRQuaterPanelImgList.Clear();
                }
            }
        }
        private bool isFrontViewDmg;

        public bool IsFrontViewDmg
        {
            get { return isFrontViewDmg; }

            set
            {
                if (SetProperty(ref  isFrontViewDmg, value) && !this.IsFrontViewDmg)
                {
                    this.FrontViewImgList.Clear();
                }
            }
        }
        private bool isBumperDmg;

        public bool IsBumperDmg
        {
            get { return isBumperDmg; }

            set
            {
                if (SetProperty(ref  isBumperDmg, value) && !this.IsBumperDmg)
                {
                    this.BumperImgList.Clear();
                }
            }
        }
        private bool isGrillDmg;

        public bool IsGrillDmg
        {
            get { return isGrillDmg; }

            set
            {
                if (SetProperty(ref  isGrillDmg, value) && !this.IsGrillDmg)
                {
                    this.GrillImgList.Clear();
                }
            }
        }
        private bool isRearMirrorDmg;

        public bool IsRearMirrorDmg
        {
            get { return isRearMirrorDmg; }

            set
            {
                if (SetProperty(ref  isRearMirrorDmg, value) && !this.IsRearMirrorDmg)
                {
                    this.RearMirrorImgList.Clear();
                }
            }
        }
        private bool isWheelArchLeftDmg;

        public bool IsWheelArchLeftDmg
        {
            get { return isWheelArchLeftDmg; }

            set
            {
                if (SetProperty(ref  isWheelArchLeftDmg, value) && !this.IsWheelArchLeftDmg)
                {
                    this.WheelArchLeftImgList.Clear();
                }
            }
        }
        private bool isWheelArchRightDmg;

        public bool IsWheelArchRightDmg
        {
            get { return isWheelArchRightDmg; }

            set
            {
                if (SetProperty(ref  isWheelArchRightDmg, value) && !this.IsWheelArchRightDmg)
                {
                    this.WheelArchRightImgList.Clear();
                }
            }
        }
        private bool isRoofDmg;

        public bool IsRoofDmg
        {
            get { return isRoofDmg; }

            set
            {
                if (SetProperty(ref  isRoofDmg, value) && !this.IsRoofDmg)
                {
                    this.RoofImgList.Clear();
                }
            }
        }
        private bool isDoorHandleLeftDmg;

        public bool IsDoorHandleLeftDmg
        {
            get { return isDoorHandleLeftDmg; }

            set
            {
                if (SetProperty(ref  isDoorHandleLeftDmg, value) && !this.IsDoorHandleLeftDmg)
                {
                    this.DoorHandleLeftImgList.Clear();
                }
            }
        }
        private bool isDoorHandleRightDmg;

        public bool IsDoorHandleRightDmg
        {
            get { return isDoorHandleRightDmg; }

            set
            {
                if (SetProperty(ref  isDoorHandleRightDmg, value) && !this.IsDoorHandleRightDmg)
                {
                    this.DoorHandleRightImgList.Clear();
                }
            }
        }
        private bool isWipersDmg;

        public bool IsWipersDmg
        {
            get { return isWipersDmg; }

            set
            {
                if (SetProperty(ref  isWipersDmg, value) && !this.IsWipersDmg)
                {
                    this.WipersImgList.Clear();
                }
            }
        }
        private bool isInternalTrimDmg;

        public bool IsInternalTrimDmg
        {
            get { return isInternalTrimDmg; }

            set
            {
                if (SetProperty(ref  isInternalTrimDmg, value) && !this.IsInternalTrimDmg)
                {
                    this.InternalTrimImgList.Clear();
                }
            }
        }
        private bool isMatsDmg;

        public bool IsMatsDmg
        {
            get { return isMatsDmg; }

            set
            {
                if (SetProperty(ref  isMatsDmg, value) && !this.IsMatsDmg)
                {
                    this.MatImgList.Clear();
                }
            }
        }
        private bool isDriverSeatDmg;

        public bool IsDriverSeatDmg
        {
            get { return isDriverSeatDmg; }

            set
            {
                if (SetProperty(ref  isDriverSeatDmg, value) && !this.IsDriverSeatDmg)
                {
                    this.DriverSeatImgList.Clear();
                }
            }
        }
        private bool isPassengerSeatDmg;

        public bool IsPassengerSeatDmg
        {
            get { return isPassengerSeatDmg; }

            set
            {
                if (SetProperty(ref  isPassengerSeatDmg, value) && !this.IsPassengerSeatDmg)
                {
                    this.PassengerSeatImgList.Clear();
                }
            }
        }




        private ObservableCollection<ImageCapture> dashboardImgList;
        [Ignore, DamageSnapshotRequired("Dashboard snapshot(s) required", "IsDashboardDmg")]
        public ObservableCollection<ImageCapture> DashboardImgList
        {
            get { return dashboardImgList; }

            set { SetProperty(ref  dashboardImgList, value); }
        }

        public string dashboardImgPathList;
        public string DashboardImgPathList
        {
            get { return string.Join("~", DashboardImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref dashboardImgPathList, value); }
        }

        private string dashboardComment;

        public string DashboardComment
        {
            get { return dashboardComment; }

            set { SetProperty(ref  dashboardComment, value); }
        }

        private bool isdashboardDmg;

        public bool IsDashboardDmg
        {
            get { return isdashboardDmg; }

            set
            {
                if (SetProperty(ref  isdashboardDmg, value) && !this.IsDashboardDmg)
                {
                    this.DashboardImgList.Clear();
                }
            }
        }


        private ObservableCollection<ImageCapture> emblemsImgList;
        [Ignore, DamageSnapshotRequired("Emblems snapshot(s) required", "IsEmblemsDmg")]
        public ObservableCollection<ImageCapture> EmblemsImgList
        {
            get { return emblemsImgList; }

            set { SetProperty(ref  emblemsImgList, value); }
        }

        public string emblemsImgPathList;
        public string EmblemsImgPathList
        {
            get { return string.Join("~", EmblemsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref emblemsImgPathList, value); }
        }

        private string emblemsComment;

        public string EmblemsComment
        {
            get { return emblemsComment; }

            set { SetProperty(ref  emblemsComment, value); }
        }

        private bool isemblemsDmg;

        public bool IsEmblemsDmg
        {
            get { return isemblemsDmg; }

            set
            {
                if (SetProperty(ref  isemblemsDmg, value) && !this.IsEmblemsDmg)
                {
                    this.EmblemsImgList.Clear();
                }
            }
        }



        private ObservableCollection<ImageCapture> steeringwheelsImgList;
        [Ignore, DamageSnapshotRequired("Steering Wheels snapshot(s) required", "IsSteeringWheelsDmg")]
        public ObservableCollection<ImageCapture> SteeringWheelsImgList
        {
            get { return steeringwheelsImgList; }

            set { SetProperty(ref  steeringwheelsImgList, value); }
        }

        public string steeringwheelsImgPathList;
        public string SteeringWheelsImgPathList
        {
            get { return string.Join("~", SteeringWheelsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref steeringwheelsImgPathList, value); }
        }

        private string steeringwheelsComment;

        public string SteeringWheelsComment
        {
            get { return steeringwheelsComment; }

            set { SetProperty(ref  steeringwheelsComment, value); }
        }

        private bool issteeringwheelsDmg;

        public bool IsSteeringWheelsDmg
        {
            get { return issteeringwheelsDmg; }

            set
            {
                if (SetProperty(ref  issteeringwheelsDmg, value) && !this.IsSteeringWheelsDmg)
                {
                    this.SteeringWheelsImgList.Clear();
                }
            }
        }


        private ObservableCollection<ImageCapture> instrumentclusterImgList;
        [Ignore, DamageSnapshotRequired("Instrument Cluster snapshot(s) required", "IsInstrumentClusterDmg")]
        public ObservableCollection<ImageCapture> InstrumentClusterImgList
        {
            get { return instrumentclusterImgList; }

            set { SetProperty(ref  instrumentclusterImgList, value); }
        }

        public string instrumentclusterImgPathList;
        public string InstrumentClusterImgPathList
        {
            get { return string.Join("~", InstrumentClusterImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref instrumentclusterImgPathList, value); }
        }

        private string instrumentclusterComment;

        public string InstrumentClusterComment
        {
            get { return instrumentclusterComment; }

            set { SetProperty(ref  instrumentclusterComment, value); }
        }

        private bool isinstrumentclusterDmg;

        public bool IsInstrumentClusterDmg
        {
            get { return isinstrumentclusterDmg; }

            set
            {
                if (SetProperty(ref  isinstrumentclusterDmg, value) && !this.IsInstrumentClusterDmg)
                {
                    this.InstrumentClusterImgList.Clear();
                }
            }
        }


        private ObservableCollection<ImageCapture> controlleversImgList;
        [Ignore, DamageSnapshotRequired("Control Levers snapshot(s) required", "IsControlLeversDmg")]
        public ObservableCollection<ImageCapture> ControlLeversImgList
        {
            get { return controlleversImgList; }

            set { SetProperty(ref  controlleversImgList, value); }
        }

        public string controlleversImgPathList;
        public string ControlLeversImgPathList
        {
            get { return string.Join("~", ControlLeversImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref controlleversImgPathList, value); }
        }

        private string controlleversComment;

        public string ControlLeversComment
        {
            get { return controlleversComment; }

            set { SetProperty(ref  controlleversComment, value); }
        }

        private bool iscontrolleversDmg;

        public bool IsControlLeversDmg
        {
            get { return iscontrolleversDmg; }

            set
            {
                if (SetProperty(ref  iscontrolleversDmg, value) && !this.IsControlLeversDmg)
                {
                    this.ControlLeversImgList.Clear();
                }
            }
        }

        private ObservableCollection<ImageCapture> backupwarningImgList;
        [Ignore, DamageSnapshotRequired("BackUp Warning snapshot(s) required", "IsBackUpWarningDmg")]
        public ObservableCollection<ImageCapture> BackUpWarningImgList
        {
            get { return backupwarningImgList; }

            set { SetProperty(ref  backupwarningImgList, value); }
        }

        public string backupwarningImgPathList;
        public string BackUpWarningImgPathList
        {
            get { return string.Join("~", BackUpWarningImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref backupwarningImgPathList, value); }
        }

        private string backupwarningComment;

        public string BackUpWarningComment
        {
            get { return backupwarningComment; }

            set { SetProperty(ref  backupwarningComment, value); }
        }

        private bool isbackupwarningDmg;

        public bool IsBackUpWarningDmg
        {
            get { return isbackupwarningDmg; }

            set
            {
                if (SetProperty(ref  isbackupwarningDmg, value) && !this.IsBackUpWarningDmg)
                {
                    this.BackUpWarningImgList.Clear();
                }
            }
        }



        private ObservableCollection<ImageCapture> overheadguardImgList;
        [Ignore, DamageSnapshotRequired("OverheadGuard snapshot(s) required", "IsOverheadGuardDmg")]
        public ObservableCollection<ImageCapture> OverheadGuardImgList
        {
            get { return overheadguardImgList; }

            set { SetProperty(ref  overheadguardImgList, value); }
        }

        public string overheadguardImgPathList;
        public string OverheadGuardImgPathList
        {
            get { return string.Join("~", OverheadGuardImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref overheadguardImgPathList, value); }
        }

        private string overheadguardComment;

        public string OverheadGuardComment
        {
            get { return overheadguardComment; }

            set { SetProperty(ref  overheadguardComment, value); }
        }

        private bool isoverheadguardDmg;

        public bool IsOverheadGuardDmg
        {
            get { return isoverheadguardDmg; }

            set
            {
                if (SetProperty(ref  isoverheadguardDmg, value) && !this.IsOverheadGuardDmg)
                {
                    this.OverheadGuardImgList.Clear();
                }
            }
        }

        private ObservableCollection<ImageCapture> rainguardImgList;
        [Ignore, DamageSnapshotRequired("Rain Guard snapshot(s) required", "IsRainGuardDmg")]
        public ObservableCollection<ImageCapture> RainGuardImgList
        {
            get { return rainguardImgList; }

            set { SetProperty(ref  rainguardImgList, value); }
        }

        public string rainguardImgPathList;
        public string RainGuardImgPathList
        {
            get { return string.Join("~", RainGuardImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rainguardImgPathList, value); }
        }

        private string rainguardComment;

        public string RainGuardComment
        {
            get { return rainguardComment; }

            set { SetProperty(ref  rainguardComment, value); }
        }

        private bool israinguardDmg;

        public bool IsRainGuardDmg
        {
            get { return israinguardDmg; }

            set
            {
                if (SetProperty(ref  israinguardDmg, value) && !this.IsRainGuardDmg)
                {
                    this.RainGuardImgList.Clear();
                }
            }
        }



        private ObservableCollection<ImageCapture> mastImgList;
        [Ignore, DamageSnapshotRequired("Mast snapshot(s) required", "IsMastDmg")]
        public ObservableCollection<ImageCapture> MastImgList
        {
            get { return mastImgList; }

            set { SetProperty(ref  mastImgList, value); }
        }

        public string mastImgPathList;
        public string MastImgPathList
        {
            get { return string.Join("~", MastImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref mastImgPathList, value); }
        }

        private string mastComment;

        public string MastComment
        {
            get { return mastComment; }

            set { SetProperty(ref  mastComment, value); }
        }

        private bool ismastDmg;

        public bool IsMastDmg
        {
            get { return ismastDmg; }

            set
            {
                if (SetProperty(ref  ismastDmg, value) && !this.IsMastDmg)
                {
                    this.MastImgList.Clear();
                }
            }
        }

        private ObservableCollection<ImageCapture> masthydraulicsImgList;
        [Ignore, DamageSnapshotRequired("Mast Hydraulics snapshot(s) required", "IsMastHydraulicsDmg")]
        public ObservableCollection<ImageCapture> MastHydraulicsImgList
        {
            get { return masthydraulicsImgList; }

            set { SetProperty(ref  masthydraulicsImgList, value); }
        }

        public string masthydraulicsImgPathList;
        public string MastHydraulicsImgPathList
        {
            get { return string.Join("~", MastHydraulicsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref masthydraulicsImgPathList, value); }
        }

        private string masthydraulicsComment;

        public string MastHydraulicsComment
        {
            get { return masthydraulicsComment; }

            set { SetProperty(ref  masthydraulicsComment, value); }
        }

        private bool ismasthydraulicsDmg;

        public bool IsMastHydraulicsDmg
        {
            get { return ismasthydraulicsDmg; }

            set
            {
                if (SetProperty(ref  ismasthydraulicsDmg, value) && !this.IsMastHydraulicsDmg)
                {
                    this.MastHydraulicsImgList.Clear();
                }
            }
        }


        private ObservableCollection<ImageCapture> forksImgList;
        [Ignore, DamageSnapshotRequired("Forks snapshot(s) required", "IsForksDmg")]
        public ObservableCollection<ImageCapture> ForksImgList
        {
            get { return forksImgList; }

            set { SetProperty(ref  forksImgList, value); }
        }

        public string forksImgPathList;
        public string ForksImgPathList
        {
            get { return string.Join("~", ForksImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref forksImgPathList, value); }
        }

        private string forksComment;

        public string ForksComment
        {
            get { return forksComment; }

            set { SetProperty(ref  forksComment, value); }
        }

        private bool isforksDmg;

        public bool IsForksDmg
        {
            get { return isforksDmg; }

            set
            {
                if (SetProperty(ref  isforksDmg, value) && !this.IsForksDmg)
                {
                    this.ForksImgList.Clear();
                }
            }
        }


        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CCabTrimInter>(x => x.VehicleInsRecID == vehicleInsRecID);
        }
    }
}
