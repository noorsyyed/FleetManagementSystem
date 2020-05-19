using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.VehicleInspection.BackgroundTask
{

    public sealed class VITask
    {
        public string CaseNumber { get; set; }

        public string Customer { get; set; }

        public string Address { get; set; }
    }
}
