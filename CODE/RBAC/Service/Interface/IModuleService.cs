using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;
using MyFrame.IService;
using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFrame.RBAC.Service
{
    public interface IModuleService : IBaseService<Module>
    {
        OperationResult FindByModuleCode(string moduleCode);
        OperationResult FindByPageWithFullInfo(Expression<Func<Module, bool>> where, Action<IOrderable<Module>> orderBy, PageArgs pageArgs);
        OperationResult FindByPageWithSimpleInfo(Expression<Func<Module, bool>> where, Action<IOrderable<Module>> orderBy, PageArgs pageArgs);

        OperationResult FindByRolesWithSimpleInfo(int[] roleIds);

        /// <summary>
        /// 更新指定模块详细信息
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        OperationResult UpdateDetail(Module module);
    }
}
