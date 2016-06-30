using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFrame.Infrastructure.Expression
{
    /// <summary>
    /// 查询表达式排序参数
    /// </summary>
    public class OrderByArgs<T> where T : class
    {
        public Expression<Func<T, object>> Expression { get; set; }
        public OrderByType OrderByType { get; set; }
    }

    public enum OrderByType
    {
        Asc,
        Desc
    }
}
