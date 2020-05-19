using Pithline.FMS.Framework.Web.Caching;
using Pithline.FMS.Framework.Web.DataAccess;
using Pithline.FMS.Framework.Web.Exceptions;
using Pithline.FMS.Framework.Web.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Util
{
    public class MefHelper
    {
        private static MefHelper _helper;

        [Import]
        public DataAccessManager DataAccess { get; set; }

        [Import]
        public ExceptionHandlerManager ExceptionHandler { get; set; }

        public dynamic App { get; private set; }

        [Import]
        public SecurityProviderManager SecurityProvider { get; set; }

        [Import]
        public CacheManager Cache { get; set; }


        public MefHelper(ComposablePartCatalog catalog)
        {
            //var catalog = new AggregateCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()),new DirectoryCatalog(".","Mazik.DataProvider.*.dll"));
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
            this.App = new AppConfig();
        }

        public static void Initialize(ComposablePartCatalog catalog)
        {
            _helper = new MefHelper(catalog);
        }

        public static MefHelper Helper
        {
            get
            {
                return _helper;
            }
        }

        public static void Error(Exception ex)
        {
            Helper.ExceptionHandler.Handle(ex);
        }

        public static List<object> GetViewPermissions(string user, string form)
        {
            return Helper.SecurityProvider.GetViewPermissions(user, form);
        }

        public static object GetSingle(string provider, object[] param, bool isCached)
        {
            return Helper.DataAccess.GetSingle(provider, param, isCached);
        }
    }
}
