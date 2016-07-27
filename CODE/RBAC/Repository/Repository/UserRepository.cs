using MyFrame.IRepository.RBAC;
using MyFrame.Model.RBAC;
using MyFrame.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Repository.RBAC
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
