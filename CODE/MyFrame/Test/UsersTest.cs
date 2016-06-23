using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.bll;
using Test.dal;
using Test.entity;

namespace Test
{
    public class UsersTest
    {
        IUsersServiceWrapper srvWrapper;
        public UsersTest()
        {
            IUsersRepository rep = new UsersRepository();
            IUsersService srv = new UsersService(rep);
            srvWrapper = new UsersServiceWrapper(srv);
        }
        public void GetCount()
        {
            var result = srvWrapper.Count();
        }

        public void Insert()
        {
            User usr = new User
            {
                UserName = "张鹏飞",
                Password = "aaaaaa",
                Email = "zpf@163.com",
                Phone = "88888888",
                Address = "山东济南",
                Remark = "呵呵"
            };
            var result = srvWrapper.Add(usr);
        }
    }
}
