
using MyFrame.Model.Unit;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository;
using MyFrame.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.RBAC.Repository
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
