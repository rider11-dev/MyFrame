using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.Web.Mvc;
using MyFrame.RBAC.Service;
using MyFrame.RBAC.Repository;
using MyFrame.Core.Repository;
using Autofac.Core;
using System.IO;
using MyFrame.Core.Model;
using MyFrame.Core.Configure;
using MyFrame.RBAC.Configure;
using WebApp.Core.Configure;

namespace WebApp.Extensions.Ioc
{
    public class AutofacRegister
    {
        public static void RegisterAutofac()
        {
            ContainerBuilder builder = new ContainerBuilder();
            //1、注册控制器
            //开启了Controller的依赖注入功能,
            //这里使用Autofac提供的RegisterControllers扩展方法来对程序集中所有的Controller一次性的完成注册
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            //2、注册过滤器
            //开启了Filter的依赖注入功能，
            //为过滤器使用属性注入必须在容器创建之前调用RegisterFilterProvider方法，
            //并将其传到AutofacDependencyResolver
            builder.RegisterFilterProvider();

            //3、框架基础注册、rbac模块注册
            new BaseConfigure().Register(builder);
            new RbacConfigure().Register(builder);
            new WebAppConfig().Register(builder);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}