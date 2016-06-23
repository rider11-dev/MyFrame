using MyFrame.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.entity;

namespace Test.dal
{
    public interface IUsersRepository : IBaseRepository<User>
    {
    }
}
