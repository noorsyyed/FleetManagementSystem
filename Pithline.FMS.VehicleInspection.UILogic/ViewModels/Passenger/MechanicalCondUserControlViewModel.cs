using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Common;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
   public class MechanicalCondUserControlViewModel : BaseViewModel
    {
       public MechanicalCondUserControlViewModel(IEventAggregator eventAggregator):base(eventAggregator)
       {
           this.Model = new PMechanicalCond();
       }

       public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
       {
           this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PMechanicalCond>(x => x.VehicleInsRecID == vehicleInsRecID);
           if (this.Model == null)
           {
               this.Model = new PMechanicalCond();
           }
           BaseModel viBaseObject = (PMechanicalCond)this.Model;
           viBaseObject.VehicleInsRecID = vehicleInsRecID;
           viBaseObject.LoadSnapshotsFromDb();

           viBaseObject.ShouldSave = false;
           PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
       }
    }
}
