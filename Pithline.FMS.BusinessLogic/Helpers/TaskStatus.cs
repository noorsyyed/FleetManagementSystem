using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Helpers
{
    public class TaskStatus
    {
        public const string AwaitInspectionDetail = "Await Inspection Detail";
        public const string AwaitInspectionDataCapture = "Await Inspection Data Capture";
        public const string AwaitCollectionDataCapture = "Await Collection Data Capture";
        public const string AwaitingInspectionConfirmation = "AwaitInspectionConfirmation";
        public const string AwaitInspectionAcceptance = "Await Inspection Acceptance";
        public const string AwaitDamageConfirmation = "Await Damage Confirmation";
        public const string AwaitCollectionDetail = "Await Collection Detail";
        public const string AwaitVehicleCollection = "Await Vehicle Collection";
        public const string AwaitVendorSelection = "Await Vendor Selection";
        public const string AwaitCollectionConfirmation = "Await Collection Confirmation";
        public const string AwaitTechnicalInspection = "Await Technical Inspection";

        public const string Completed = "Completed";


        /*
         * Await Collection Confirmation
         * Await Supplier Selection
         * Await Collection Data Capture
         * Await Damage Confirmation
         * 
         */
    }
}
