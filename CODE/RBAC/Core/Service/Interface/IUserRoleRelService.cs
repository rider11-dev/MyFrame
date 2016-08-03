using MyFrame.Infrastructure.OptResult;
using MyFrame.Core.Service;
using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Service.Interface
{
    public interface IUserRoleRelService : IBaseService<UserRoleRelation>
    {
        /// <summary>
        /// 设置指定用户的角色
        /// 先清空指定用户所有角色，后分配新角色
        /// </summary>
        /// <param name="usrIds"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        OperationResult SetRoles(int[] usrIds, int[] roleIds);
        /// <summary>
        /// 分配角色到指定用户
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="usrIds"></param>
        /// <returns></returns>
        OperationResult AssignToUsers(int[] roleIds, int[] usrIds);
    }
}
