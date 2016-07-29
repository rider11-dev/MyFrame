using Autofac;
using AutoMapper;
using MyFrame.Model.Unit;
using MyFrame.Repository.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.Configure
{
    /// <summary>
    /// 框架基础配置
    /// </summary>
    public class BaseConfigure : IRegister, MyFrame.Configure.IMapper
    {

        public void Register(ContainerBuilder builder)
        {
            //注册默认数据上下文
            builder.RegisterType<EFDbContext>().As<DbContext>().InstancePerRequest();

            //注册工作单元,PropertiesAutowired是为了属性DbContext实现自动注入
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().PropertiesAutowired().InstancePerRequest();
        }

        public void Map(IMapperConfiguration cfg)
        {
        }
    }
}
