using MyFrame.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.entity;

namespace Test.bll
{
    public class UsersServiceWrapper : BaseServiceWrapper<User>, IUsersServiceWrapper
    {
        public UsersServiceWrapper(IUsersService srv)
            : base(srv)
        { }
    }
}
