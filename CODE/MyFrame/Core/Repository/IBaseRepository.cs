using MyFrame.Infrastructure.OptResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MyFrame.Infrastructure.Pagination;
using MyFrame.Infrastructure.OrderBy;
using System.Data;

namespace MyFrame.Core.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Entities { get; }

        TEntity Add(TEntity entity);
        int Count(Expression<Func<TEntity, bool>> where = null);
        bool Delete(Expression<Func<TEntity, bool>> where);
        bool Update(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update);
        bool Exists(Expression<Func<TEntity, bool>> where);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where);

        IQueryable<TEntity> FindByPage(Expression<Func<TEntity, bool>> where, Action<IOrderable<TEntity>> orderBy, PageArgs pageArgs);
        IQueryable<TEntity> QueryByPage(IQueryable<TEntity> querable, Action<IOrderable<TEntity>> orderBy, PageArgs pageArgs);

    }
}
