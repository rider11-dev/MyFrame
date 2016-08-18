using MyFrame.Core.Repository;
using MyFrame.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Core.Models;
using WebApp.Core.Repository.Interface;

namespace WebApp.Core.Repository.Impl
{
    public class UserDetailsRepository : BaseRepository<UserDetails>, IUserDetailsRepository
    {
        public UserDetailsRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
