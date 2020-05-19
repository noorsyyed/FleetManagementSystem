using Pithline.FMS.Framework.Web.Caching;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pithline.FMS.Framework.Web.Util;

namespace Pithline.FMS.Framework.Web.Security
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SecurityProviderManager
    {
        [ImportMany]
        public Lazy<ISecurityProvider, ISecurityProviderMetadata>[] Providers { get; set; }

        [Import]
        public CacheManager Cache { get; set; }

        public ISecurityProvider GetProvider()
        {
            var providerName = MefHelper.Helper.App.SecurityProvider;
            var provider = (from pro in this.Providers where pro.Metadata.Name.Equals(providerName) select pro).FirstOrDefault();
            if (provider != null)
            {
                return provider.Value;
            }
            else
            {
                throw new Exception( providerName);
            }
        }

        public List<string> GetMenuItems(string user)
        {
            var key = "Security." + user;
            var value = (List<string>)this.Cache[key];

            if (value == null)
            {
                value = this.GetProvider().GetMenuPermissions(user);
                this.Cache[key] = value;
            }
            return value;
        }

        public List<object> GetViewPermissions(string user, string form)
        {
            var key = "Security." + user + "." + form;
            var value = (List<object>)this.Cache[key];
            if (value == null)
            {
                value = this.GetProvider().GetViewPermissions(user, form);
                this.Cache[key] = value;
            }
            return value;
        }


    }
}
