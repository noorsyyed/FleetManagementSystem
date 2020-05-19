using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Pithline.FMS.Framework.Web.Util
{
    public class AppConfig : DynamicObject
    {
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = ConfigurationManager.AppSettings[binder.Name];
            return true;
        }
    }
}
