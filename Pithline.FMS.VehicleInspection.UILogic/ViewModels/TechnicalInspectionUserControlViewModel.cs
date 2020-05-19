using Pithline.FMS.BusinessLogic;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
    class TechnicalInspectionUserControlViewModel : BaseViewModel
    {
        public TechnicalInspectionUserControlViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {

        }

        public override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
           // this.Model = new TIData();
            return System.Threading.Tasks.Task.FromResult<object>(null);
        }
    }
}
