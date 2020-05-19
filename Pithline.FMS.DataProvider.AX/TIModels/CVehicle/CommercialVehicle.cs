using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.CVehicle
{
    public class CommercialVehicle
    {
        public String Color { get; set; }
        public String ODOReading { get; set; }
        public String RegistrationNumber { get; set; }
        public String EngineNumber { get; set; }
        public String ChassisNumber { get; set; }
        public String Make { get; set; }
        public String Year { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

