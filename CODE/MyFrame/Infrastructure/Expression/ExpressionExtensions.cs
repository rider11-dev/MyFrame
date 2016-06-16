using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.Infrastructure.Extension
{
    /// <summary>
    /// 查询表达式方法扩展类
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// 扩展方法——构建查询条件And表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first">表达式一</param>
        /// <param name="second">表达式二</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            DynamicLambda<T> builder = new DynamicLambda<T>();
            Expression<Func<T, bool>> combined = builder.BuildQueryAnd(first, second);
            return combined;
        }

        /// <summary>
        /// 扩展方法——构建查询条件Or表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first">表达式一</param>
        /// <param name="second">表达式二</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            DynamicLambda<T> builder = new DynamicLambda<T>();
            Expression<Func<T, bool>> combined = builder.BuildQueryOr(first, second);
            return combined;
        }
    }

    /// <summary>
    /// Dynamic Expression, it is used to combine seperate lambda expressions
    /// </summary>
    /// <typeparam name="T">Type of Entity</typeparam>
    /// <remarks>
    /// For combine the seperate lambda expression, it must change the two lambda have the same parameter, 
    /// otherwise it would throw exception for cannot find the parameter.
    ///</remarks>
    public class DynamicLambda<T>
    {
        private ParameterExpression _parameter;
        private ParameterExpressionRewriter _paraRewriter;

        public DynamicLambda()
        {
            _parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "parameter");
            _paraRewriter = new ParameterExpressionRewriter(_parameter);
        }

        /// <summary>
        /// Combine two lambda epression by AndAlso operation
        /// </summary>
        /// <param name="leftExpression">Left Expression, and it can be null.</param>
        /// <param name="rightExpression">Right Expression, and it can be null.</param>
        /// <returns>Combined Expression. But may be null.</returns>
        /// <remarks>
        /// If leftParameter have value, and rightParameter is null, then return rightParamter.
        /// If rightParameter have value, and leftParameter is null, then return leftParamter.
        /// If both parameters is null, then return null.
        /// </remarks>
        public Expression<Func<T, bool>> BuildQueryAnd(Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {
            //都为null
            if (leftExpression == null && rightExpression == null)
            {
                return null;
            }
            //只有一个为null
            if (leftExpression == null || rightExpression == null)
            {
                return leftExpression == null ? leftExpression : rightExpression;
            }
            //都不为null
            //Change the Left Expression's Parameter
            leftExpression = _paraRewriter.VisitAndConvert(leftExpression, "BuildQueryAnd");
            rightExpression = _paraRewriter.VisitAndConvert(rightExpression, "BuildQueryAnd");
            //Combine two lambda expression by AndAlso operation
            dynamic result = System.Linq.Expressions.Expression.AndAlso(leftExpression.Body, rightExpression.Body);
            //Build the Expression to Lambda, and return it.
            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(result, _parameter);
        }

        /// <summary>
        /// Combine two lambda epression by OrElse operation
        /// </summary>
        /// <param name="leftExpression">Left Expression, and it can be null.</param>
        /// <param name="rightExpression">Right Expression, and it can be null.</param>
        /// <returns>Combined Expression. But may be null.</returns>
        /// <remarks>
        /// If leftParameter have value, and rightParameter is null, then return rightParamter.
        /// If rightParameter have value, and leftParameter is null, then return leftParamter.
        /// If both parameters is null, then return null.
        /// </remarks>
        public Expression<Func<T, bool>> BuildQueryOr(Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {
            //都为null
            if (leftExpression == null && rightExpression == null)
            {
                return null;
            }
            //只有一个为null
            if (leftExpression == null || rightExpression == null)
            {
                return leftExpression == null ? leftExpression : rightExpression;
            }
            //都不为null
            //Change the Left Expression's Parameter
            leftExpression = _paraRewriter.VisitAndConvert(leftExpression, "BuildQueryOr");
            //Change the Right Expression's Parameter
            rightExpression = _paraRewriter.VisitAndConvert(rightExpression, "BuildQueryOr");

            //Combine two lambda expression by OrElse operation
            dynamic result = System.Linq.Expressions.Expression.OrElse(leftExpression.Body, rightExpression.Body);
            //Build the Expression to Lambda, and return it.
            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(result, _parameter);
        }
    }

    /// <summary>
    /// This Class is inherit from ExpressionVisitor.
    /// It is use to rebuild the Expression Tree and change the Expression's Parameter
    /// </summary>
    /// <remarks></remarks>
    public class ParameterExpressionRewriter : ExpressionVisitor
    {
        private ParameterExpression _param;

        public ParameterExpressionRewriter(ParameterExpression param)
        {
            _param = param;
        }

        protected override System.Linq.Expressions.Expression VisitParameter(ParameterExpression node)
        {
            if (node != null && !object.ReferenceEquals(node, _param))
            {
                return _param;
            }
            return node;
        }
    }
}
