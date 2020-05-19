using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.DataProvider.AX.Helpers
{
    public class TaskStatus
    {
        public const string AwaitInspectionDetail = "Await Inspection Detail";
        public const string AwaitInspectionDataCapture = "Await Inspection Data Capture";
        public const string AwaitingConfirmation = "Await Inspection Confirmation";
        public const string AwaitInspectionAcceptance = "Await Inspection Acceptance";
        public const string AwaitDamageConfirmation = "Await Damage Confirmation";
        public const string AwaitCollectionDetail = "Await Collection Detail";
        public const string AwaitVehicleCollection = "Await Vehicle Collection";
        public const string AwaitVendorSelection = "Await Vendor Selection";
        public const string AwaitCollectionConfirmation = "Await Collection Confirmation";


        public const string AwaitServiceDetail = "Await Service Booking Detail";
        public const string AwaitSupplierSelection = "Await Supplier Selection";
        public const string AwaitServiceConfirmation = "Await Service Booking Confirmation";
        public const string Completed = "Completed";

    }
}
