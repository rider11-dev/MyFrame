using MyFrame.IRepository.RBAC;
using MyFrame.Repository.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApp.Extensions.Ioc;
using WebApp.Extensions.Mapping;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //ASP.NET 路由操作处理所有请求（甚至包括与现有文件匹配的请求）
            RouteTable.Routes.RouteExistingFiles = true;
            //注册视图引擎
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            ViewEngines.Engines.Add(new WebFormViewEngine());
            //注册所有区域
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //注册autofac
            AutofacRegister.RegisterAutofac();
            //注册automapper
            AutoMapperRegister.Configure();

            //解析数据库上下文提供者
            EFDbContextFactory.DbContextProvider = DependencyResolver.Current.GetService<IEFDbContextProvider>();
        }
    }
}
