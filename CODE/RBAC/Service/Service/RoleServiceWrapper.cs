using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Service.RBAC
{
    public class RoleServiceWrapper : BaseServiceWrapper<Role>, IRoleServiceWrapper
    {
        public RoleServiceWrapper(IRoleService _roleSrv)
            : base(_roleSrv)
        { }
    }
}
