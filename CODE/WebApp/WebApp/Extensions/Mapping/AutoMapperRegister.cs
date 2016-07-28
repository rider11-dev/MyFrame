using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.ViewModel;
using MyFrame.RBAC.Configure;

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
                new RbacConfigure().Map(cfg);
            });
            //校验
            Mapper.AssertConfigurationIsValid();
        }
    }
}