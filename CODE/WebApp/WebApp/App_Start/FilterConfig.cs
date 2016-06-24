using System.Web;
using System.Web.Mvc;
using WebApp.Extensions.Filters;

namespace WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //注册登录校验过滤器
            //filters.Add(new LoginCheckFilterAttribute());
        }
    }
}
