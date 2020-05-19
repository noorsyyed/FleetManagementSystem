using System;
namespace Pithline.FMS.DataProvider.AX.TIModels.PVehicle
{
    public class PVehicleDetails
    {

        public string LeftSnapshot { get; set; }
        public Boolean IsLicenseDiscCurrent { get; set; }
        public DateTime LicenseDiscExpireDate { get; set; }
        public Boolean IsSpareKeysShown { get; set; }
        public Boolean IsSpareKeysTested { get; set; }
        public string LicenseDiscSnapshot { get; set; }
        public string ODOReadingSnapshot { get; set; }
        public string BackSnapshot { get; set; }
        public string RightSnapshot { get; set; }
        public string FrontSnapshot { get; set; }
        public string TopSnapshot { get; set; }
        public String LeftSnapshotPath { get; set; }
        public String BackSnapshotPath { get; set; }
        public String RightSnapshotPath { get; set; }
        public String FrontSnapshotPath { get; set; }
        public String TopSnapshotPath { get; set; }
        public String LicenseDiscSnapshotPath { get; set; }
        public String ODOReadingSnapshotPath { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public Boolean ShouldSave { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

