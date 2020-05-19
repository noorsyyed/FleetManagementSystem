using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.PVehicle
{
    public class PTyreCondition
    {

        public List<string> RFImgList { get; set; }
        public List<string> LFImgList { get; set; }
        public List<string> RRImgList { get; set; }
        public List<string> LRImgList { get; set; }
        public List<string> SpareImgList { get; set; }
        public String RFTreadDepth { get; set; }
        public String RFMake { get; set; }
        public String RFComment { get; set; }
        public String LFTreadDepth { get; set; }
        public String LFMake { get; set; }
        public String LFComment { get; set; }
        public String RRTreadDepth { get; set; }
        public String RRMake { get; set; }
        public String RRComment { get; set; }
        public String LRTreadDepth { get; set; }
        public String LRMake { get; set; }
        public String LRComment { get; set; }
        public String SpareTreadDepth { get; set; }
        public String SpareMake { get; set; }
        public String SpareComment { get; set; }
        public Boolean IsRFGChecked { get; set; }
        public Boolean IsRFFChecked { get; set; }
        public Boolean IsRFPChecked { get; set; }
        public Boolean IsLFGChecked { get; set; }
        public Boolean IsLFFChecked { get; set; }
        public Boolean IsLFPChecked { get; set; }
        public Boolean IsRRGChecked { get; set; }
        public Boolean IsRRFChecked { get; set; }
        public Boolean IsRRPChecked { get; set; }
        public Boolean IsLRGChecked { get; set; }
        public Boolean IsLRFChecked { get; set; }
        public Boolean IsLRPChecked { get; set; }
        public Boolean IsSpareGChecked { get; set; }
        public Boolean IsSpareFChecked { get; set; }
        public Boolean IsSparePChecked { get; set; }
        public String RFImgPathList { get; set; }
        public String LFImgPathList { get; set; }
        public String RRImgPathList { get; set; }
        public String SpareImgPathList { get; set; }
        public String LRImgPathList { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

