using Autofac;
using MyFrame.Core.Configure;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository;
using MyFrame.RBAC.Service;
using MyFrame.RBAC.ViewModel;
using MyFrame.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFrame.RBAC.Repository.Impl;
using MyFrame.RBAC.Service.Impl;
using MyFrame.RBAC.Repository.Interface;
using MyFrame.RBAC.Service.Interface;

namespace MyFrame.RBAC.Configure
{
    /// <summary>
    /// rbac模块配置
    /// </summary>
    public class RbacConfigure : IRegister
    {
        public void Register(ContainerBuilder builder)
        {
            //1、注册rbac数据上下文
            builder.RegisterType<EFDbContextRBAC>().As<DbContext>().InstancePerRequest();

            //2、注册业务模块
            //用户
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();

            //角色
            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerRequest();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerRequest();

            //用户角色关系
            builder.RegisterType<UserRoleRelRepository>().As<IUserRoleRelRepository>().InstancePerRequest();
            builder.RegisterType<UserRoleRelService>().As<IUserRoleRelService>().InstancePerRequest();

            //角色权限关系
            builder.RegisterType<RolePermissionRepository>().As<IRolePermissionRepository>().InstancePerRequest();
            builder.RegisterType<RolePermissionService>().As<IRolePermissionService>().InstancePerRequest();


            //模块
            builder.RegisterType<ModuleRepository>().As<IModuleRepository>().InstancePerRequest();
            builder.RegisterType<ModuleService>().As<IModuleService>().InstancePerRequest();

            //操作
            builder.RegisterType<OperationRepository>().As<IOperationRepository>().InstancePerRequest();
            builder.RegisterType<OperationService>().As<IOperationService>().InstancePerRequest();

        }
    }
}
