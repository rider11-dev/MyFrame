using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MyFrame.Model.RBAC;
using MyFrame.ViewModel.RBAC;

namespace WebApp.Extensions.Mapping
{
    /// <summary>
    /// automapper注册
    /// </summary>
    public class AutoMapperRegister
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<RoleViewModel, Role>();
                cfg.CreateMap<UserViewModel, User>()
                    .ForMember(m => m.Password, opt => opt.Ignore());
                cfg.CreateMap<ModuleViewModel, Module>();

            });
            //校验
            Mapper.AssertConfigurationIsValid();
        }
    }
}