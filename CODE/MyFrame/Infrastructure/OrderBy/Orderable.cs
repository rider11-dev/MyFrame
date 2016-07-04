using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFrame.Infrastructure.OrderBy
{
    /// <summary>
    /// Linq架构里对集合排序实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Orderable<TEntity> : IOrderable<TEntity>
    {
        private IQueryable<TEntity> _queryable;
        public IQueryable<TEntity> Queryable
        {
            get { return _queryable; }
        }
        public Orderable(IQueryable<TEntity> queryable)
        {
            _queryable = queryable;
        }

        public IOrderable<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            _queryable = _queryable.OrderBy(keySelector);
            return this;
        }

        public IOrderable<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            _queryable = _queryable.OrderByDescending(keySelector);
            return this;
        }

        public IOrderable<TEntity> ThenBy<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            _queryable = (_queryable as IOrderedQueryable<TEntity>).ThenBy(keySelector);
            return this;
        }

        public IOrderable<TEntity> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            _queryable = (_queryable as IOrderedQueryable<TEntity>).ThenByDescending(keySelector);
            return this;
        }
    }
}
