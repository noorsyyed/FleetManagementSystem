using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.PVehicle
{
    public class PGlass
    {

        public List<string> GVWindscreenImgList { get; set; }
        public List<string> GVRearGlassImgList { get; set; }
        public List<string> GVSideGlassImgList { get; set; }
        public List<string> GVHeadLightsImgList { get; set; }
        public List<string> GVTailLightsImgList { get; set; }
        public List<string> GVInductorLensesImgList { get; set; }
        public List<string> GVExtRearViewMirrorImgList { get; set; }
        public String GVWindscreenComment { get; set; }
        public String GVRearGlassComment { get; set; }
        public String GVSideGlassComment { get; set; }
        public String GVHeadLightsComment { get; set; }
        public String GVTailLightsComment { get; set; }
        public String GVInductorLensesComment { get; set; }
        public String GVExtRearViewMirrorComment { get; set; }
        public Boolean IsWindscreen { get; set; }
        public Boolean IsRearGlass { get; set; }
        public Boolean IsSideGlass { get; set; }
        public Boolean IsHeadLights { get; set; }
        public Boolean IsTailLights { get; set; }
        public Boolean IsInductorLenses { get; set; }
        public Boolean IsExtRearViewMirror { get; set; }
        public String GVWindscreenImgPathList { get; set; }
        public String GVRearGlassImgPathList { get; set; }
        public String GVSideGlassImgPathList { get; set; }
        public String GVHeadLightsImgPathList { get; set; }
        public String GVTailLightsImgPathList { get; set; }
        public String GVInductorLensesImgPathList { get; set; }
        public String GVExtRearViewMirrorImgPathList { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

