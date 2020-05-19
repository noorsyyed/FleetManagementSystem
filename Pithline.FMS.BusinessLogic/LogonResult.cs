using Pithline.FMS.BusinessLogic.DeliveryModel;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic
{
    public class LogonResult
    {
        [RestorableState]
        public UserInfo UserInfo { get; set; }
    }

    public class CDLogonResult
    {
        [RestorableState]
        public CDUserInfo UserInfo { get; set; }
    }
}
