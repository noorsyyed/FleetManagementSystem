using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Commercial;
using Pithline.FMS.BusinessLogic.Common;
using Pithline.FMS.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
   public class ChassisBodyUserControlViewModel : BaseViewModel
    {
       public ChassisBodyUserControlViewModel(IEventAggregator eventAggregator)
           : base(eventAggregator)
        {
            this.Model = new CChassisBody();
        }

       public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
       {
           this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CChassisBody>(x => x.VehicleInsRecID == vehicleInsRecID);
           if (this.Model == null)
           {
               this.Model = new CChassisBody();
           }
           BaseModel viBaseObject = (CChassisBody)this.Model;
           viBaseObject.LoadSnapshotsFromDb();
           viBaseObject.VehicleInsRecID = vehicleInsRecID;
           PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
           viBaseObject.ShouldSave = false;
       }
    }
}
