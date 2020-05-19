using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Helpers
{
    public class CDTaskStatus
    {
        public static string AwaitDriverCollection = "Await Driver Collection";
        public static string AwaitCollectionDetail = "Await Collection Detail";
        public static string AwaitCourierCollection = "Await Courier Collection";

        public static string AwaitDeliveryConfirmation = "Await Delivery Confirmation";
        public static string AwaitLicenceDisc = "Await Licence Disc";
        public static string AwaitInvoice = "Await Invoice";

        public static string Completed = "Completed";
    }
}
