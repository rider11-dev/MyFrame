using System.Web;
using System.Web.Mvc;
using WebApp.Extensions.Filters;

namespace WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());//Application_Error将不再触发
            filters.Add(new CustomHandleErrorAttribute { View = "~/Views/Common/Error.cshtml" });
        }
    }
}
