using MyFrame.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.dal;
using Test.entity;

namespace Test.bll
{
    public class UsersService : BaseService<User>, IUsersService
    {
        public UsersService(IUsersRepository _usersRep)
            : base(_usersRep)
        { }


    }
}
