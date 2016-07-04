using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFrame.IService.RBAC
{
    public interface IModuleService : IBaseService<Module>
    {
        OperationResult FindByModuleCode(string moduleCode);
        OperationResult FindByPageWithFullInfo(Expression<Func<Module, bool>> where, Action<IOrderable<Module>> orderBy, PageArgs pageArgs);

        /// <summary>
        /// 更新指定模块详细信息
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        OperationResult UpdateDetail(Module module);
    }
}
