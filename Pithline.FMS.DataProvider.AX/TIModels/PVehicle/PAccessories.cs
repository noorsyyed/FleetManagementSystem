using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.PVehicle
{
    public class PAccessories
    {

        public List<string> RadioImgList { get; set; }
        public Boolean HasRadio { get; set; }
        public Boolean IsRadioDmg { get; set; }
        public String RadioComment { get; set; }
        public Boolean HasCDShuffle { get; set; }
        public Boolean IsCDShuffleDmg { get; set; }
        public String CDShuffleComment { get; set; }
        public List<string> CDShuffleImgList { get; set; }
        public Boolean HasNavigation { get; set; }
        public Boolean IsNavigationDmg { get; set; }
        public String NavigationComment { get; set; }
        public List<string> NavigationImgList { get; set; }
        public Boolean HasAircon { get; set; }
        public Boolean IsAirconDmg { get; set; }
        public String AirconComment { get; set; }
        public List<string> AirconImgList { get; set; }
        public Boolean HasAlarm { get; set; }
        public Boolean IsAlarmDmg { get; set; }
        public String AlarmComment { get; set; }
        public List<string> AlarmImgList { get; set; }
        public Boolean HasKey { get; set; }
        public Boolean IsKeyDmg { get; set; }
        public String KeyComment { get; set; }
        public List<string> KeyImgList { get; set; }
        public Boolean HasSpareKeys { get; set; }
        public Boolean IsSpareKeysDmg { get; set; }
        public String SpareKeysComment { get; set; }
        public List<string> SpareKeysImgList { get; set; }
        public Boolean HasServicesBook { get; set; }
        public Boolean IsServicesBookDmg { get; set; }
        public String ServicesBookComment { get; set; }
        public List<string> ServicesBookImgList { get; set; }
        public Boolean HasSpareTyre { get; set; }
        public Boolean IsSpareTyreDmg { get; set; }
        public String SpareTyreComment { get; set; }
        public List<string> SpareTyreImgList { get; set; }
        public Boolean HasTools { get; set; }
        public Boolean IsToolsDmg { get; set; }
        public String ToolsComment { get; set; }
        public List<string> ToolsImgList { get; set; }
        public Boolean HasJack { get; set; }
        public Boolean IsJackDmg { get; set; }
        public String JackComment { get; set; }
        public List<string> JackImgList { get; set; }
        public Boolean HasCanopy { get; set; }
        public Boolean IsCanopyDmg { get; set; }
        public String CanopyComment { get; set; }
        public List<string> CanopyImgList { get; set; }
        public Boolean HasTrackingUnit { get; set; }
        public Boolean IsTrackingUnitDmg { get; set; }
        public String TrackingUnitComment { get; set; }
        public List<string> TrackingUnitImgList { get; set; }
        public Boolean HasMags { get; set; }
        public Boolean IsMagsDmg { get; set; }
        public String MagsComment { get; set; }
        public List<string> MagsImgList { get; set; }
        public Boolean IsOthers { get; set; }
        public String OthersComment { get; set; }
        public String RadioImgPathList { get; set; }
        public String CDShuffleImgPathList { get; set; }
        public String KeyImgPathList { get; set; }
        public String NavigationImgPathList { get; set; }
        public String AirconImgPathList { get; set; }
        public String AlarmImgPathList { get; set; }
        public String MagsImgPathList { get; set; }
        public String TrackingUnitImgPathList { get; set; }
        public String CanopyImgPathList { get; set; }
        public String JackImgPathList { get; set; }
        public String ToolsImgPathList { get; set; }
        public String SpareKeysImgPathList { get; set; }
        public String ServicesBookImgPathList { get; set; }
        public String SpareTyreImgPathList { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

