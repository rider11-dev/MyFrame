using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;
using MyFrame.IRepository;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFrame.IService.RBAC
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

        OperationResult FindByPageWithFullInfo(Expression<Func<User, bool>> where, Action<IOrderable<User>> orderBy, PageArgs pageArgs);

        OperationResult SetRoles(int[] usrIds, int[] roleIds);
    }
}
