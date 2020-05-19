using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.CVehicle
{
    public class CChassisBody
    {

        public List<string> DoorsImgList { get; set; }
        public String DoorsComment { get; set; }
        public Boolean IsDoorsDmg { get; set; }
        public String ChassisComment { get; set; }
        public Boolean IsChassisDmg { get; set; }
        public List<string> ChassisImgList { get; set; }
        public String FloorComment { get; set; }
        public Boolean IsFloorDmg { get; set; }
        public List<string> FloorImgList { get; set; }
        public String HeadboardComment { get; set; }
        public Boolean IsHeadboardDmg { get; set; }
        public List<string> HeadboardImgList { get; set; }
        public String DropSideLeftComment { get; set; }
        public Boolean IsDropSideLeftDmg { get; set; }
        public List<string> DropSideLeftImgList { get; set; }
        public String DropSideRightComment { get; set; }
        public Boolean IsDropSideRightDmg { get; set; }
        public List<string> DropSideRightImgList { get; set; }
        public String DropSideFrontComment { get; set; }
        public Boolean IsDropSideFrontDmg { get; set; }
        public List<string> DropSideFrontImgList { get; set; }
        public String DropSideRearComment { get; set; }
        public Boolean IsDropSideRearDmg { get; set; }
        public List<string> DropSideRearImgList { get; set; }
        public String FuelTankComment { get; set; }
        public Boolean IsFuelTankDmg { get; set; }
        public List<string> FuelTankImgList { get; set; }
        public String SpareWheelCarrierComment { get; set; }
        public Boolean IsSpareWheelCarrierDmg { get; set; }
        public List<string> SpareWheelCarrierImgList { get; set; }
        public String UnderRunBumperComment { get; set; }
        public Boolean IsUnderRunBumperDmg { get; set; }
        public List<string> UnderRunBumperImgList { get; set; }
        public String ChevronComment { get; set; }
        public Boolean IsChevronDmg { get; set; }
        public List<string> ChevronImgList { get; set; }
        public String LandingLegsComment { get; set; }
        public Boolean IsLandingLegsDmg { get; set; }
        public List<string> LandingLegsImgList { get; set; }
        public String DoorsImgPathList { get; set; }
        public String ChassisImgPathList { get; set; }
        public String FloorImgPathList { get; set; }
        public String HeadboardImgPathList { get; set; }
        public String DropSideLeftImgPathList { get; set; }
        public String DropSideRightImgPathList { get; set; }
        public String DropSideFrontImgPathList { get; set; }
        public String DropSideRearImgPathList { get; set; }
        public String FuelTankImgPathList { get; set; }
        public String SpareWheelCarrierImgPathList { get; set; }
        public String UnderRunBumperImgPathList { get; set; }
        public String ChevronImgPathList { get; set; }
        public String LandingLegsImgPathList { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }
    }

}

