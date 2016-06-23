using MyFrame.Model.RBAC;
using MyFrame.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.IRepository.RBAC
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}
