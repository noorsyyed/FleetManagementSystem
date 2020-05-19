using Pithline.FMS.BusinessLogic.ServiceSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pithline.FMS.BusinessLogic;
using System.Collections.ObjectModel;
namespace Pithline.FMS.ServiceScheduling.UILogic.Helpers
{
    public class PersistentData
    {

        private static PersistentData _instance = new PersistentData();
        public static PersistentData Instance { get { return _instance; } }
        public DriverTask DriverTask { get; set; }
        public ObservableCollection<DriverTask> PoolOfTask { get; set; }
        public Pithline.FMS.BusinessLogic.CustomerDetails CustomerDetails { get; set; }
        public static void RefreshInstance()
        {
            _instance = new PersistentData();
        }

    }
}
