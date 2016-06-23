using MyFrame.Model.RBAC;
using MyFrame.Repository.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RBAC
{
    class Program
    {
        static void Main(string[] args)
        {
            InitDbContext();

            UserTest();

            //RoleTest();

            Console.ReadKey();
        }

        static void UserTest()
        {
            UserManage usrManage = new UserManage();
            usrManage.GetUserCount();
            User usr = new User
            {
                UserName = "张鹏飞",
                Password = "aaaaaa",
                Email = "zpf@163.com",
                Phone = "88889999",
                Address = "山东济南",
                CreateTime = DateTime.Now,
                Remark = "呵呵"
            };
            usrManage.AddUser(usr);

            /*
            usrManage.RemoveUser(2);
            usrManage.RemoveUser(4);
            usrManage.RemoveUser(u => u.Id > 0);

            usrManage.GetUserCount();
             * 
             */
            usrManage.UpdateAllUser(u => new User { LastModifyTime = DateTime.Now });
            usrManage.UpdateUserWithCondition(null, u => new User { Enabled = !u.Enabled });
            usrManage.UpdateUserWithCondition(u => u.Enabled == false, u => new User { Enabled = !u.Enabled });
        }

        static void RoleTest()
        {
            RoleManage roleManage = new RoleManage();
            roleManage.GetRoleCount();
            Role role = new Role
            {
                RoleName = "管理员",
                Remark = "gly",
                SortOrder = 1,
                CreateTime = DateTime.Now
            };
            roleManage.AddRole(role);

            roleManage.GetRoleCount();
        }

        static void InitDbContext()
        {
            EFDbContextFactory.DbContextProvider = new EFDbContextProviderRBAC();
        }
    }
}
