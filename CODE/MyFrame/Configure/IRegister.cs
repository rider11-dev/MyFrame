using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Configure
{
    public interface IRegister
    {
        void Register(ContainerBuilder builder);
    }
}
