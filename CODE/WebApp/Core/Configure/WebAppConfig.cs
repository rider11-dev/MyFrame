using Autofac;
using MyFrame.Core.Configure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Core.Repository;
using WebApp.Core.Repository.Impl;
using WebApp.Core.Repository.Interface;
using WebApp.Core.Service.Impl;
using WebApp.Core.Service.Interface;

namespace WebApp.Core.Configure
{
    public class WebAppConfig : IRegister
    {
        public void Register(ContainerBuilder builder)
        {
            //1、注册数据库上下文
            builder.RegisterType<EFDbContextWebApp>().As<DbContext>().InstancePerRequest();

            //2、注册业务模块
            //用户详细信息
            builder.RegisterType<UserDetailsRepository>().As<IUserDetailsRepository>().InstancePerRequest();
            builder.RegisterType<UserDetailsService>().As<IUserDetailsService>().InstancePerRequest();
        }
    }
}
