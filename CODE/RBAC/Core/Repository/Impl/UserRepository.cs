
using MyFrame.Core.Model;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository;
using MyFrame.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFrame.Core.UnitOfWork;
using MyFrame.RBAC.Repository.Interface;

namespace MyFrame.RBAC.Repository.Impl
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        public User FindByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            var list = base.Find(u => u.UserName == userName);
            return list.FirstOrDefault();
        }
    }
}
