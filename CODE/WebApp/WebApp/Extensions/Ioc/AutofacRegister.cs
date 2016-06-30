using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.Web.Mvc;
using MyFrame.Service.RBAC;
using MyFrame.IService.RBAC;
using MyFrame.Repository.RBAC;
using MyFrame.IRepository.RBAC;
using MyFrame.Repository.EF;
using Autofac.Core;

namespace WebApp.Extensions.Ioc
{
    public class AutofacRegister
    {
        public static void RegisterAutofac()
        {
            ContainerBuilder builder = new ContainerBuilder();
            //开启了Controller的依赖注入功能,
            //这里使用Autofac提供的RegisterControllers扩展方法来对程序集中所有的Controller一次性的完成注册
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //开启了Filter的依赖注入功能，
            //为过滤器使用属性注入必须在容器创建之前调用RegisterFilterProvider方法，
            //并将其传到AutofacDependencyResolver
            builder.RegisterFilterProvider();

            #region IOC注册区域
            //注册一个接口多个实现并定义多个Name的情况需要使用的Helper类
            builder.RegisterType<AutofacHelper>().As<IAutofacHelper>().InstancePerRequest();
            //注册dbcontext提供器
            builder.RegisterType<EFDbContextProviderRBAC>().As<IEFDbContextProvider>();
            //注册RBAC模块 
            RegisterInstanceForRBAC(builder);
            #endregion
            //注册autofac
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
        private static void RegisterInstanceForRBAC(ContainerBuilder builder)
        {
            //用户
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<UserServiceWrapper>().As<IUserServiceWrapper>().InstancePerRequest();

            //角色
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerRequest();
            builder.RegisterType<RoleServiceWrapper>().As<IRoleServiceWrapper>().InstancePerRequest();

            //模块
            builder.RegisterType<ModuleRepository>().As<IModuleRepository>().InstancePerRequest();
            builder.RegisterType<ModuleService>().As<IModuleService>().InstancePerRequest();
            builder.RegisterType<ModuleServiceWrapper>().As<IModuleServiceWrapper>().InstancePerRequest();
        }
    }
}