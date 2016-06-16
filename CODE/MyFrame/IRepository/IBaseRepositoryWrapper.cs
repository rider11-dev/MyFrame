using MyFrame.Infrastructure.OptResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MyFrame.Infrastructure.Expression;
using MyFrame.Infrastructure.Pagination;

namespace MyFrame.IRepository
{
    public interface IBaseRepositoryWrapper<TEntity> where TEntity : class
    {
        OperationResult Entities { get; }

        OperationResult Add(TEntity entity);

        OperationResult Count(Expression<Func<TEntity, bool>> where);

        OperationResult Delete(TEntity entity);
        OperationResult Delete(string id);
        OperationResult Update(TEntity entity);
        OperationResult Exists(Expression<Func<TEntity, bool>> where);

        OperationResult Find(Expression<Func<TEntity, bool>> where);
        OperationResult Find(string id);

        OperationResult FindByPage(Expression<Func<TEntity, bool>> where, IList<OrderByArgs<TEntity>> orderByList, PageArgs pageArgs);
    }
}
