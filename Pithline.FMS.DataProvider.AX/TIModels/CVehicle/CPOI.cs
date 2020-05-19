using System;
using System.Collections.Generic;
namespace Pithline.FMS.DataProvider.AX.TIModels.CVehicle
{
    public class CPOI
    {
        public DateTime CRTime { get; set; }
        public String CRSignFileName { get; set; }
        public String EQRSignFileName { get; set; }
        public DateTime CRDate { get; set; }
        public DateTime EQRDate { get; set; }
        public DateTime EQRTime { get; set; }
        public string CustSignature { get; set; }
        public string PithlineRepSignature { get; set; }
        public Boolean IsSellChecked { get; set; }
        public Boolean IsNotFeasChecked { get; set; }
        public Boolean IsRetainChecked { get; set; }
        public Boolean IsGoodChecked { get; set; }
        public Boolean IsFairChecked { get; set; }
        public Boolean IsPoorChecked { get; set; }
        public String CaseNumber { get; set; }
        public Int64 VehicleInsRecID { get; set; }
        public Int32 TableId { get; set; }
        public Int64 RecID { get; set; }
        public List<string> Errors { get; set; }
        public Boolean ShouldSave { get; set; }

    }

}

