using Pithline.FMS.BusinessLogic.DocumentDelivery;
using Syncfusion.UI.Xaml.Schedule;

namespace Pithline.FMS.DocumentDelivery.UILogic.Helpers
{
    public class PersistentData
    {
        private static PersistentData _instance = new PersistentData();
        public static PersistentData Instance { get { return _instance; } }
        public CDCustomerDetails CustomerDetails { get; set; }
        public ScheduleAppointmentCollection Appointments { get; set; }
        public CollectDeliveryTask CollectDeliveryTask { get; set; }
        public static void RefreshInstance()
        {
            _instance = new PersistentData();
        }
    }
}
