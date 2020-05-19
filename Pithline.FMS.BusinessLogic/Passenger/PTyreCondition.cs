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
    public class PTyreCondition : BaseModel
    {
        public PTyreCondition()
        {
            this.RFImgList = new ObservableCollection<ImageCapture>();
            this.LFImgList = new ObservableCollection<ImageCapture>();
            this.RRImgList = new ObservableCollection<ImageCapture>();
            this.SpareImgList = new ObservableCollection<ImageCapture>();
            this.LRImgList = new ObservableCollection<ImageCapture>();
        }
        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<PTyreCondition>(x => x.VehicleInsRecID == vehicleInsRecID);
        }
        private ObservableCollection<ImageCapture> rFImgList;

        [Ignore, DamageSnapshotRequired("RF snapshot(s) required", "IsRFPChecked")]
        public ObservableCollection<ImageCapture> RFImgList
        {
            get { return rFImgList; }

            set { SetProperty(ref  rFImgList, value); }
        }

        private ObservableCollection<ImageCapture> lFImgList;

        [Ignore, DamageSnapshotRequired("LF snapshot(s) required", "IsLFPChecked")]
        public ObservableCollection<ImageCapture> LFImgList
        {
            get { return lFImgList; }

            set { SetProperty(ref  lFImgList, value); }
        }

        private ObservableCollection<ImageCapture> rRImgList;

        [Ignore, DamageSnapshotRequired("RR snapshot(s) required", "IsRRPChecked")]
        public ObservableCollection<ImageCapture> RRImgList
        {
            get { return rRImgList; }

            set { SetProperty(ref  rRImgList, value); }
        }

        private ObservableCollection<ImageCapture> lRImgList;

        [Ignore, DamageSnapshotRequired("LR snapshot(s) required", "IsLRPChecked")]
        public ObservableCollection<ImageCapture> LRImgList
        {
            get { return lRImgList; }

            set { SetProperty(ref  lRImgList, value); }
        }

        private ObservableCollection<ImageCapture> spareImgList;

        [Ignore, DamageSnapshotRequired("Spare snapshot(s) required", "IsSparePChecked")]
        public ObservableCollection<ImageCapture> SpareImgList
        {
            get { return spareImgList; }

            set { SetProperty(ref  spareImgList, value); }
        }



        private string rFTreadDepth;

        public string RFTreadDepth
        {
            get { return rFTreadDepth; }

            set { SetProperty(ref  rFTreadDepth, value); }
        }
        private string rFMake;

        public string RFMake
        {
            get { return rFMake; }

            set { SetProperty(ref  rFMake, value); }
        }
        private string rFComment;

        public string RFComment
        {
            get { return rFComment; }

            set { SetProperty(ref  rFComment, value); }
        }
        private string lFTreadDepth;

        public string LFTreadDepth
        {
            get { return lFTreadDepth; }

            set { SetProperty(ref  lFTreadDepth, value); }
        }
        private string lFMake;

        public string LFMake
        {
            get { return lFMake; }

            set { SetProperty(ref  lFMake, value); }
        }
        private string lFComment;

        public string LFComment
        {
            get { return lFComment; }

            set { SetProperty(ref  lFComment, value); }
        }
        private string rRTreadDepth;

        public string RRTreadDepth
        {
            get { return rRTreadDepth; }

            set { SetProperty(ref  rRTreadDepth, value); }
        }
        private string rRMake;

        public string RRMake
        {
            get { return rRMake; }

            set { SetProperty(ref  rRMake, value); }
        }
        private string rRComment;

        public string RRComment
        {
            get { return rRComment; }

            set { SetProperty(ref  rRComment, value); }
        }
        private string lRTreadDepth;

        public string LRTreadDepth
        {
            get { return lRTreadDepth; }

            set { SetProperty(ref  lRTreadDepth, value); }
        }
        private string lRMake;

        public string LRMake
        {
            get { return lRMake; }

            set { SetProperty(ref  lRMake, value); }
        }
        private string lRComment;

        public string LRComment
        {
            get { return lRComment; }

            set { SetProperty(ref  lRComment, value); }
        }
        private string spareTreadDepth;

        public string SpareTreadDepth
        {
            get { return spareTreadDepth; }

            set { SetProperty(ref  spareTreadDepth, value); }
        }
        private string spareMake;

        public string SpareMake
        {
            get { return spareMake; }

            set { SetProperty(ref  spareMake, value); }
        }
        private string spareComment;

        public string SpareComment
        {
            get { return spareComment; }

            set { SetProperty(ref  spareComment, value); }
        }

        private bool isRFGChecked;

        public bool IsRFGChecked
        {
            get { return isRFGChecked; }

            set { SetProperty(ref  isRFGChecked, value); }
        }
        private bool isRFFChecked;

        public bool IsRFFChecked
        {
            get { return isRFFChecked; }

            set { SetProperty(ref  isRFFChecked, value); }
        }
        private bool isRFPChecked;

        public bool IsRFPChecked
        {
            get { return isRFPChecked; }

            set
            {
                if (SetProperty(ref  isRFPChecked, value) && !this.IsRFPChecked)
                {
                    this.RFImgList.Clear();
                }
            }
        }
        private bool isLFGChecked;

        public bool IsLFGChecked
        {
            get { return isLFGChecked; }

            set { SetProperty(ref  isLFGChecked, value); }
        }
        private bool isLFFChecked;

        public bool IsLFFChecked
        {
            get { return isLFFChecked; }

            set { SetProperty(ref  isLFFChecked, value); }
        }
        private bool isLFPChecked;

        public bool IsLFPChecked
        {
            get { return isLFPChecked; }

            set
            {
                if (SetProperty(ref  isLFPChecked, value) && !this.IsLFPChecked)
                {
                    this.LFImgList.Clear();
                }
            }
        }
        private bool isRRGChecked;

        public bool IsRRGChecked
        {
            get { return isRRGChecked; }

            set { SetProperty(ref  isRRGChecked, value); }
        }
        private bool isRRFChecked;

        public bool IsRRFChecked
        {
            get { return isRRFChecked; }

            set { SetProperty(ref  isRRFChecked, value); }
        }
        private bool isRRPChecked;

        public bool IsRRPChecked
        {
            get { return isRRPChecked; }

            set
            {
                if (SetProperty(ref  isRRPChecked, value) && !this.IsRRPChecked)
                {
                    this.RRImgList.Clear();
                }
            }
        }
        private bool isLRGChecked;

        public bool IsLRGChecked
        {
            get { return isLRGChecked; }

            set { SetProperty(ref  isLRGChecked, value); }
        }
        private bool isLRFChecked;

        public bool IsLRFChecked
        {
            get { return isLRFChecked; }

            set { SetProperty(ref  isLRFChecked, value); }
        }
        private bool isLRPChecked;

        public bool IsLRPChecked
        {
            get { return isLRPChecked; }

            set
            {
                if (SetProperty(ref  isLRPChecked, value) && !this.IsLRPChecked)
                {
                    this.LRImgList.Clear();
                }
            }
        }
        private bool isSpareGChecked;

        public bool IsSpareGChecked
        {
            get { return isSpareGChecked; }

            set { SetProperty(ref  isSpareGChecked, value); }
        }
        private bool isSpareFChecked;

        public bool IsSpareFChecked
        {
            get { return isSpareFChecked; }

            set { SetProperty(ref  isSpareFChecked, value); }
        }
        private bool isSparePChecked;

        public bool IsSparePChecked
        {
            get { return isSparePChecked; }

            set
            {
                if (SetProperty(ref  isSparePChecked, value) && !this.IsSparePChecked)
                {
                    this.SpareImgList.Clear();
                }
            }
        }


        public string rFImgPathList;
        public string RFImgPathList
        {
            get { return string.Join("~", RFImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFImgPathList, value); }
        }

        public string lFImgPathList;
        public string LFImgPathList
        {
            get { return string.Join("~", LFImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFImgPathList, value); }
        }

        public string rRImgPathList;
        public string RRImgPathList
        {
            get { return string.Join("~", RRImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRImgPathList, value); }
        }

        public string spareImgPathList;
        public string SpareImgPathList
        {
            get { return string.Join("~", SpareImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref spareImgPathList, value); }
        }

        public string lRImgPathList;
        public string LRImgPathList
        {
            get { return string.Join("~", LRImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRImgPathList, value); }
        }

    }
}
