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
    public class CGlass : BaseModel
    {
        public CGlass()
        {
            this.WindscreenImgList = new ObservableCollection<ImageCapture>();
            this.RearGlassImgList = new ObservableCollection<ImageCapture>();
            this.SideGlassImgList = new ObservableCollection<ImageCapture>();
            this.HeadLightsImgList = new ObservableCollection<ImageCapture>();
            this.TailLightsImgList = new ObservableCollection<ImageCapture>();
            this.InductorLensesImgList = new ObservableCollection<ImageCapture>();
            this.ExtRearViewMirrorImgList = new ObservableCollection<ImageCapture>();
        }

        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CGlass>(x => x.VehicleInsRecID == vehicleInsRecID);
        }

        private string windscreenComment;

        public string WindscreenComment
        {
            get { return windscreenComment; }

            set { SetProperty(ref  windscreenComment, value); }
        }
        private bool isWindscreenDmg;

        public bool IsWindscreenDmg
        {
            get { return isWindscreenDmg; }

            set
            {
                if (SetProperty(ref  isWindscreenDmg, value) && !this.IsWindscreenDmg)
                {
                    this.WindscreenImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> windscreenImgList;

        [Ignore, DamageSnapshotRequired("Wind screen snapshot(s) required", "IsWindscreenDmg")]
        public ObservableCollection<ImageCapture> WindscreenImgList
        {
            get { return windscreenImgList; }

            set { SetProperty(ref  windscreenImgList, value); }
        }

        private string rearGlassComment;

        public string RearGlassComment
        {
            get { return rearGlassComment; }

            set { SetProperty(ref  rearGlassComment, value); }
        }
        private bool isRearGlassDmg;

        public bool IsRearGlassDmg
        {
            get { return isRearGlassDmg; }

            set
            {
                if (SetProperty(ref  isRearGlassDmg, value) && !this.IsRearGlassDmg)
                {
                    this.RearGlassImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> rearGlassImgList;

        [Ignore, DamageSnapshotRequired("Rear glass snapshot(s) required", "IsRearGlassDmg")]
        public ObservableCollection<ImageCapture> RearGlassImgList
        {
            get { return rearGlassImgList; }

            set { SetProperty(ref  rearGlassImgList, value); }
        }

        private string sideGlassComment;

        public string SideGlassComment
        {
            get { return sideGlassComment; }

            set { SetProperty(ref  sideGlassComment, value); }
        }
        private bool isSideGlassDmg;

        public bool IsSideGlassDmg
        {
            get { return isSideGlassDmg; }

            set
            {
                if (SetProperty(ref  isSideGlassDmg, value) && !this.IsSideGlassDmg)
                {
                    this.SideGlassImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> sideGlassImgList;

        [Ignore, DamageSnapshotRequired("Side glass snapshot(s) required", "IsSideGlassDmg")]
        public ObservableCollection<ImageCapture> SideGlassImgList
        {
            get { return sideGlassImgList; }

            set { SetProperty(ref  sideGlassImgList, value); }
        }

        private string headLightsComment;

        public string HeadLightsComment
        {
            get { return headLightsComment; }

            set { SetProperty(ref  headLightsComment, value); }
        }
        private bool isHeadLightsDmg;

        public bool IsHeadLightsDmg
        {
            get { return isHeadLightsDmg; }

            set
            {
                if (SetProperty(ref  isHeadLightsDmg, value) && !this.IsHeadLightsDmg)
                {
                    this.HeadLightsImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> headLightsImgList;

        [Ignore, DamageSnapshotRequired("Head lights snapshot(s) required", "IsHeadLightsDmg")]
        public ObservableCollection<ImageCapture> HeadLightsImgList
        {
            get { return headLightsImgList; }

            set { SetProperty(ref  headLightsImgList, value); }
        }


        private string tailLightsComment;

        public string TailLightsComment
        {
            get { return tailLightsComment; }

            set { SetProperty(ref  tailLightsComment, value); }
        }
        private bool isTailLightsDmg;

        public bool IsTailLightsDmg
        {
            get { return isTailLightsDmg; }

            set
            {
                if (SetProperty(ref  isTailLightsDmg, value) && !this.IsTailLightsDmg)
                {
                    this.TailLightsImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> tailLightsImgList;

        [Ignore, DamageSnapshotRequired("Tail lights snapshot(s) required", "IsTailLightsDmg")]
        public ObservableCollection<ImageCapture> TailLightsImgList
        {
            get { return tailLightsImgList; }

            set { SetProperty(ref  tailLightsImgList, value); }
        }

        private string inductorLensesComment;

        public string InductorLensesComment
        {
            get { return inductorLensesComment; }

            set { SetProperty(ref  inductorLensesComment, value); }
        }
        private bool isInductorLensesDmg;

        public bool IsInductorLensesDmg
        {
            get { return isInductorLensesDmg; }

            set
            {
                if (SetProperty(ref  isInductorLensesDmg, value) && !this.IsInductorLensesDmg)
                {
                    this.InductorLensesImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> inductorLensesImgList;

        [Ignore, DamageSnapshotRequired("Inductor lenses snapshot(s) required", "IsInductorLensesDmg")]
        public ObservableCollection<ImageCapture> InductorLensesImgList
        {
            get { return inductorLensesImgList; }

            set { SetProperty(ref  inductorLensesImgList, value); }
        }

        private string extRearViewMirrorComment;

        public string ExtRearViewMirrorComment
        {
            get { return extRearViewMirrorComment; }

            set { SetProperty(ref  extRearViewMirrorComment, value); }
        }
        private bool isExtRearViewMirrorDmg;

        public bool IsExtRearViewMirrorDmg
        {
            get { return isExtRearViewMirrorDmg; }

            set
            {
                if (SetProperty(ref  isExtRearViewMirrorDmg, value) && !this.IsExtRearViewMirrorDmg)
                {
                    this.ExtRearViewMirrorImgList.Clear();
                }
            }
        }
        private ObservableCollection<ImageCapture> extRearViewMirrorImgList;

        [Ignore, DamageSnapshotRequired("Ext rear view mirror snapshot(s) required", "IsExtRearViewMirrorDmg")]
        public ObservableCollection<ImageCapture> ExtRearViewMirrorImgList
        {
            get { return extRearViewMirrorImgList; }

            set { SetProperty(ref  extRearViewMirrorImgList, value); }
        }


        public string windscreenImgPathList;
        public string WindscreenImgPathList
        {
            get { return string.Join("~", WindscreenImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref windscreenImgPathList, value); }
        }

        public string rearGlassImgPathList;
        public string RearGlassImgPathList
        {
            get { return string.Join("~", RearGlassImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rearGlassImgPathList, value); }
        }

        public string sideGlassImgPathList;
        public string SideGlassImgPathList
        {
            get { return string.Join("~", SideGlassImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref sideGlassImgPathList, value); }
        }

        public string headLightsImgPathList;
        public string HeadLightsImgPathList
        {
            get { return string.Join("~", HeadLightsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref headLightsImgPathList, value); }
        }

        public string tailLightsImgPathList;
        public string TailLightsImgPathList
        {
            get { return string.Join("~", TailLightsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref tailLightsImgPathList, value); }
        }

        public string inductorLensesImgPathList;
        public string InductorLensesImgPathList
        {
            get { return string.Join("~", InductorLensesImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref inductorLensesImgPathList, value); }
        }

        public string extRearViewMirrorImgPathList;
        public string ExtRearViewMirrorImgPathList
        {
            get { return string.Join("~", ExtRearViewMirrorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref extRearViewMirrorImgPathList, value); }
        }

    }
}
