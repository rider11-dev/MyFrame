using MyFrame.Infrastructure.Expression;
using MyFrame.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFrame.IService
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);

        int Count(Expression<Func<TEntity, bool>> where);

        bool Delete(TEntity entity);
        bool Update(TEntity entity);
        bool Exists(Expression<Func<TEntity, bool>> where);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where);
        IQueryable<TEntity> FindByPage(Expression<Func<TEntity, bool>> where, IList<OrderByArgs<TEntity>> orderByList, PageArgs pageArgs);
    }
}
