using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.PVehicle
{
    public class PTrimInterior
    {

        public List<string> InternalTrimImgList { get; set; }
        public List<string> RRDoorTrimImgList { get; set; }
        public List<string> LRDoorTrimImgList { get; set; }
        public List<string> RFDoorTrimImgList { get; set; }
        public List<string> LFDoorTrimImgList { get; set; }
        public List<string> DriverSeatImgList { get; set; }
        public List<string> PassengerSeatImgList { get; set; }
        public List<string> RearSeatImgList { get; set; }
        public List<string> DashImgList { get; set; }
        public List<string> CarpetImgList { get; set; }
        public String InternalTrimImgPathList { get; set; }
        public String RRDoorTrimImgPathList { get; set; }
        public String LRDoorTrimImgPathList { get; set; }
        public String RFDoorTrimImgPathList { get; set; }
        public String LFDoorTrimImgPathList { get; set; }
        public String DriverSeatImgPathList { get; set; }
        public String PassengerSeatImgPathList { get; set; }
        public String RearSeatImgPathList { get; set; }
        public String DashImgPathList { get; set; }
        public String CarpetImgPathList { get; set; }
        public String InternalTrimComment { get; set; }
        public Boolean IsInternalTrimDmg { get; set; }
        public Boolean IsRRDoorTrimDmg { get; set; }
        public String RRDoorTrimComment { get; set; }
        public Boolean IsLFDoorTrimDmg { get; set; }
        public String LFDoorTrimComment { get; set; }
        public String RFDoorTrimComment { get; set; }
        public Boolean IsRFDoorTrimDmg { get; set; }
        public String LRDoorTrimComment { get; set; }
        public Boolean IsLRDoorTrimDmg { get; set; }
        public Boolean IsDriverSeatDmg { get; set; }
        public String DriverSeatComment { get; set; }
        public Boolean IsPassengerSeatDmg { get; set; }
        public String PassengerSeatComment { get; set; }
        public Boolean IsRearSeatDmg { get; set; }
        public String RearSeatComment { get; set; }
        public Boolean IsDashDmg { get; set; }
        public String DashComment { get; set; }
        public Boolean IsCarpetDmg { get; set; }
        public String CarpetComment { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

