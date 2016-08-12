using Autofac;
using MyFrame.Core.Model;
using MyFrame.Core.Repository;
using MyFrame.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.Core.Configure
{
    /// <summary>
    /// 框架基础配置
    /// </summary>
    public class BaseConfigure : IRegister
    {

        public void Register(ContainerBuilder builder)
        {
            //注册默认数据上下文
            builder.RegisterType<EFDbContext>().As<DbContext>().InstancePerRequest();

            //注册工作单元,PropertiesAutowired是为了属性DbContext实现自动注入
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().PropertiesAutowired().InstancePerRequest();
        }
    }
}
