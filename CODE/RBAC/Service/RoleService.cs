using MyFrame.IRepository.RBAC;
using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Service.RBAC
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        public RoleService(IRoleRepository _roleRep)
            : base(_roleRep)
        { }
    }
}
