using MyFrame.Infrastructure.OptResult;
using MyFrame.IRepository.RBAC;
using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using MyFrame.Repository.RBAC;
using MyFrame.Service.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RBAC
{
    public class UserManage
    {
        IUserServiceWrapper srvWrapper;

        public UserManage()
        {
            IUserRepository rep = new UserRepository();
            IUserService srv = new UserService(rep);
            srvWrapper = new UserServiceWrapper(srv);
        }

        public void AddUser(User usr)
        {
            var rst = srvWrapper.Add(usr);
            if (rst.ResultType == OperationResultType.Success)
            {
                Console.WriteLine("add user succeed");
            }
            else
            {
                Console.WriteLine("add user failed");
            }
        }

        public void GetUserCount()
        {
            var rst = srvWrapper.Count();
            if (rst.ResultType == OperationResultType.Success)
            {
                Console.WriteLine("user count:" + rst.AppendData);
            }
            else
            {
                Console.WriteLine("count user failed");
            }
        }

        public void RemoveUser(int userId)
        {
            var result = srvWrapper.Delete(usr => usr.Id == userId);
            if (result.ResultType == OperationResultType.Success)
            {
                bool delRst = Convert.ToBoolean(result.AppendData);
                Console.WriteLine("remove user {0},id:{1}", delRst ? "succeed" : "failed", userId);
            }
            else
            {
                Console.WriteLine("remove user failed,id:" + userId);
            }
        }

        public void RemoveUser(Expression<Func<User, bool>> where)
        {
            var result = srvWrapper.Delete(where);
            if (result.ResultType == OperationResultType.Success)
            {
                bool delRst = Convert.ToBoolean(result.AppendData);
                Console.WriteLine("remove user {0}", delRst ? "succeed" : "failed");
            }
            else
            {
                Console.WriteLine("remove user failed");
            }
        }

        public void UpdateUserWithCondition(Expression<Func<User, bool>> where, Expression<Func<User, User>> update)
        {
            if (where == null)
            {
                Console.WriteLine("UpdateUserWithCondition faled,where cant't be null");
                return;
            }
            var result = srvWrapper.Update(where, update);
            if (result.ResultType == OperationResultType.Success)
            {
                bool delRst = Convert.ToBoolean(result.AppendData);
                Console.WriteLine("UpdateUserWithCondition {0}", delRst ? "succeed" : "failed");
            }
            else
            {
                Console.WriteLine("UpdateUserWithCondition failed");
            }
        }
        public void UpdateAllUser(Expression<Func<User, User>> update)
        {
            var result = srvWrapper.Update(null, update);
            if (result.ResultType == OperationResultType.Success)
            {
                bool delRst = Convert.ToBoolean(result.AppendData);
                Console.WriteLine("UpdateAllUser {0}", delRst ? "succeed" : "failed");
            }
            else
            {
                Console.WriteLine("UpdateAllUser failed");
            }
        }
    }
}
