
using MyFrame.RBAC.Model;
using MyFrame.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.RBAC.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User FindByUserName(string userName);
    }
}
