﻿using Autofac;
using AutoMapper;
using MyFrame.Configure;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository;
using MyFrame.RBAC.Service;
using MyFrame.RBAC.ViewModel;
using MyFrame.Repository.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Configure
{
    /// <summary>
    /// rbac模块配置
    /// </summary>
    public class RbacConfigure : IRegister, MyFrame.Configure.IMapper
    {
        public void Register(ContainerBuilder builder)
        {
            //1、注册rbac数据上下文提供器
            builder.RegisterType<EFDbContextProviderRBAC>().As<IEFDbContextProvider>().InstancePerLifetimeScope();

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
        }

        public void Map(IMapperConfiguration cfg)
        {
            cfg.CreateMap<RoleViewModel, Role>();
            cfg.CreateMap<UserViewModel, User>()
                .ForMember(m => m.Password, opt => opt.Ignore());
            cfg.CreateMap<ModuleViewModel, MyFrame.RBAC.Model.Module>();
        }
    }
}
