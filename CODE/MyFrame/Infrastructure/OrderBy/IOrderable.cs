using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFrame.Infrastructure.OrderBy
{
    /// <summary>
    /// 排序接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IOrderable<TEntity>
    {
        IOrderable<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> keySelector);
        IOrderable<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector);
        IOrderable<TEntity> ThenBy<TKey>(Expression<Func<TEntity, TKey>> keySelector);
        IOrderable<TEntity> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector);

        /// <summary>
        /// 排序后的结果集
        /// </summary>
        IQueryable<TEntity> Queryable { get; }
    }
}
