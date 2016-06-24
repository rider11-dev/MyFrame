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
        IUserRepository usrRepository;
        public UserService(IUserRepository _usrRep)
            : base(_usrRep)
        {
            usrRepository = _usrRep;
        }

        public User FindByUserName(string userName)
        {
            return usrRepository.FindByUserName(userName);
        }
    }
}
