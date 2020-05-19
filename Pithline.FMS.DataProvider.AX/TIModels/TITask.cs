using Pithline.FMS.DataProvider.AX.TI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.DataProvider.AX.TIModels
{
    public class TITask : Task
    {
        public string CustEmailId { get; set; }
        public List<MaintenanceRepair> ComponentList { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }

        public string EngineNumber { get; set; }
    }
}
