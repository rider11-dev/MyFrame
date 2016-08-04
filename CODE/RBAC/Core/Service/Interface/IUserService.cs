using MyFrame.Infrastructure.OptResult;

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
    public interface IUserService : IBaseService<User>
    {
        OperationResult FindByUserName(string userName);

        /// <summary>
        /// 更新指定用户详细信息
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        OperationResult UpdateDetail(User usr);

        OperationResult FindByPageWithFullInfo(Expression<Func<User, bool>> where, Func<IQueryable<User>, IOrderedQueryable<User>> orderBy, PageArgs pageArgs);

        OperationResult DeleteWithRelations(int[] usrIds);
    }
}
