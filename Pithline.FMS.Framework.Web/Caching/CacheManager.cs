using Pithline.FMS.Framework.Web.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Caching
{
    [Export]
    public class CacheManager
    {
        [ImportMany(AllowRecomposition = true)]
        public List<Lazy<ICacheProvider, ICacheProviderMetadata>> Providers { get; set; }

        private dynamic _conf;

        public CacheManager() 
        {
            this._conf = new AppConfig();
        }

        private ICacheProvider FindProvider(string provider)
        {
            var prov = (from p in this.Providers where p.Metadata.Name.Equals(provider) select p).FirstOrDefault();
            return (prov != null) ? prov.Value : null;
        }

        public object this[string key]
        {
            get
            {
                var provider = this.FindProvider(this._conf.CacheProvider);
                return provider[key];
            }
            set
            {
                var provider = this.FindProvider(this._conf.CacheProvider);
                provider[key] = value;
            }
        }
    }
}
