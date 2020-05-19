using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.ServiceSchedule
{
    public class LocationType : ValidatableBindableBase
    {
        private string locType;

        public string LocType
        {
            get { return locType; }
            set { SetProperty(ref locType, value); }
        }
        private string locationName;
        public string LocationName
        {
            get { return locationName; }
            set { SetProperty(ref locationName, value); }
        }
        private long recID;
        public long RecID
        {
            get { return recID; }
            set { SetProperty(ref recID, value); }
        }
    }
}
