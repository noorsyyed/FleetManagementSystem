using Pithline.FMS.BusinessLogic.ServiceSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pithline.FMS.BusinessLogic;
namespace Pithline.FMS.BusinessLogic
{
    public class StampPersistData
    {

        private static StampPersistData _instance = new StampPersistData();
        public static StampPersistData Instance { get { return _instance; } }
        public Pithline.FMS.BusinessLogic.Task Task { get; set; }
        public Pithline.FMS.BusinessLogic.CustomerDetails CustomerDetails { get; set; }
        public Pithline.FMS.BusinessLogic.DataStamp DataStamp { get; set; }
        public static void RefreshInstance()
        {
            _instance = new StampPersistData();
        }

    }
}
