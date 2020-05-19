using Pithline.FMS.Framework.Web.Util;
using Pithline.FMS.Framework.Web.Caching;
using Microsoft.ApplicationServer.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Caching.Implementations
{
    [CacheProvider(Name="AppFabric")]
    public class AppFabricCacheProvider:ICacheProvider
    {
        private DataCacheFactory _factory;
        private DataCache _cache;

        public AppFabricCacheProvider()
        {
            dynamic app = new AppConfig();

            
            var configuration = new DataCacheFactoryConfiguration();
            
            DataCacheClientLogManager.ChangeLogLevel(System.Diagnostics.TraceLevel.Off);

            _factory = new DataCacheFactory(configuration);
                
            _cache = _factory.GetDefaultCache();
        }

        public object this[string key]
        {
            get
            {
                return this._cache[key];
            }
            set
            {
                this._cache[key] = value;
            }
        }
    }
}
