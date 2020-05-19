using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.PVehicle
{
    public class PMechanicalCond
    {

        public String Remarks { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

