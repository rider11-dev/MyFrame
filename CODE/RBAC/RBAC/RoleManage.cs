using MyFrame.Infrastructure.OptResult;
using MyFrame.IRepository.RBAC;
using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using MyFrame.Repository.RBAC;
using MyFrame.Service.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RBAC
{
    public class RoleManage
    {
        IRoleServiceWrapper srvWrapper;

        public RoleManage()
        {
            IRoleRepository rep = new RoleRepository();
            IRoleService srv = new RoleService(rep);
            srvWrapper = new RoleServiceWrapper(srv);
        }

        public void AddRole(Role role)
        {
            var rst = srvWrapper.Add(role);
            if (rst.ResultType == OperationResultType.Success)
            {
                Console.WriteLine("add Role succeed");
            }
            else
            {
                Console.WriteLine("add Role failed");
            }
        }

        public void GetRoleCount()
        {
            var rst = srvWrapper.Count();
            if (rst.ResultType == OperationResultType.Success)
            {
                Console.WriteLine("Role count:" + rst.AppendData);
            }
            else
            {
                Console.WriteLine("count Role failed");
            }
        }
    }
}
