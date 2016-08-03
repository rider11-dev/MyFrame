using MyFrame.RBAC.Repository;
using MyFrame.Core.Repository;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
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
            //MiniProfilerEF6初始化，必须放在第一行
            //因为要监控ef，所以要在ef初始化之前
            MiniProfilerEF6.Initialize();

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

            //ioc配置
            AutofacRegister.RegisterAutofac();
            //automapper配置
            AutoMapperRegister.Configure();
        }

        protected void Application_BeginRequest()
        {
            //这里是允许本地访问启动监控,可不写
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }
    }
}
