using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Passenger
{
    public class PBodywork : BaseModel
    {
        public PBodywork()
        {
            this.HailDamageImgList = new ObservableCollection<ImageCapture>();
            this.RFDoorImgList = new ObservableCollection<ImageCapture>();
            this.RRDoorImgList = new ObservableCollection<ImageCapture>();
            this.LFDoorImgList = new ObservableCollection<ImageCapture>();
            this.LRDoorImgList = new ObservableCollection<ImageCapture>();

            this.RFBumperImgList = new ObservableCollection<ImageCapture>();
            this.LFBumperImgList = new ObservableCollection<ImageCapture>();
            this.RRBumperImgList = new ObservableCollection<ImageCapture>();
            this.LRBumperImgList = new ObservableCollection<ImageCapture>();
            this.BonnetImgList = new ObservableCollection<ImageCapture>();
            this.RoofImgList = new ObservableCollection<ImageCapture>();

            this.BootTailgateImgList = new ObservableCollection<ImageCapture>();
            this.DoorHandleImgList = new ObservableCollection<ImageCapture>();
            this.HubcapsImgList = new ObservableCollection<ImageCapture>();

            this.RightSideImgList = new ObservableCollection<ImageCapture>();
            this.LeftSideImgList = new ObservableCollection<ImageCapture>();
            this.RFWheelArchImgList = new ObservableCollection<ImageCapture>();

            this.LFWheelArchImgList = new ObservableCollection<ImageCapture>();
            this.RRWheelArchImgList = new ObservableCollection<ImageCapture>();
            this.LRWheelArchImgList = new ObservableCollection<ImageCapture>();
            this.WipersImgList = new ObservableCollection<ImageCapture>();


        }
        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<PBodywork>(x => x.VehicleInsRecID == vehicleInsRecID);
        }

        private ObservableCollection<ImageCapture> hailDamageImgList;
        [Ignore, DamageSnapshotRequired("Hail snapshot(s) required", "IsHailDmg")]
        public ObservableCollection<ImageCapture> HailDamageImgList
        {
            get { return hailDamageImgList; }
            set { SetProperty(ref hailDamageImgList, value); }
        }

        private ObservableCollection<ImageCapture> rFDoorImgList;
        [Ignore, DamageSnapshotRequired("RF door snapshot(s) required", "IsRFDoorDmg")]
        public ObservableCollection<ImageCapture> RFDoorImgList
        {
            get { return rFDoorImgList; }
            set { SetProperty(ref rFDoorImgList, value); }
        }

        private ObservableCollection<ImageCapture> rRDoorImgList;
        [Ignore, DamageSnapshotRequired("RR door snapshot(s) required", "IsRRDoorDmg")]
        public ObservableCollection<ImageCapture> RRDoorImgList
        {
            get { return rRDoorImgList; }
            set { SetProperty(ref rRDoorImgList, value); }
        }

        private ObservableCollection<ImageCapture> lFDoorImgList;
        [Ignore, DamageSnapshotRequired("LF door snapshot(s) required", "IsLFDoorDmg")]
        public ObservableCollection<ImageCapture> LFDoorImgList
        {
            get { return lFDoorImgList; }
            set { SetProperty(ref lFDoorImgList, value); }
        }
        private ObservableCollection<ImageCapture> lRDoorImgList;
        [Ignore, DamageSnapshotRequired("LR door snapshot(s) required", "IsLRDoorDmg")]
        public ObservableCollection<ImageCapture> LRDoorImgList
        {
            get { return lRDoorImgList; }
            set { SetProperty(ref lRDoorImgList, value); }
        }

        private ObservableCollection<ImageCapture> roofImgList;
        [Ignore, DamageSnapshotRequired("Roof snapshot(s) required", "IsRoofDmg")]
        public ObservableCollection<ImageCapture> RoofImgList
        {
            get { return roofImgList; }
            set { SetProperty(ref roofImgList, value); }
        }

        private ObservableCollection<ImageCapture> rFBumperImgList;
        [Ignore, DamageSnapshotRequired("RF bumper snapshot(s) required", "IsRFBumperDmg")]
        public ObservableCollection<ImageCapture> RFBumperImgList
        {
            get { return rFBumperImgList; }
            set { SetProperty(ref rFBumperImgList, value); }
        }

        private ObservableCollection<ImageCapture> lFBumperImgList;
        [Ignore, DamageSnapshotRequired("LF bumper snapshot(s) required", "IsLFBumperDmg")]
        public ObservableCollection<ImageCapture> LFBumperImgList
        {
            get { return lFBumperImgList; }
            set { SetProperty(ref lFBumperImgList, value); }
        }

        private ObservableCollection<ImageCapture> rRBumperImgList;
        [Ignore, DamageSnapshotRequired("RR bumper snapshot(s) required", "IsRRBumperDmg")]
        public ObservableCollection<ImageCapture> RRBumperImgList
        {
            get { return rRBumperImgList; }
            set { SetProperty(ref rRBumperImgList, value); }
        }

        private ObservableCollection<ImageCapture> lRBumperImgList;
        [Ignore, DamageSnapshotRequired("LR bumper snapshot(s) required", "IsLRBumperDmg")]
        public ObservableCollection<ImageCapture> LRBumperImgList
        {
            get { return lRBumperImgList; }
            set { SetProperty(ref lRBumperImgList, value); }
        }

        private ObservableCollection<ImageCapture> bonnetImgList;
        [Ignore, DamageSnapshotRequired("Bonnet snapshot(s) required", "IsBonnetDmg")]
        public ObservableCollection<ImageCapture> BonnetImgList
        {
            get { return bonnetImgList; }
            set { SetProperty(ref bonnetImgList, value); }
        }

        private ObservableCollection<ImageCapture> bootTailgateImgList;
        [Ignore, DamageSnapshotRequired("Boot tail gate snapshot(s) required", "IsBootTailgateDmg")]
        public ObservableCollection<ImageCapture> BootTailgateImgList
        {
            get { return bootTailgateImgList; }
            set { SetProperty(ref bootTailgateImgList, value); }
        }
        private ObservableCollection<ImageCapture> doorHandleImgList;
        [Ignore, DamageSnapshotRequired("Door handle snapshot(s) required", "IsDoorHandleDmg")]
        public ObservableCollection<ImageCapture> DoorHandleImgList
        {
            get { return doorHandleImgList; }
            set { SetProperty(ref doorHandleImgList, value); }
        }
        private ObservableCollection<ImageCapture> hubcapsImgList;
        [Ignore, DamageSnapshotRequired("Hubcap snapshot(s) required", "IsHubcapsDmg")]
        public ObservableCollection<ImageCapture> HubcapsImgList
        {
            get { return hubcapsImgList; }
            set { SetProperty(ref hubcapsImgList, value); }
        }

        private ObservableCollection<ImageCapture> rightSideImgList;
        [Ignore, DamageSnapshotRequired("Right side snapshot(s) required", "IsRightSideDmg")]
        public ObservableCollection<ImageCapture> RightSideImgList
        {
            get { return rightSideImgList; }
            set { SetProperty(ref  rightSideImgList, value); }
        }
        private ObservableCollection<ImageCapture> leftSideImgList;
        [Ignore, DamageSnapshotRequired("Left side snapshot(s) required", "IsLeftSideDmg")]
        public ObservableCollection<ImageCapture> LeftSideImgList
        {
            get { return leftSideImgList; }
            set { SetProperty(ref  leftSideImgList, value); }
        }
        private ObservableCollection<ImageCapture> rFWheelArchImgList;
        [Ignore, DamageSnapshotRequired("RF wheel arch snapshot(s) required", "IsRFWheelArchDmg")]
        public ObservableCollection<ImageCapture> RFWheelArchImgList
        {
            get { return rFWheelArchImgList; }
            set { SetProperty(ref  rFWheelArchImgList, value); }
        }
        private ObservableCollection<ImageCapture> lFWheelArchImgList;
        [Ignore, DamageSnapshotRequired("LF wheel arch snapshot(s) required", "IsLFWheelArchDmg")]
        public ObservableCollection<ImageCapture> LFWheelArchImgList
        {
            get { return lFWheelArchImgList; }
            set { SetProperty(ref  lFWheelArchImgList, value); }
        }
        private ObservableCollection<ImageCapture> rRWheelArchImgList;
        [Ignore, DamageSnapshotRequired("RR wheel arch snapshot(s) required", "IsRRWheelArchDmg")]
        public ObservableCollection<ImageCapture> RRWheelArchImgList
        {
            get { return rRWheelArchImgList; }
            set { SetProperty(ref  rRWheelArchImgList, value); }
        }

        private ObservableCollection<ImageCapture> lRWheelArchImgList;
        [Ignore, DamageSnapshotRequired("LR wheel arch snapshot(s) required", "IsLRWheelArchDmg")]
        public ObservableCollection<ImageCapture> LRWheelArchImgList
        {
            get { return lRWheelArchImgList; }
            set { SetProperty(ref  lRWheelArchImgList, value); }
        }
        private ObservableCollection<ImageCapture> wipersImgList;
        [Ignore, DamageSnapshotRequired("Wipers snapshot(s) required", "IsWipersDmg")]
        public ObservableCollection<ImageCapture> WipersImgList
        {
            get { return wipersImgList; }
            set { SetProperty(ref  wipersImgList, value); }
        }


        public string hailDamageImgPathList;
        public string HailDamageImgPathList
        {
            get { return string.Join("~", HailDamageImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref hailDamageImgPathList, value); }
        }

        public string rFDoorImgPathList;
        public string RFDoorImgPathList
        {
            get { return string.Join("~", RFDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFDoorImgPathList, value); }
        }

        public string rRDoorImgPathList;
        public string RRDoorImgPathList
        {
            get { return string.Join("~", RRDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRDoorImgPathList, value); }
        }

        public string lFDoorImgPathList;
        public string LFDoorImgPathList
        {
            get { return string.Join("~", LFDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFDoorImgPathList, value); }
        }

        public string lRDoorImgPathList;
        public string LRDoorImgPathList
        {
            get { return string.Join("~", LRDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRDoorImgPathList, value); }
        }

        public string rFBumperImgPathList;
        public string RFBumperImgPathList
        {
            get { return string.Join("~", RFBumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFBumperImgPathList, value); }
        }

        public string lFBumperImgPathList;
        public string LFBumperImgPathList
        {
            get { return string.Join("~", LFBumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFBumperImgPathList, value); }
        }

        public string rRBumperImgPathList;
        public string RRBumperImgPathList
        {
            get { return string.Join("~", RRBumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRBumperImgPathList, value); }
        }

        public string lRBumperImgPathList;
        public string LRBumperImgPathList
        {
            get { return string.Join("~", LRBumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRBumperImgPathList, value); }
        }

        public string bonnetImgPathList;
        public string BonnetImgPathList
        {
            get { return string.Join("~", BonnetImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref bonnetImgPathList, value); }
        }

        public string roofImgPathList;
        public string RoofImgPathList
        {
            get { return string.Join("~", RoofImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref roofImgPathList, value); }
        }

        public string bootTailgateImgPathList;
        public string BootTailgateImgPathList
        {
            get { return string.Join("~", BootTailgateImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref bootTailgateImgPathList, value); }
        }

        public string doorHandleImgPathList;
        public string DoorHandleImgPathList
        {
            get { return string.Join("~", DoorHandleImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref doorHandleImgPathList, value); }
        }

        public string hubcapsImgPathList;
        public string HubcapsImgPathList
        {
            get { return string.Join("~", HubcapsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref hubcapsImgPathList, value); }
        }

        public string rightSideImgPathList;
        public string RightSideImgPathList
        {
            get { return string.Join("~", RightSideImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rightSideImgPathList, value); }
        }

        public string leftSideImgPathList;
        public string LeftSideImgPathList
        {
            get { return string.Join("~", LeftSideImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref leftSideImgPathList, value); }
        }

        public string rFWheelArchImgPathList;
        public string RFWheelArchImgPathList
        {
            get { return string.Join("~", RFWheelArchImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFWheelArchImgPathList, value); }
        }

        public string lFWheelArchImgPathList;
        public string LFWheelArchImgPathList
        {
            get { return string.Join("~", LFWheelArchImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFWheelArchImgPathList, value); }
        }

        public string rRWheelArchImgPathList;
        public string RRWheelArchImgPathList
        {
            get { return string.Join("~", RRWheelArchImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRWheelArchImgPathList, value); }
        }

        public string lRWheelArchImgPathList;
        public string LRWheelArchImgPathList
        {
            get { return string.Join("~", LRWheelArchImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRWheelArchImgPathList, value); }
        }

        public string wipersImgPathList;
        public string WipersImgPathList
        {
            get { return string.Join("~", WipersImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref wipersImgPathList, value); }
        }

        private bool isHailDmg;

        public bool IsHailDmg
        {
            get { return isHailDmg; }

            set
            {
                if (SetProperty(ref  isHailDmg, value) && !this.IsHailDmg)
                {
                    this.HailDamageImgList.Clear();
                }
            }
        }
        private bool isRFDoorDmg;

        public bool IsRFDoorDmg
        {
            get { return isRFDoorDmg; }

            set
            {
                if (SetProperty(ref  isRFDoorDmg, value) && !this.IsRFDoorDmg)
                {
                    this.RFDoorImgList.Clear();
                }
            }
        }
        private bool isLFDoorDmg;

        public bool IsLFDoorDmg
        {
            get { return isLFDoorDmg; }

            set
            {
                if (SetProperty(ref  isLFDoorDmg, value) && !this.IsLFDoorDmg)
                {
                    this.LFDoorImgList.Clear();
                }
            }
        }
        private bool isRRDoorDmg;

        public bool IsRRDoorDmg
        {
            get { return isRRDoorDmg; }

            set
            {
                if (SetProperty(ref  isRRDoorDmg, value) && !this.IsRRDoorDmg)
                {
                    this.RRDoorImgList.Clear();
                }
            }
        }
        private bool isLRDoorDmg;

        public bool IsLRDoorDmg
        {
            get { return isLRDoorDmg; }

            set
            {
                if (SetProperty(ref  isLRDoorDmg, value) && !this.IsLRDoorDmg)
                {
                    this.LRDoorImgList.Clear();
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
        private bool isRFBumperDmg;

        public bool IsRFBumperDmg
        {
            get { return isRFBumperDmg; }

            set
            {
                if (SetProperty(ref  isRFBumperDmg, value) && !this.IsRFBumperDmg)
                {
                    this.RFBumperImgList.Clear();
                }
            }
        }
        private bool isLFBumperDmg;

        public bool IsLFBumperDmg
        {
            get { return isLFBumperDmg; }

            set
            {
                if (SetProperty(ref  isLFBumperDmg, value) && !this.IsLFBumperDmg)
                {
                    this.LFBumperImgList.Clear();
                }
            }
        }
        private bool isRRBumperDmg;

        public bool IsRRBumperDmg
        {
            get { return isRRBumperDmg; }

            set
            {
                if (SetProperty(ref  isRRBumperDmg, value) && !this.IsRRBumperDmg)
                {
                    this.RRBumperImgList.Clear();
                }
            }
        }
        private bool isBonnetDmg;

        public bool IsBonnetDmg
        {
            get { return isBonnetDmg; }

            set
            {
                if (SetProperty(ref  isBonnetDmg, value) && !this.IsBonnetDmg)
                {
                    this.BonnetImgList.Clear();
                }
            }
        }
        private bool isBootTailgateDmg;

        public bool IsBootTailgateDmg
        {
            get { return isBootTailgateDmg; }

            set
            {
                if (SetProperty(ref  isBootTailgateDmg, value) && !this.IsBootTailgateDmg)
                {
                    this.BootTailgateImgList.Clear();
                }
            }
        }
        private bool isDoorHandleDmg;

        public bool IsDoorHandleDmg
        {
            get { return isDoorHandleDmg; }

            set
            {
                if (SetProperty(ref  isDoorHandleDmg, value) && !this.IsDoorHandleDmg)
                {
                    this.DoorHandleImgList.Clear();
                }
            }
        }
        private bool isHubcapsDmg;

        public bool IsHubcapsDmg
        {
            get { return isHubcapsDmg; }

            set
            {
                if (SetProperty(ref  isHubcapsDmg, value) && !this.IsHubcapsDmg)
                {
                    this.HubcapsImgList.Clear();
                }
            }
        }
        private bool isRightSideDmg;

        public bool IsRightSideDmg
        {
            get { return isRightSideDmg; }

            set
            {
                if (SetProperty(ref  isRightSideDmg, value) && !this.IsRightSideDmg)
                {
                    this.RightSideImgList.Clear();
                }
            }
        }
        private bool isLeftSideDmg;

        public bool IsLeftSideDmg
        {
            get { return isLeftSideDmg; }

            set
            {
                if (SetProperty(ref  isLeftSideDmg, value) && !this.IsLeftSideDmg)
                {
                    this.LeftSideImgList.Clear();
                }
            }
        }
        private bool isRFWheelArchDmg;

        public bool IsRFWheelArchDmg
        {
            get { return isRFWheelArchDmg; }

            set
            {
                if (SetProperty(ref  isRFWheelArchDmg, value) && !this.IsRFWheelArchDmg)
                {
                    this.RFWheelArchImgList.Clear();
                }
            }
        }
        private bool isLRBumperDmg;

        public bool IsLRBumperDmg
        {
            get { return isLRBumperDmg; }

            set
            {
                if (SetProperty(ref  isLRBumperDmg, value) && !this.IsLRBumperDmg)
                {
                    this.LRBumperImgList.Clear();
                }
            }
        }
        private bool isLFWheelArchDmg;

        public bool IsLFWheelArchDmg
        {
            get { return isLFWheelArchDmg; }
            set
            {
                if (SetProperty(ref  isLFWheelArchDmg, value) && !this.IsLFWheelArchDmg)
                {
                    this.LFWheelArchImgList.Clear();
                }
            }
        }
        private bool isRRWheelArchDmg;

        public bool IsRRWheelArchDmg
        {
            get { return isRRWheelArchDmg; }

            set
            {
                if (SetProperty(ref  isRRWheelArchDmg, value) && !this.IsRRWheelArchDmg)
                {
                    this.RRWheelArchImgList.Clear();
                }
            }
        }
        private bool isLRWheelArchDmg;

        public bool IsLRWheelArchDmg
        {
            get { return isLRWheelArchDmg; }

            set
            {
                if (SetProperty(ref  isLRWheelArchDmg, value) && !this.IsLRWheelArchDmg)
                {
                    this.LRWheelArchImgList.Clear();
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



        private string gVHailDamageComment;

        public string GVHailDamageComment
        {
            get { return gVHailDamageComment; }

            set { SetProperty(ref  gVHailDamageComment, value); }
        }
        private string gVRFDoorComment;

        public string GVRFDoorComment
        {
            get { return gVRFDoorComment; }

            set { SetProperty(ref  gVRFDoorComment, value); }
        }
        private string gVLFDoorComment;

        public string GVLFDoorComment
        {
            get { return gVLFDoorComment; }

            set { SetProperty(ref  gVLFDoorComment, value); }
        }
        private string gVRRDoorComment;

        public string GVRRDoorComment
        {
            get { return gVRRDoorComment; }

            set { SetProperty(ref  gVRRDoorComment, value); }
        }
        private string gVLRDoorComment;

        public string GVLRDoorComment
        {
            get { return gVLRDoorComment; }

            set { SetProperty(ref  gVLRDoorComment, value); }
        }
        private string gVRoofComment;

        public string GVRoofComment
        {
            get { return gVRoofComment; }

            set { SetProperty(ref  gVRoofComment, value); }
        }
        private string gVRFBumperComment;

        public string GVRFBumperComment
        {
            get { return gVRFBumperComment; }

            set { SetProperty(ref  gVRFBumperComment, value); }
        }
        private string gVLFBumperComment;

        public string GVLFBumperComment
        {
            get { return gVLFBumperComment; }

            set { SetProperty(ref  gVLFBumperComment, value); }
        }
        private string gVRRBumperComment;

        public string GVRRBumperComment
        {
            get { return gVRRBumperComment; }

            set { SetProperty(ref  gVRRBumperComment, value); }
        }
        private string gVBonnetComment;

        public string GVBonnetComment
        {
            get { return gVBonnetComment; }

            set { SetProperty(ref  gVBonnetComment, value); }
        }
        private string gVBootTailgateComment;

        public string GVBootTailgateComment
        {
            get { return gVBootTailgateComment; }

            set { SetProperty(ref  gVBootTailgateComment, value); }
        }
        private string gVLFDoorHandleComment;

        public string GVLFDoorHandleComment
        {
            get { return gVLFDoorHandleComment; }

            set { SetProperty(ref  gVLFDoorHandleComment, value); }
        }
        private string gVHubcapsComment;

        public string GVHubcapsComment
        {
            get { return gVHubcapsComment; }

            set { SetProperty(ref  gVHubcapsComment, value); }
        }
        private string gVRightSideComment;

        public string GVRightSideComment
        {
            get { return gVRightSideComment; }

            set { SetProperty(ref  gVRightSideComment, value); }
        }
        private string gVLeftSideComment;

        public string GVLeftSideComment
        {
            get { return gVLeftSideComment; }

            set { SetProperty(ref  gVLeftSideComment, value); }
        }
        private string gVRFWheelArchComment;

        public string GVRFWheelArchComment
        {
            get { return gVRFWheelArchComment; }

            set { SetProperty(ref  gVRFWheelArchComment, value); }
        }
        private string gVLFWheelArchToggleComment;

        public string GVLFWheelArchToggleComment
        {
            get { return gVLFWheelArchToggleComment; }

            set { SetProperty(ref  gVLFWheelArchToggleComment, value); }
        }
        private string gVRRWheelArchComment;

        public string GVRRWheelArchComment
        {
            get { return gVRRWheelArchComment; }

            set { SetProperty(ref  gVRRWheelArchComment, value); }
        }
        private string gVLRWheelArchComment;

        public string GVLRWheelArchComment
        {
            get { return gVLRWheelArchComment; }

            set { SetProperty(ref  gVLRWheelArchComment, value); }
        }
        private string gVLRBumperComment;

        public string GVLRBumperComment
        {
            get { return gVLRBumperComment; }

            set { SetProperty(ref  gVLRBumperComment, value); }
        }
        private string gVWipersComment;

        public string GVWipersComment
        {
            get { return gVWipersComment; }

            set { SetProperty(ref  gVWipersComment, value); }
        }
    }
}

