using System;
using System.Linq.Expressions;

namespace CPC
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<T, bool>> True<T>()
            where T : class => t => true;

        public static Expression<Func<T, bool>> False<T>()
            where T : class => t => false;

        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, object>> columnName, object propertyValue, ExpressionOperator eop)
            where T : class => Create<T>(columnName.GetPropertyName(), propertyValue, eop);

        public static Expression<Func<T, bool>> Create<T>(string propertyName, object propertyValue, ExpressionOperator eop)
            where T : class
        {
            if (eop == ExpressionOperator.None || propertyValue == null || propertyValue.ConvertString().IsNull())
            {
                return True<T>();
            }

            Expression exp = null;
            var p = Expression.Parameter(typeof(T), "p");
            var member = Expression.PropertyOrField(p, propertyName);
            var constant = Expression.Constant(propertyValue);//创建常数

            switch (eop)
            {
                case ExpressionOperator.Equal:
                    {
                        exp = Expression.Equal(member, constant);
                        break;
                    }

                case ExpressionOperator.NotEqual:
                    {
                        exp = Expression.NotEqual(member, constant);
                        break;
                    }

                case ExpressionOperator.GreaterThan:
                    {
                        exp = Expression.GreaterThan(member, constant);
                        break;
                    }

                case ExpressionOperator.GreaterThanOrEqual:
                    {
                        exp = Expression.GreaterThanOrEqual(member, constant);
                        break;
                    }

                case ExpressionOperator.LessThan:
                    {
                        exp = Expression.LessThan(member, constant);
                        break;
                    }

                case ExpressionOperator.LessThanOrEqual:
                    {
                        exp = Expression.LessThanOrEqual(member, constant);
                        break;
                    }

                case ExpressionOperator.Contains:
                case ExpressionOperator.StartsWith:
                case ExpressionOperator.EndsWith:
                    {
                        var name = eop.ToString();
                        var method = typeof(string).GetMethod(name, new[] { typeof(string) });
                        exp = Expression.Call(member, method, constant);
                        break;
                    }

                case ExpressionOperator.NotContains:
                case ExpressionOperator.NotStartsWith:
                case ExpressionOperator.NotEndsWith:
                    {
                        var name = eop.ToString().Substring(3);
                        var method = typeof(string).GetMethod(name, new[] { typeof(string) });
                        exp = Expression.Not(Expression.Call(member, method, constant));
                        break;
                    }

                case ExpressionOperator.In:
                    {
                        var method = constant.Type.GetMethod("Contains");
                        if (method == null)
                        {
                            throw new MissingMethodException(constant.Type.Name);
                        }
                        exp = Expression.Call(constant, method, member);
                        break;
                    }

                case ExpressionOperator.NotIn:
                    {
                        var method = constant.Type.GetMethod("Contains");
                        if (method == null)
                        {
                            throw new MissingMethodException(constant.Type.Name);
                        }
                        exp = Expression.Not(Expression.Call(constant, method, member));
                        break;
                    }

                default:
                    {
                        throw new NotImplementedException(nameof(ExpressionOperator));
                    }
            }
            return Expression.Lambda<Func<T, bool>>(exp, p);
        }
    }

    public enum ExpressionOperator : int
    {
        None = 0,
        Equal = 1,
        NotEqual = 2,
        GreaterThan = 3,
        GreaterThanOrEqual = 4,
        LessThan = 5,
        LessThanOrEqual = 6,
        Contains = 7,
        NotContains = 8,
        StartsWith = 9,
        NotStartsWith = 10,
        EndsWith = 11,
        NotEndsWith = 12,
        In = 13,
        NotIn = 14
    }
}
