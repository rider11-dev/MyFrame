using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Service.RBAC
{
    public class ModuleServiceWrapper : BaseServiceWrapper<Module>, IModuleServiceWrapper
    {
        public ModuleServiceWrapper(IModuleService _moduleSrv)
            : base(_moduleSrv)
        { }
    }
}
