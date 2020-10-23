using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SDT.BaseTool
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// or superposition of lambda expressions 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> aim, Expression<Func<T, bool>> expr)
        {
            if (aim == null || expr == null)
            {
                return aim;
            }

            var invokedExpr = Expression.Invoke(expr, aim.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(aim.Body, invokedExpr), aim.Parameters);
        }

        /// <summary>
        /// or superposition of lambda expressions 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="eop"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> aim, string propertyName, object propertyValue, ExpressionOperator eop = ExpressionOperator.Equal)
            where T : class
        {
            var expr = ExpressionBuilder.Create<T>(propertyName, propertyValue, eop);
            return aim.Or(expr);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> aim, Expression<Func<T, object>> columnName, object propertyValue, ExpressionOperator eop = ExpressionOperator.Equal)
            where T : class => aim.Or(columnName.GetPropertyName(), propertyValue, eop);

        /// <summary>
        /// and superposition of lambda expressions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> aim, Expression<Func<T, bool>> expr)
        {
            if (aim == null || expr == null)
            {
                return aim;
            }

            var invokedExpr = Expression.Invoke(expr, aim.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(aim.Body, invokedExpr), aim.Parameters);
        }

        /// <summary>
        /// and superposition of lambda expressions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="eop"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> aim, string propertyName, object propertyValue, ExpressionOperator eop = ExpressionOperator.Equal)
            where T : class
        {
            var expr = ExpressionBuilder.Create<T>(propertyName, propertyValue, eop);
            return aim.And(expr);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> aim, Expression<Func<T, object>> columnName, object propertyValue, ExpressionOperator eop = ExpressionOperator.Equal)
            where T : class => aim.And(columnName.GetPropertyName(), propertyValue, eop);

        public static string GetPropertyName(this Expression method)
        {
            var lambda = method as LambdaExpression;

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            return memberExpr.Member.Name;
        }
    }
}
