using MyFrame.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.entity;

namespace Test.dal
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public UsersRepository()
            : base()
        { }
    }
}
