using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;
using MyFrame.Core.Service;
using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFrame.RBAC.Service.Interface
{
    public interface IRoleService : IBaseService<Role>
    {
        OperationResult FindByName(string roleName);

        /// <summary>
        /// 更新指定角色详细信息
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        OperationResult UpdateDetail(Role role);

        OperationResult FindByPageWithFullInfo(Expression<Func<Role, bool>> where, Action<IOrderable<Role>> orderBy, PageArgs pageArgs);

        OperationResult DeleteWithRelations(int[] roleIds);
    }
}
