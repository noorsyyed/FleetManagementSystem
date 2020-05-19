using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Caching.Implementations
{
    [CacheProvider(Name="Basic")]
    public class BasicCacheProvider:ICacheProvider
    {
        public object this[string key]
        {
            get
            {
                return "";
            }
            set
            {
                
            }
        }
    }
}
