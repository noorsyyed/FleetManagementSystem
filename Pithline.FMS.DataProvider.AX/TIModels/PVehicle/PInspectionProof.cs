using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.PVehicle
{
    public class PInspectionProof
    {

        public DateTime CRTime { get; set; }
        public String CRSignFileName { get; set; }
        public String EQRSignFileName { get; set; }
        public DateTime CRDate { get; set; }
        public DateTime EQRDate { get; set; }
        public DateTime EQRTime { get; set; }
        public String CRSignComment { get; set; }
        public String EQRSignComment { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

