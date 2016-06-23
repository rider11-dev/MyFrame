using MyFrame.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.entity;

namespace Test.bll
{
    interface IUsersServiceWrapper : IBaseServiceWrapper<User>
    {
    }
}
