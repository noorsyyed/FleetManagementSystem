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
    public class CTyres : BaseModel
    {
        public CTyres()
        {
            this.LFImgList = new ObservableCollection<ImageCapture>();
            this.LInnerAxleInImgList = new ObservableCollection<ImageCapture>();
            this.LInnerAxleOutImgList = new ObservableCollection<ImageCapture>();
            this.LRInnerImgList = new ObservableCollection<ImageCapture>();
            this.SpareImgList = new ObservableCollection<ImageCapture>();
            this.RROuterImgList = new ObservableCollection<ImageCapture>();
            this.RFImgList = new ObservableCollection<ImageCapture>();
            this.RRInnerImgList = new ObservableCollection<ImageCapture>();
            this.RInnerAxleInImgList = new ObservableCollection<ImageCapture>();
            this.LROuterImgList = new ObservableCollection<ImageCapture>();
            this.RInnerAxleOutImgList = new ObservableCollection<ImageCapture>();
        }


        public async override System.Threading.Tasks.Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CTyres>(x => x.VehicleInsRecID == vehicleInsRecID);
        }
        private string lFComment;

        public string LFComment
        {
            get { return lFComment; }

            set { SetProperty(ref  lFComment, value); }
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
        private bool hasGLF;

        public bool HasGLF
        {
            get { return hasGLF; }

            set { SetProperty(ref  hasGLF, value); }
        }
        private bool hasFLF;

        public bool HasFLF
        {
            get { return hasFLF; }

            set { SetProperty(ref  hasFLF, value); }
        }
        private bool hasPLF;

        public bool HasPLF
        {
            get { return hasPLF; }

            set
            {
                if (SetProperty(ref  hasPLF, value) && !this.HasPLF)
                {
                    this.LFImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> lFImgList;

        [Ignore, DamageSnapshotRequired("LF snapshot(s) required", "HasPLF")]
        public ObservableCollection<ImageCapture> LFImgList
        {
            get { return lFImgList; }

            set { SetProperty(ref  lFImgList, value); }
        }


        private string lRInnerComment;

        public string LRInnerComment
        {
            get { return lRInnerComment; }

            set { SetProperty(ref  lRInnerComment, value); }
        }
        private string lRInnerTreadDepth;

        public string LRInnerTreadDepth
        {
            get { return lRInnerTreadDepth; }

            set { SetProperty(ref  lRInnerTreadDepth, value); }
        }
        private string lRInnerMake;

        public string LRInnerMake
        {
            get { return lRInnerMake; }

            set { SetProperty(ref  lRInnerMake, value); }
        }
        private bool hasGLRInner;

        public bool HasGLRInner
        {
            get { return hasGLRInner; }

            set { SetProperty(ref  hasGLRInner, value); }
        }
        private bool hasFLRInner;

        public bool HasFLRInner
        {
            get { return hasFLRInner; }

            set { SetProperty(ref  hasFLRInner, value); }
        }
        private bool hasPLRInner;

        public bool HasPLRInner
        {
            get { return hasPLRInner; }

            set
            {
                if (SetProperty(ref  hasPLRInner, value) && !this.HasPLRInner)
                {
                    this.LRInnerImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> lRInnerImgList;

        [Ignore, DamageSnapshotRequired("LR Inner snapshot(s) required", "HasPLRInner")]
        public ObservableCollection<ImageCapture> LRInnerImgList
        {
            get { return lRInnerImgList; }

            set { SetProperty(ref  lRInnerImgList, value); }
        }


        private string lROuterComment;

        public string LROuterComment
        {
            get { return lROuterComment; }

            set { SetProperty(ref  lROuterComment, value); }
        }
        private string lROuterTreadDepth;

        public string LROuterTreadDepth
        {
            get { return lROuterTreadDepth; }

            set { SetProperty(ref  lROuterTreadDepth, value); }
        }
        private string lROuterMake;

        public string LROuterMake
        {
            get { return lROuterMake; }

            set { SetProperty(ref  lROuterMake, value); }
        }
        private bool hasGLROuter;

        public bool HasGLROuter
        {
            get { return hasGLROuter; }

            set { SetProperty(ref  hasGLROuter, value); }
        }
        private bool hasFLROuter;

        public bool HasFLROuter
        {
            get { return hasFLROuter; }

            set { SetProperty(ref  hasFLROuter, value); }
        }
        private bool hasPLROuter;

        public bool HasPLROuter
        {
            get { return hasPLROuter; }

            set
            {
                if (SetProperty(ref  hasPLROuter, value) && !this.HasPLROuter)
                {
                    this.LROuterImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> lROuterImgList;

        [Ignore, DamageSnapshotRequired("LR Outer snapshot(s) required", "HasPLROuter")]
        public ObservableCollection<ImageCapture> LROuterImgList
        {
            get { return lROuterImgList; }

            set { SetProperty(ref  lROuterImgList, value); }
        }

        private string lInnerAxleInComment;

        public string LInnerAxleInComment
        {
            get { return lInnerAxleInComment; }

            set { SetProperty(ref  lInnerAxleInComment, value); }
        }
        private string lInnerAxleInTreadDepth;

        public string LInnerAxleInTreadDepth
        {
            get { return lInnerAxleInTreadDepth; }

            set { SetProperty(ref  lInnerAxleInTreadDepth, value); }
        }
        private string lInnerAxleInMake;

        public string LInnerAxleInMake
        {
            get { return lInnerAxleInMake; }

            set { SetProperty(ref  lInnerAxleInMake, value); }
        }
        private bool hasGLInnerAxleIn;

        public bool HasGLInnerAxleIn
        {
            get { return hasGLInnerAxleIn; }

            set { SetProperty(ref  hasGLInnerAxleIn, value); }
        }
        private bool hasFLInnerAxleIn;

        public bool HasFLInnerAxleIn
        {
            get { return hasFLInnerAxleIn; }

            set { SetProperty(ref  hasFLInnerAxleIn, value); }
        }
        private bool hasPLInnerAxleIn;

        public bool HasPLInnerAxleIn
        {
            get { return hasPLInnerAxleIn; }

            set
            {
                if (SetProperty(ref  hasPLInnerAxleIn, value) && !this.HasPLInnerAxleIn)
                {
                    this.LInnerAxleInImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> lInnerAxleInImgList;

        [Ignore, DamageSnapshotRequired("L Inner Axle In snapshot(s) required", "HasPLInnerAxleIn")]
        public ObservableCollection<ImageCapture> LInnerAxleInImgList
        {
            get { return lInnerAxleInImgList; }

            set { SetProperty(ref  lInnerAxleInImgList, value); }
        }

        private string lInnerAxleOutComment;

        public string LInnerAxleOutComment
        {
            get { return lInnerAxleOutComment; }

            set { SetProperty(ref  lInnerAxleOutComment, value); }
        }
        private string lInnerAxleOutTreadDepth;

        public string LInnerAxleOutTreadDepth
        {
            get { return lInnerAxleOutTreadDepth; }

            set { SetProperty(ref  lInnerAxleOutTreadDepth, value); }
        }
        private string lInnerAxleOutMake;

        public string LInnerAxleOutMake
        {
            get { return lInnerAxleOutMake; }

            set { SetProperty(ref  lInnerAxleOutMake, value); }
        }
        private bool hasGLInnerAxleOut;

        public bool HasGLInnerAxleOut
        {
            get { return hasGLInnerAxleOut; }

            set { SetProperty(ref  hasGLInnerAxleOut, value); }
        }
        private bool hasFLInnerAxleOut;

        public bool HasFLInnerAxleOut
        {
            get { return hasFLInnerAxleOut; }

            set { SetProperty(ref  hasFLInnerAxleOut, value); }
        }
        private bool hasPLInnerAxleOut;

        public bool HasPLInnerAxleOut
        {
            get { return hasPLInnerAxleOut; }

            set
            {
                if (SetProperty(ref  hasPLInnerAxleOut, value) && !this.HasPLInnerAxleOut)
                {
                    this.LInnerAxleOutImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> lInnerAxleOutImgList;

        [Ignore, DamageSnapshotRequired("L Inner Axle Out snapshot(s) required", "HasPLInnerAxleOut")]
        public ObservableCollection<ImageCapture> LInnerAxleOutImgList
        {
            get { return lInnerAxleOutImgList; }

            set { SetProperty(ref  lInnerAxleOutImgList, value); }
        }

        private string rFComment;

        public string RFComment
        {
            get { return rFComment; }

            set { SetProperty(ref  rFComment, value); }
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
        private bool hasGRF;

        public bool HasGRF
        {
            get { return hasGRF; }

            set { SetProperty(ref  hasGRF, value); }
        }
        private bool hasFRF;

        public bool HasFRF
        {
            get { return hasFRF; }

            set { SetProperty(ref  hasFRF, value); }
        }
        private bool hasPRF;

        public bool HasPRF
        {
            get { return hasPRF; }

            set
            {
                if (SetProperty(ref  hasPRF, value) && !this.HasPRF)
                {
                    this.RFImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> rFImgList;

        [Ignore, DamageSnapshotRequired("RF snapshot(s) required", "HasPRF")]
        public ObservableCollection<ImageCapture> RFImgList
        {
            get { return rFImgList; }

            set { SetProperty(ref  rFImgList, value); }
        }

        private string rRInnerComment;

        public string RRInnerComment
        {
            get { return rRInnerComment; }

            set { SetProperty(ref  rRInnerComment, value); }
        }
        private string rRInnerTreadDepth;

        public string RRInnerTreadDepth
        {
            get { return rRInnerTreadDepth; }

            set { SetProperty(ref  rRInnerTreadDepth, value); }
        }
        private string rRInnerMake;

        public string RRInnerMake
        {
            get { return rRInnerMake; }

            set { SetProperty(ref  rRInnerMake, value); }
        }
        private bool hasGRRInner;

        public bool HasGRRInner
        {
            get { return hasGRRInner; }

            set { SetProperty(ref  hasGRRInner, value); }
        }
        private bool hasFRRInner;

        public bool HasFRRInner
        {
            get { return hasFRRInner; }

            set { SetProperty(ref  hasFRRInner, value); }
        }
        private bool hasPRRInner;

        public bool HasPRRInner
        {
            get { return hasPRRInner; }

            set
            {
                if (SetProperty(ref  hasPRRInner, value) && !this.HasPRRInner)
                {
                    this.RRInnerImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> rRInnerImgList;

        [Ignore, DamageSnapshotRequired("RR Inner snapshot(s) required", "HasPRRInner")]
        public ObservableCollection<ImageCapture> RRInnerImgList
        {
            get { return rRInnerImgList; }

            set { SetProperty(ref  rRInnerImgList, value); }
        }

        private string rROuterComment;

        public string RROuterComment
        {
            get { return rROuterComment; }

            set { SetProperty(ref  rROuterComment, value); }
        }
        private string rROuterTreadDepth;

        public string RROuterTreadDepth
        {
            get { return rROuterTreadDepth; }

            set { SetProperty(ref  rROuterTreadDepth, value); }
        }
        private string rROuterMake;

        public string RROuterMake
        {
            get { return rROuterMake; }

            set { SetProperty(ref  rROuterMake, value); }
        }
        private bool hasGRROuter;

        public bool HasGRROuter
        {
            get { return hasGRROuter; }

            set { SetProperty(ref  hasGRROuter, value); }
        }
        private bool hasFRROuter;

        public bool HasFRROuter
        {
            get { return hasFRROuter; }

            set { SetProperty(ref  hasFRROuter, value); }
        }
        private bool hasPRROuter;

        public bool HasPRROuter
        {
            get { return hasPRROuter; }

            set
            {
                if (SetProperty(ref  hasPRROuter, value) && !this.HasPRROuter)
                {
                    this.RROuterImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> rROuterImgList;

        [Ignore, DamageSnapshotRequired("RR Outer snapshot(s) required", "HasPRROuter")]
        public ObservableCollection<ImageCapture> RROuterImgList
        {
            get { return rROuterImgList; }

            set { SetProperty(ref  rROuterImgList, value); }
        }

        private string rInnerAxleInComment;

        public string RInnerAxleInComment
        {
            get { return rInnerAxleInComment; }

            set { SetProperty(ref  rInnerAxleInComment, value); }
        }
        private string rInnerAxleInTreadDepth;

        public string RInnerAxleInTreadDepth
        {
            get { return rInnerAxleInTreadDepth; }

            set { SetProperty(ref  rInnerAxleInTreadDepth, value); }
        }
        private string rInnerAxleInMake;

        public string RInnerAxleInMake
        {
            get { return rInnerAxleInMake; }

            set { SetProperty(ref  rInnerAxleInMake, value); }
        }
        private bool hasGRInnerAxleIn;

        public bool HasGRInnerAxleIn
        {
            get { return hasGRInnerAxleIn; }

            set { SetProperty(ref  hasGRInnerAxleIn, value); }
        }
        private bool hasFRInnerAxleIn;

        public bool HasFRInnerAxleIn
        {
            get { return hasFRInnerAxleIn; }

            set { SetProperty(ref  hasFRInnerAxleIn, value); }
        }
        private bool hasPRInnerAxleIn;

        public bool HasPRInnerAxleIn
        {
            get { return hasPRInnerAxleIn; }

            set
            {
                if (SetProperty(ref  hasPRInnerAxleIn, value) && !this.HasPRInnerAxleIn)
                {
                    this.RInnerAxleInImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> rInnerAxleInImgList;

        [Ignore, DamageSnapshotRequired("R Inner Axle In snapshot(s) required", "HasPRInnerAxleIn")]
        public ObservableCollection<ImageCapture> RInnerAxleInImgList
        {
            get { return rInnerAxleInImgList; }

            set { SetProperty(ref  rInnerAxleInImgList, value); }
        }

        private string rInnerAxleOutComment;

        public string RInnerAxleOutComment
        {
            get { return rInnerAxleOutComment; }

            set { SetProperty(ref  rInnerAxleOutComment, value); }
        }
        private string rInnerAxleOutTreadDepth;

        public string RInnerAxleOutTreadDepth
        {
            get { return rInnerAxleOutTreadDepth; }

            set { SetProperty(ref  rInnerAxleOutTreadDepth, value); }
        }
        private string rInnerAxleOutMake;

        public string RInnerAxleOutMake
        {
            get { return rInnerAxleOutMake; }

            set { SetProperty(ref  rInnerAxleOutMake, value); }
        }
        private bool hasGRInnerAxleOut;

        public bool HasGRInnerAxleOut
        {
            get { return hasGRInnerAxleOut; }

            set { SetProperty(ref  hasGRInnerAxleOut, value); }
        }
        private bool hasFRInnerAxleOut;

        public bool HasFRInnerAxleOut
        {
            get { return hasFRInnerAxleOut; }

            set { SetProperty(ref  hasFRInnerAxleOut, value); }
        }
        private bool hasPRInnerAxleOut;

        public bool HasPRInnerAxleOut
        {
            get { return hasPRInnerAxleOut; }

            set
            {
                if (SetProperty(ref  hasPRInnerAxleOut, value) && !this.HasPRInnerAxleOut)
                {
                    this.RInnerAxleOutImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> rInnerAxleOutImgList;

        [Ignore, DamageSnapshotRequired("R Inner Axle Out snapshot(s) required", "HasPRInnerAxleOut")]
        public ObservableCollection<ImageCapture> RInnerAxleOutImgList
        {
            get { return rInnerAxleOutImgList; }

            set { SetProperty(ref  rInnerAxleOutImgList, value); }
        }


        private string spareComment;

        public string SpareComment
        {
            get { return spareComment; }

            set { SetProperty(ref  spareComment, value); }
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
        private bool hasGSpare;

        public bool HasGSpare
        {
            get { return hasGSpare; }

            set { SetProperty(ref  hasGSpare, value); }
        }
        private bool hasFSpare;

        public bool HasFSpare
        {
            get { return hasFSpare; }

            set { SetProperty(ref  hasFSpare, value); }
        }
        private bool hasPSpare;

        public bool HasPSpare
        {
            get { return hasPSpare; }

            set
            {
                if (SetProperty(ref  hasPSpare, value) && !this.HasPSpare)
                {
                    this.SpareImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> spareImgList;

        [Ignore, DamageSnapshotRequired("Spare snapshot(s) required", "HasPSpare")]
        public ObservableCollection<ImageCapture> SpareImgList
        {
            get { return spareImgList; }

            set { SetProperty(ref  spareImgList, value); }
        }

        public string lFImgPathList;
        public string LFImgPathList
        {
            get { return string.Join("~", LFImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFImgPathList, value); }
        }

        public string lInnerAxleInImgPathList;
        public string LInnerAxleInImgPathList
        {
            get { return string.Join("~", LInnerAxleInImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lInnerAxleInImgPathList, value); }
        }

        public string lInnerAxleOutImgPathList;
        public string LInnerAxleOutImgPathList
        {
            get { return string.Join("~", LInnerAxleOutImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lInnerAxleOutImgPathList, value); }
        }

        public string lRInnerImgPathList;
        public string LRInnerImgPathList
        {
            get { return string.Join("~", LRInnerImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRInnerImgPathList, value); }
        }

        public string spareImgPathList;
        public string SpareImgPathList
        {
            get { return string.Join("~", SpareImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref spareImgPathList, value); }
        }

        public string rROuterImgPathList;
        public string RROuterImgPathList
        {
            get { return string.Join("~", RROuterImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rROuterImgPathList, value); }
        }

        public string rFImgPathList;
        public string RFImgPathList
        {
            get { return string.Join("~", RFImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFImgPathList, value); }
        }

        public string rRInnerImgPathList;
        public string RRInnerImgPathList
        {
            get { return string.Join("~", RRInnerImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRInnerImgPathList, value); }
        }

        public string rInnerAxleInImgPathList;
        public string RInnerAxleInImgPathList
        {
            get { return string.Join("~", RInnerAxleInImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rInnerAxleInImgPathList, value); }
        }

        public string lROuterImgPathList;
        public string LROuterImgPathList
        {
            get { return string.Join("~", LROuterImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lROuterImgPathList, value); }
        }

        public string rInnerAxleOutImgPathList;
        public string RInnerAxleOutImgPathList
        {
            get { return string.Join("~", RInnerAxleOutImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rInnerAxleOutImgPathList, value); }
        }
    }
}
