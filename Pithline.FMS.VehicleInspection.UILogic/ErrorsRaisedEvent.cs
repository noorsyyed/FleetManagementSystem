using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.PubSubEvents;
using System.Collections.ObjectModel;
using Pithline.FMS.BusinessLogic.Base;

namespace Pithline.FMS.VehicleInspection.UILogic
{
    public class ErrorsRaisedEvent : PubSubEvent<ObservableCollection<ValidationError>>
    {
    }
}
