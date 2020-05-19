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
    public class PGlass : BaseModel
    {
        public PGlass()
        {
            this.GVWindscreenImgList = new ObservableCollection<ImageCapture>();
            this.GVRearGlassImgList = new ObservableCollection<ImageCapture>();
            this.GVHeadLightsImgList = new ObservableCollection<ImageCapture>();
            this.GVSideGlassImgList = new ObservableCollection<ImageCapture>();
            this.GVHeadLightsImgList = new ObservableCollection<ImageCapture>();
            this.GVTailLightsImgList = new ObservableCollection<ImageCapture>();
            this.GVInductorLensesImgList = new ObservableCollection<ImageCapture>();
            this.GVExtRearViewMirrorImgList = new ObservableCollection<ImageCapture>();
            this.ShouldSave = false;
        }

        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<PGlass>(x => x.VehicleInsRecID == vehicleInsRecID);
        }

        private ObservableCollection<ImageCapture> gVWindscreenImgList;
        [Ignore, DamageSnapshotRequired("Windscreen snapshot(s) required", "IsWindscreen")]
        public ObservableCollection<ImageCapture> GVWindscreenImgList
        {
            get { return gVWindscreenImgList; }
            set { SetProperty(ref  gVWindscreenImgList, value); }
        }

        private ObservableCollection<ImageCapture> gVRearGlassImgList;
        [Ignore, DamageSnapshotRequired("Rear glass snapshot(s) required", "IsRearGlass")]
        public ObservableCollection<ImageCapture> GVRearGlassImgList
        {
            get { return gVRearGlassImgList; }
            set { SetProperty(ref  gVRearGlassImgList, value); }
        }

        private ObservableCollection<ImageCapture> gVSideGlassImgList;
        [Ignore, DamageSnapshotRequired("Side glass snapshot(s) required", "IsSideGlass")]
        public ObservableCollection<ImageCapture> GVSideGlassImgList
        {
            get { return gVSideGlassImgList; }
            set { SetProperty(ref  gVSideGlassImgList, value); }
        }
        private ObservableCollection<ImageCapture> gVHeadLightsImgList;
        [Ignore, DamageSnapshotRequired("Head lights snapshot(s) required", "IsHeadLights")]
        public ObservableCollection<ImageCapture> GVHeadLightsImgList
        {
            get { return gVHeadLightsImgList; }
            set { SetProperty(ref  gVHeadLightsImgList, value); }
        }
        private ObservableCollection<ImageCapture> gVTailLightsImgList;
        [Ignore, DamageSnapshotRequired("Tail lights snapshot(s) required", "IsTailLights")]
        public ObservableCollection<ImageCapture> GVTailLightsImgList
        {
            get { return gVTailLightsImgList; }
            set { SetProperty(ref  gVTailLightsImgList, value); }
        }
        private ObservableCollection<ImageCapture> gVInductorLensesImgList;
        [Ignore, DamageSnapshotRequired("Inductor lenses snapshot(s) required", "IsInductorLenses")]
        public ObservableCollection<ImageCapture> GVInductorLensesImgList
        {
            get { return gVInductorLensesImgList; }
            set { SetProperty(ref  gVInductorLensesImgList, value); }
        }
        private ObservableCollection<ImageCapture> gVExtRearViewMirrorImgList;
        [Ignore, DamageSnapshotRequired("Ext rear view mirror snapshot(s) required", "IsExtRearViewMirror")]
        public ObservableCollection<ImageCapture> GVExtRearViewMirrorImgList
        {
            get { return gVExtRearViewMirrorImgList; }
            set { SetProperty(ref  gVExtRearViewMirrorImgList, value); }
        }

        private string gVWindscreenComment;

        public string GVWindscreenComment
        {
            get { return gVWindscreenComment; }

            set { SetProperty(ref  gVWindscreenComment, value); }
        }
        private string gVRearGlassComment;

        public string GVRearGlassComment
        {
            get { return gVRearGlassComment; }

            set
            {
                SetProperty(ref  gVRearGlassComment, value);

            }
        }
        private string gVSideGlassComment;

        public string GVSideGlassComment
        {
            get { return gVSideGlassComment; }

            set { SetProperty(ref  gVSideGlassComment, value); }
        }
        private string gVHeadLightsComment;

        public string GVHeadLightsComment
        {
            get { return gVHeadLightsComment; }

            set { SetProperty(ref  gVHeadLightsComment, value); }
        }
        private string gVTailLightsComment;

        public string GVTailLightsComment
        {
            get { return gVTailLightsComment; }

            set { SetProperty(ref  gVTailLightsComment, value); }
        }
        private string gVInductorLensesComment;

        public string GVInductorLensesComment
        {
            get { return gVInductorLensesComment; }

            set { SetProperty(ref  gVInductorLensesComment, value); }
        }
        private string gVExtRearViewMirrorComment;

        public string GVExtRearViewMirrorComment
        {
            get { return gVExtRearViewMirrorComment; }

            set { SetProperty(ref  gVExtRearViewMirrorComment, value); }
        }


        private bool isWindscreen;

        public bool IsWindscreen
        {
            get { return isWindscreen; }

            set
            {
                if (SetProperty(ref  isWindscreen, value) && !this.IsWindscreen)
                {
                    this.GVWindscreenImgList.Clear();
                }
            }
        }
        private bool isRearGlass;

        public bool IsRearGlass
        {
            get { return isRearGlass; }

            set
            {
                if (SetProperty(ref  isRearGlass, value) && !this.IsRearGlass)
                {
                    this.GVRearGlassImgList.Clear();
                }
            }
        }
        private bool isSideGlass;

        public bool IsSideGlass
        {
            get { return isSideGlass; }

            set
            {
                if (SetProperty(ref  isSideGlass, value) && !this.IsSideGlass)
                {
                    this.GVSideGlassImgList.Clear();
                }
            }
        }
        private bool isHeadLights;

        public bool IsHeadLights
        {
            get { return isHeadLights; }

            set
            {
                if (SetProperty(ref  isHeadLights, value) && !this.IsHeadLights)
                {
                    this.GVHeadLightsImgList.Clear();
                }
            }
        }
        private bool isTailLights;

        public bool IsTailLights
        {
            get { return isTailLights; }

            set
            {
                if (SetProperty(ref  isTailLights, value) && !this.IsTailLights)
                {
                    this.GVTailLightsImgList.Clear();
                }
            }
        }
        private bool isInductorLenses;

        public bool IsInductorLenses
        {
            get { return isInductorLenses; }

            set
            {
                if (SetProperty(ref  isInductorLenses, value) && !this.IsInductorLenses)
                {
                    this.GVInductorLensesImgList.Clear();
                }
            }
        }
        private bool isExtRearViewMirror;

        public bool IsExtRearViewMirror
        {
            get { return isExtRearViewMirror; }

            set
            {
                if (SetProperty(ref  isExtRearViewMirror, value) && !this.IsExtRearViewMirror)
                {
                    this.GVExtRearViewMirrorImgList.Clear();
                }
            }
        }


        public string gVWindscreenImgPathList;
        public string GVWindscreenImgPathList
        {
            get { return string.Join("~", GVWindscreenImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVWindscreenImgPathList, value); }
        }

        public string gVRearGlassImgPathList;
        public string GVRearGlassImgPathList
        {
            get { return string.Join("~", GVRearGlassImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVRearGlassImgPathList, value); }
        }


        public string gVSideGlassImgPathList;
        public string GVSideGlassImgPathList
        {
            get { return string.Join("~", GVSideGlassImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVSideGlassImgPathList, value); }
        }

        public string gVHeadLightsImgPathList;
        public string GVHeadLightsImgPathList
        {
            get { return string.Join("~", GVHeadLightsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVHeadLightsImgPathList, value); }
        }

        public string gVTailLightsImgPathList;
        public string GVTailLightsImgPathList
        {
            get { return string.Join("~", GVTailLightsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVTailLightsImgPathList, value); }
        }

        public string gVInductorLensesImgPathList;
        public string GVInductorLensesImgPathList
        {
            get { return string.Join("~", GVInductorLensesImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVInductorLensesImgPathList, value); }
        }

        public string gVExtRearViewMirrorImgPathList;
        public string GVExtRearViewMirrorImgPathList
        {
            get { return string.Join("~", GVExtRearViewMirrorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVExtRearViewMirrorImgPathList, value); }
        }

    }
}
