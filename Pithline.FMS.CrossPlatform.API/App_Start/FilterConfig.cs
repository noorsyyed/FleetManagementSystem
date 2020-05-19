using System.Web;
using System.Web.Mvc;

namespace Pithline.FMS.CrossPlatform.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
