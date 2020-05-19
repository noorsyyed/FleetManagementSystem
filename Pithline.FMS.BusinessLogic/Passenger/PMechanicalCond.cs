using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Passenger
{
    public class PMechanicalCond : BaseModel
    {
        private string remarks;
        public string Remarks
        {
            get { return remarks; }

            set { SetProperty(ref  remarks, value); }
        }

        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<PMechanicalCond>(x => x.VehicleInsRecID == vehicleInsRecID);
        }
    }
}
