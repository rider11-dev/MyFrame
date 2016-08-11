using MyFrame.Core.Service;
using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.Pagination;
using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Service.Interface
{
    public interface IOperationService : IBaseService<Operation>
    {
        OperationResult FindByOptCode(int moduleId, string optCode);

        OperationResult FindByPageWithFullInfo(Expression<Func<Operation, bool>> where, Func<IQueryable<Operation>, IOrderedQueryable<Operation>> orderBy, PageArgs pageArgs);
        OperationResult FindByPageWithSimpleInfo(Expression<Func<Operation, bool>> where, Func<IQueryable<Operation>, IOrderedQueryable<Operation>> orderBy, PageArgs pageArgs);
        OperationResult FindByRolesWithSimpleInfo(int[] roleIds);
        OperationResult UpdateDetail(Operation opt);
        OperationResult GetOptInfoByController(string controller, bool enableRbac = false);

    }
}
