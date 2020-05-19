using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Passenger
{
    public class PTrimInterior : BaseModel
    {
        public PTrimInterior()
        {
            this.InternalTrimImgList = new ObservableCollection<ImageCapture>();
            this.RRDoorTrimImgList = new ObservableCollection<ImageCapture>();
            this.LRDoorTrimImgList = new ObservableCollection<ImageCapture>();
            this.RFDoorTrimImgList = new ObservableCollection<ImageCapture>();
            this.LFDoorTrimImgList = new ObservableCollection<ImageCapture>();
            this.DriverSeatImgList = new ObservableCollection<ImageCapture>();
            this.PassengerSeatImgList = new ObservableCollection<ImageCapture>();
            this.RearSeatImgList = new ObservableCollection<ImageCapture>();
            this.DashImgList = new ObservableCollection<ImageCapture>();
            this.CarpetImgList = new ObservableCollection<ImageCapture>();
            // this.CarpetImgList.Add(new ImageCapture() { ImagePath = "ms-appx:///Images/images.png" });
        }

        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<PTrimInterior>(x => x.VehicleInsRecID == vehicleInsRecID);
        }

        private ObservableCollection<ImageCapture> internalTrimImgList;
        [RestorableState, Ignore, DamageSnapshotRequired("Internal snapshot(s) required", "IsInternalTrimDmg")]
        public ObservableCollection<ImageCapture> InternalTrimImgList
        {
            get { return internalTrimImgList; }
            set { SetProperty(ref internalTrimImgList, value); }
        }

        private ObservableCollection<ImageCapture> rrDoorTrimiImgList;
        [RestorableState, Ignore, DamageSnapshotRequired("RR door snapshot(s) required", "IsRRDoorTrimDmg")]

        public ObservableCollection<ImageCapture> RRDoorTrimImgList
        {
            get { return rrDoorTrimiImgList; }
            set { SetProperty(ref rrDoorTrimiImgList, value); }
        }

        private ObservableCollection<ImageCapture> lrDoorTrimImgList;
        [RestorableState, Ignore, DamageSnapshotRequired("LR door snapshot(s) required", "IsLRDoorTrimDmg")]

        public ObservableCollection<ImageCapture> LRDoorTrimImgList
        {
            get { return lrDoorTrimImgList; }
            set { SetProperty(ref lrDoorTrimImgList, value); }
        }

        private ObservableCollection<ImageCapture> rfDoorTrimImgList;
        [RestorableState, Ignore, DamageSnapshotRequired("RF door snapshot(s) required", "IsRFDoorTrimDmg")]

        public ObservableCollection<ImageCapture> RFDoorTrimImgList
        {
            get { return rfDoorTrimImgList; }
            set { SetProperty(ref rfDoorTrimImgList, value); }
        }

        private ObservableCollection<ImageCapture> lfDoorTrimImgList;
        [RestorableState, Ignore, DamageSnapshotRequired("LF door snapshot(s) required", "IsLFDoorTrimDmg")]

        public ObservableCollection<ImageCapture> LFDoorTrimImgList
        {
            get { return lfDoorTrimImgList; }
            set { SetProperty(ref lfDoorTrimImgList, value); }
        }
        private ObservableCollection<ImageCapture> driverSeatImgList;
        [RestorableState, Ignore, DamageSnapshotRequired("Driver seat snapshot(s) required", "IsDriverSeatDmg")]

        public ObservableCollection<ImageCapture> DriverSeatImgList
        {
            get { return driverSeatImgList; }
            set { SetProperty(ref driverSeatImgList, value); }
        }

        private ObservableCollection<ImageCapture> passengerSeatImgList;
        [RestorableState, Ignore, DamageSnapshotRequired("Passenger seat snapshot(s) required", "IsPassengerSeatDmg")]

        public ObservableCollection<ImageCapture> PassengerSeatImgList
        {
            get { return passengerSeatImgList; }
            set { SetProperty(ref passengerSeatImgList, value); }
        }
        private ObservableCollection<ImageCapture> rearSeatImgList;
        [RestorableState, Ignore, DamageSnapshotRequired("Rear seat snapshot(s) required", "IsRearSeatDmg")]

        public ObservableCollection<ImageCapture> RearSeatImgList
        {
            get { return rearSeatImgList; }
            set { SetProperty(ref rearSeatImgList, value); }
        }

        private ObservableCollection<ImageCapture> dashImgList;
        [RestorableState, Ignore, DamageSnapshotRequired("Dash snapshot(s) required", "IsDashDmg")]

        public ObservableCollection<ImageCapture> DashImgList
        {
            get { return dashImgList; }
            set { SetProperty(ref dashImgList, value); }
        }

        private ObservableCollection<ImageCapture> carpetImgList;
        [RestorableState, Ignore, DamageSnapshotRequired("Carpet snapshot(s) required", "IsCarpetDmg")]
        public ObservableCollection<ImageCapture> CarpetImgList
        {
            get { return carpetImgList; }

            set { SetProperty(ref carpetImgList, value); }
        }


        public string internalTrimImgPathList;
        public string InternalTrimImgPathList
        {
            get { return string.Join("~", InternalTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref internalTrimImgPathList, value); }
        }

        public string rRDoorTrimImgPathList;
        public string RRDoorTrimImgPathList
        {
            get { return string.Join("~", RRDoorTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRDoorTrimImgPathList, value); }
        }

        public string lRDoorTrimImgPathList;
        public string LRDoorTrimImgPathList
        {
            get { return string.Join("~", LRDoorTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRDoorTrimImgPathList, value); }
        }

        public string rFDoorTrimImgPathList;
        public string RFDoorTrimImgPathList
        {
            get { return string.Join("~", RFDoorTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFDoorTrimImgPathList, value); }
        }

        public string lFDoorTrimImgPathList;
        public string LFDoorTrimImgPathList
        {
            get { return string.Join("~", LFDoorTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFDoorTrimImgPathList, value); }
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

        public string rearSeatImgPathList;
        public string RearSeatImgPathList
        {
            get { return string.Join("~", RearSeatImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rearSeatImgPathList, value); }
        }


        public string dashImgPathList;
        public string DashImgPathList
        {
            get { return string.Join("~", DashImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref dashImgPathList, value); }
        }


        public string carpetImgPathList;
        public string CarpetImgPathList
        {
            get { return string.Join("~", CarpetImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref carpetImgPathList, value); }
        }


        private string internalTrimComment;
        [RestorableState]
        public string InternalTrimComment
        {
            get { return internalTrimComment; }
            set { SetProperty(ref internalTrimComment, value); }
        }

        private bool isInternalTrimDmg;
        [RestorableState]
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

        private bool isRRDoorTrimDmg;
        [RestorableState]
        public bool IsRRDoorTrimDmg
        {
            get { return isRRDoorTrimDmg; }
            set
            {
                if (SetProperty(ref  isRRDoorTrimDmg, value) && !this.IsRRDoorTrimDmg)
                {
                    this.RRDoorTrimImgList.Clear();
                }
            }
        }

        private string rrDoorTrimComment;
        [RestorableState]
        public string RRDoorTrimComment
        {
            get { return rrDoorTrimComment; }
            set { SetProperty(ref rrDoorTrimComment, value); }
        }

        private bool isLFDoorTrimDmg;
        [RestorableState]
        public bool IsLFDoorTrimDmg
        {
            get { return isLFDoorTrimDmg; }
            set
            {
                if (SetProperty(ref  isLFDoorTrimDmg, value) && !this.IsLFDoorTrimDmg)
                {
                    this.LFDoorTrimImgList.Clear();
                }
            }
        }

        private string lfDoorTrimComment;
        [RestorableState]
        public string LFDoorTrimComment
        {
            get { return lfDoorTrimComment; }
            set { SetProperty(ref lfDoorTrimComment, value); }
        }

        private string rfDoorTrimComment;
        [RestorableState]
        public string RFDoorTrimComment
        {
            get { return rfDoorTrimComment; }
            set { SetProperty(ref rfDoorTrimComment, value); }
        }

        private bool isRFDoorTrimDmg;
        [RestorableState]
        public bool IsRFDoorTrimDmg
        {
            get { return isRFDoorTrimDmg; }
            set
            {
                if (SetProperty(ref  isRFDoorTrimDmg, value) && !this.IsRFDoorTrimDmg)
                {
                    this.RFDoorTrimImgList.Clear();
                }
            }
        }

        private string lrDoorTrimComment;
        [RestorableState]
        public string LRDoorTrimComment
        {
            get { return lrDoorTrimComment; }
            set { SetProperty(ref lrDoorTrimComment, value); }
        }

        private bool isLRDoorTrimDmg;
        [RestorableState]
        public bool IsLRDoorTrimDmg
        {
            get { return isLRDoorTrimDmg; }
            set
            {
                if (SetProperty(ref  isLRDoorTrimDmg, value) && !this.IsLRDoorTrimDmg)
                {
                    this.LRDoorTrimImgList.Clear();
                }
            }
        }

        private bool isDriverSeatDmg;
        [RestorableState]
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

        private string driverSeatComment;
        [RestorableState]
        public string DriverSeatComment
        {
            get { return driverSeatComment; }
            set { SetProperty(ref driverSeatComment, value); }
        }

        private bool isPassengerSeatDmg;
        [RestorableState]
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

        private string passengerSeatComment;
        [RestorableState]
        public string PassengerSeatComment
        {
            get { return passengerSeatComment; }
            set { SetProperty(ref passengerSeatComment, value); }
        }

        private bool isRearSeatDmg;
        [RestorableState]
        public bool IsRearSeatDmg
        {
            get { return isRearSeatDmg; }
            set
            {
                if (SetProperty(ref  isRearSeatDmg, value) && !this.IsRearSeatDmg)
                {
                    this.RearSeatImgList.Clear();
                }
            }
        }

        private string rearSeatComment;
        [RestorableState]
        public string RearSeatComment
        {
            get { return rearSeatComment; }
            set { SetProperty(ref rearSeatComment, value); }
        }

        private bool isDashDmg;
        [RestorableState]
        public bool IsDashDmg
        {
            get { return isDashDmg; }
            set
            {
                if (SetProperty(ref  isDashDmg, value) && !this.IsDashDmg)
                {
                    this.DashImgList.Clear();
                }
            }
        }

        private string dashComment;
        [RestorableState]
        public string DashComment
        {
            get { return dashComment; }
            set { SetProperty(ref dashComment, value); }
        }

        private bool isCarpetDmg;
        [RestorableState]
        public bool IsCarpetDmg
        {
            get { return isCarpetDmg; }
            set
            {
                if (SetProperty(ref  isCarpetDmg, value) && !this.IsCarpetDmg)
                {
                    this.CarpetImgList.Clear();
                }
            }
        }
        private string carpetComment;
        [RestorableState]
        public string CarpetComment
        {
            get { return carpetComment; }
            set { SetProperty(ref carpetComment, value); }
        }

    }
}
