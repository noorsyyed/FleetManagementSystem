
using Pithline.FMS.BusinessLogic.Portable.SSModels;
using System.Collections.ObjectModel;
namespace Pithline.FMS.ServiceScheduling.UILogic
{
    public class PersistentData
    {
        private static PersistentData _instance = new PersistentData();
        public static PersistentData Instance { get { return _instance; } }
        public static void RefreshInstance()
        {
            _instance = new PersistentData();
        }
        public ObservableCollection<BusinessLogic.Portable.SSModels.Task> PoolofTasks { get; set; }
        public ObservableCollection<Supplier> PoolofSupplier { get; set; }
        public ServiceSchedulingDetail ServiceSchedulingDetail { get; set; }
    }
}
