using MyFrame.IRepository.RBAC;
using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Service.RBAC
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(IUserRepository _usrRep)
            : base(_usrRep)
        { }
    }
}
