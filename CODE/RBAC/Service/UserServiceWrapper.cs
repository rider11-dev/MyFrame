using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Service.RBAC
{
    public class UserServiceWrapper : BaseServiceWrapper<User>, IUserServiceWrapper
    {
        public UserServiceWrapper(IUserService _usrSrv)
            : base(_usrSrv)
        { }
    }
}
