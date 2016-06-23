using MyFrame.IRepository.RBAC;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Repository.RBAC
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
    }
}
