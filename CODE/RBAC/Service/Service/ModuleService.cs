using MyFrame.IRepository.RBAC;
using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Service.RBAC
{
    public class ModuleService : BaseService<Module>, IModuleService
    {
        IModuleRepository _moduleRepository;
        public ModuleService(IModuleRepository moduleRep)
            : base(moduleRep)
        {
            _moduleRepository = moduleRep;
        }
    }
}
