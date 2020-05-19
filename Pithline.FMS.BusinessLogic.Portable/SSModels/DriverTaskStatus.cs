using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Portable.SSModels
{
    public class DriverTaskStatus
    {
        public const string AwaitServiceBookingDetail = "Await Service Booking Detail";
        public const string AwaitSupplierSelection = "Await Supplier Selection";
        public const string AwaitServiceBookingConfirmation = "Await Service Booking Confirmation";
        public const string AwaitJobCardCapture = "Await Job Card Capture";
        public const string Completed = "Completed";
    }
}
