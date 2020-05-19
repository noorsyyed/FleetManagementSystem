using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.CVehicle
{
    public class CAccessories
    {

        public List<string> ServiceBlockImgList { get; set; }
        public String ServiceBlockComment { get; set; }
        public Boolean IsServiceBlockDmg { get; set; }
        public String ToolsComment { get; set; }
        public Boolean IsToolsDmg { get; set; }
        public List<string> ToolsImgList { get; set; }
        public String JackComment { get; set; }
        public Boolean IsJackDmg { get; set; }
        public List<string> JackImgList { get; set; }
        public String BullBarComment { get; set; }
        public Boolean IsBullBarDmg { get; set; }
        public List<string> BullBarImgList { get; set; }
        public String TrackingDeviceComment { get; set; }
        public Boolean IsTrackingDeviceDmg { get; set; }
        public List<string> TrackingDeviceImgList { get; set; }
        public String EngineProtectionUnitComment { get; set; }
        public Boolean IsEngineProtectionUnitDmg { get; set; }
        public List<string> EngineProtectionUnitImgList { get; set; }
        public String DecalSignWritingComment { get; set; }
        public Boolean IsDecalSignWritingDmg { get; set; }
        public List<string> DecalSignWritingImgList { get; set; }
        public String ReflectiveTapeComment { get; set; }
        public Boolean IsReflectiveTapeDmg { get; set; }
        public List<string> ReflectiveTapeImgList { get; set; }
        public String ServiceBlockImgPathList { get; set; }
        public String ToolsImgPathList { get; set; }
        public String JackImgPathList { get; set; }
        public String BullBarImgPathList { get; set; }
        public String TrackingDeviceImgPathList { get; set; }
        public String EngineProtectionUnitImgPathList { get; set; }
        public String DecalSignWritingImgPathList { get; set; }
        public String ReflectiveTapeImgPathList { get; set; }
        public Boolean HasServiceBlockDmg { get; set; }
        public Boolean HasToolsDmg { get; set; }
        public Boolean HasJackDmg { get; set; }
        public Boolean HasBullBarDmg { get; set; }
        public Boolean HasTrackingDeviceDmg { get; set; }
        public Boolean HasEngineProtectionUnitDmg { get; set; }
        public Boolean HasDecalSignWritingDmg { get; set; }
        public Boolean HasReflectiveTapeDmg { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

