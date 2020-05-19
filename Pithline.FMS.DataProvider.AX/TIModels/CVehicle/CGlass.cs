using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.CVehicle
{
    public class CGlass
    {

        public List<string> WindscreenImgList { get; set; }
        public String WindscreenComment { get; set; }
        public Boolean IsWindscreenDmg { get; set; }
        public String RearGlassComment { get; set; }
        public Boolean IsRearGlassDmg { get; set; }
        public List<string> RearGlassImgList { get; set; }
        public String SideGlassComment { get; set; }
        public Boolean IsSideGlassDmg { get; set; }
        public List<string> SideGlassImgList { get; set; }
        public String HeadLightsComment { get; set; }
        public Boolean IsHeadLightsDmg { get; set; }
        public List<string> HeadLightsImgList { get; set; }
        public String TailLightsComment { get; set; }
        public Boolean IsTailLightsDmg { get; set; }
        public List<string> TailLightsImgList { get; set; }
        public String InductorLensesComment { get; set; }
        public Boolean IsInductorLensesDmg { get; set; }
        public List<string> InductorLensesImgList { get; set; }
        public String ExtRearViewMirrorComment { get; set; }
        public Boolean IsExtRearViewMirrorDmg { get; set; }
        public List<string> ExtRearViewMirrorImgList { get; set; }
        public String WindscreenImgPathList { get; set; }
        public String RearGlassImgPathList { get; set; }
        public String SideGlassImgPathList { get; set; }
        public String HeadLightsImgPathList { get; set; }
        public String TailLightsImgPathList { get; set; }
        public String InductorLensesImgPathList { get; set; }
        public String ExtRearViewMirrorImgPathList { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

