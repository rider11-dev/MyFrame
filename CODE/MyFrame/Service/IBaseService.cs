using MyFrame.Infrastructure.OptResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MyFrame.Infrastructure.Pagination;
using MyFrame.Infrastructure.OrderBy;

namespace MyFrame.IService
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        OperationResult Add(TEntity entity);

        OperationResult Count(Expression<Func<TEntity, bool>> where = null);
        OperationResult Delete(Expression<Func<TEntity, bool>> where);
        OperationResult Update(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update);
        OperationResult Exists(Expression<Func<TEntity, bool>> where);

        OperationResult Find(Expression<Func<TEntity, bool>> where);

        OperationResult FindByPage(Expression<Func<TEntity, bool>> where, Action<IOrderable<TEntity>> orderBy, PageArgs pageArgs);
    }
}
