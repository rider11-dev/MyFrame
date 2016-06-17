using MyFrame.Infrastructure.OptResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MyFrame.Infrastructure.Expression;
using MyFrame.Infrastructure.Pagination;

namespace MyFrame.IService
{
    public interface IBaseServiceWrapper<TEntity> where TEntity : class
    {
        OperationResult Add(TEntity entity);

        OperationResult Count(Expression<Func<TEntity, bool>> where);

        OperationResult Delete(TEntity entity);
        OperationResult Update(TEntity entity);
        OperationResult Exists(Expression<Func<TEntity, bool>> where);

        OperationResult Find(Expression<Func<TEntity, bool>> where);

        OperationResult FindByPage(Expression<Func<TEntity, bool>> where, IList<OrderByArgs<TEntity>> orderByList, PageArgs pageArgs);
    }
}
