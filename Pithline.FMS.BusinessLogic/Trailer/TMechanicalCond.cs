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

namespace Pithline.FMS.BusinessLogic
{
    public class TMechanicalCond : BaseModel
    {
        public TMechanicalCond()
        {
            this.RearSuspImgList = new ObservableCollection<ImageCapture>();
            this.FrontSuspImgList = new ObservableCollection<ImageCapture>();
            this.HandBrakeImgList = new ObservableCollection<ImageCapture>();
            this.FootBrakeImgList = new ObservableCollection<ImageCapture>();
        }

        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<TMechanicalCond>(x => x.VehicleInsRecID == vehicleInsRecID);
        }


        private string frontSuspComment;

        public string FrontSuspComment
        {
            get { return frontSuspComment; }

            set { SetProperty(ref  frontSuspComment, value); }
        }
        private bool isFrontSuspDmg;

        public bool IsFrontSuspDmg
        {
            get { return isFrontSuspDmg; }

            set
            {
                if (SetProperty(ref  isFrontSuspDmg, value) && !this.IsFrontSuspDmg)
                {
                    this.FrontSuspImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> frontSuspImgList;

        [Ignore, DamageSnapshotRequired("Front susp snapshot(s) required", "IsFrontSuspDmg")]
        public ObservableCollection<ImageCapture> FrontSuspImgList
        {
            get { return frontSuspImgList; }

            set { SetProperty(ref  frontSuspImgList, value); }
        }

        private string rearSuspComment;

        public string RearSuspComment
        {
            get { return rearSuspComment; }

            set { SetProperty(ref  rearSuspComment, value); }
        }
        private bool isRearSuspDmg;

        public bool IsRearSuspDmg
        {
            get { return isRearSuspDmg; }

            set
            {
                if (SetProperty(ref  isRearSuspDmg, value) && !this.IsRearSuspDmg)
                {
                    this.RearSuspImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> rearSuspImgList;

        [Ignore, DamageSnapshotRequired("Rear susp snapshot(s) required", "IsRearSuspDmg")]
        public ObservableCollection<ImageCapture> RearSuspImgList
        {
            get { return rearSuspImgList; }

            set { SetProperty(ref  rearSuspImgList, value); }
        }

        private string handBrakeComment;

        public string HandBrakeComment
        {
            get { return handBrakeComment; }

            set { SetProperty(ref  handBrakeComment, value); }
        }
        private bool isHandBrakeDmg;

        public bool IsHandBrakeDmg
        {
            get { return isHandBrakeDmg; }

            set
            {
                if (SetProperty(ref  isHandBrakeDmg, value) && !this.IsHandBrakeDmg)
                {
                    this.HandBrakeImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> handBrakeImgList;

        [Ignore, DamageSnapshotRequired("Hand brake snapshot(s) required", "IsHandBrakeDmg")]
        public ObservableCollection<ImageCapture> HandBrakeImgList
        {
            get { return handBrakeImgList; }

            set { SetProperty(ref  handBrakeImgList, value); }
        }

        private string footBrakeComment;

        public string FootBrakeComment
        {
            get { return footBrakeComment; }

            set { SetProperty(ref  footBrakeComment, value); }
        }
        private bool isFootBrakeDmg;

        public bool IsFootBrakeDmg
        {
            get { return isFootBrakeDmg; }

            set
            {
                if (SetProperty(ref  isFootBrakeDmg, value) && !this.IsFootBrakeDmg)
                {
                    this.FootBrakeImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> footBrakeImgList;

        [Ignore, DamageSnapshotRequired("Foot brake snapshot(s) required", "IsFootBrakeDmg")]
        public ObservableCollection<ImageCapture> FootBrakeImgList
        {
            get { return footBrakeImgList; }

            set { SetProperty(ref  footBrakeImgList, value); }
        }

        public string rearSuspImgPathList;
        public string RearSuspImgPathList
        {
            get { return string.Join("~", RearSuspImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rearSuspImgPathList, value); }
        }

        public string frontSuspImgPathList;
        public string FrontSuspImgPathList
        {
            get { return string.Join("~", FrontSuspImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref frontSuspImgPathList, value); }
        }

        public string handBrakeImgPathList;
        public string HandBrakeImgPathList
        {
            get { return string.Join("~", HandBrakeImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref handBrakeImgPathList, value); }
        }

        public string footBrakeImgPathList;
        public string FootBrakeImgPathList
        {
            get { return string.Join("~", FootBrakeImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref footBrakeImgPathList, value); }
        }
    }
}
