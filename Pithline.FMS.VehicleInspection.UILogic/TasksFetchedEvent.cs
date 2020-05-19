using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.VehicleInspection.UILogic
{
  public  class TasksFetchedEvent : PubSubEvent<IEnumerable<Pithline.FMS.BusinessLogic.Task>>
    {
    }
}
