using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SDT.BaseTool
{
    public class ExpressionModel<T>
        where T : class
    {
        #region Members
        private readonly List<ExpressionConfig<T>> _configs = new List<ExpressionConfig<T>>();
        #endregion

        #region Methods
        public ExpressionModel<T> Config(string propertyName, Expression<Func<T, bool>> expression)
        {
            AddConfig(new ExpressionConfig<T>(propertyName, expression));
            return this;
        }

        public ExpressionModel<T> Config(Expression<Func<T, object>> columnName, Expression<Func<T, bool>> expression)
        {
            return Config(columnName.GetPropertyName(), expression);
        }

        public ExpressionModel<T> Config(string propertyName, ExpressionOperator @operator, string fieldName = "")
        {
            AddConfig(new ExpressionConfig<T>(propertyName, @operator, fieldName));
            return this;
        }

        public ExpressionModel<T> Config(Expression<Func<T, object>> columnName, ExpressionOperator @operator, string fieldName = "")
        {
            return Config(columnName.GetPropertyName(), @operator, fieldName);
        }

        public ExpressionModel<T> Config(ExpressionConfig<T> config)
        {
            AddConfig(config);
            return this;
        }

        public ExpressionModel<T> ConfigIgnore(string propertyName)
        {
            AddConfig(new ExpressionConfig<T>(propertyName));
            return this;
        }

        public ExpressionModel<T> ConfigIgnore(Expression<Func<T, object>> columnName)
        {
            return ConfigIgnore(columnName.GetPropertyName());
        }

        public ExpressionModel<T> Config(IEnumerable<ExpressionConfig<T>> configs)
        {
            foreach (var config in configs)
            {
                AddConfig(config);
            }
            return this;
        }

        private void AddConfig(ExpressionConfig<T> config)
        {
            lock (_configs)
            {
                if (_configs.Find(t => t.PropertyName == config.PropertyName) != null)
                {
                    throw new InvalidOperationException($"PropertyName:{config.PropertyName} already configured");
                }

                _configs.Add(config);
            }
        }

        public Expression<Func<T, bool>> Generate(object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var result = ExpressionBuilder.True<T>();

            var pros = data.GetType().GetProperties();
            foreach (var pro in pros)
            {
                var propertyName = pro.Name;
                var config = _configs.Find(t => t.PropertyName == propertyName);
                var eop = ExpressionOperator.Equal;
                if (config != null)
                {
                    if (config.Ignore)
                    {
                        continue;
                    }

                    if (config.Expression != null)
                    {
                        result = result.And(config.Expression);
                        continue;
                    }

                    eop = config.Operator;
                    if (!config.FieldName.IsNull())
                    {
                        propertyName = config.FieldName;
                    }
                }
                else
                {
                    var ignore = pro.GetCustomAttribute<ExpressionIgnoreAttribute>(false);
                    if (ignore != null)
                    {
                        continue;
                    }

                    var expAttr = pro.GetCustomAttribute<ExpressionPropertyAttribute>(true);
                    if (expAttr != null)
                    {
                        eop = expAttr.Operator;
                        if (!expAttr.FieldName.IsNull())
                        {
                            propertyName = expAttr.FieldName;
                        }
                    }
                }

                if (eop == ExpressionOperator.None)
                {
                    continue;
                }

                var propertyValue = pro.GetValue(data);
                if (propertyValue == null || propertyValue.ConvertString().IsNull())
                {
                    continue;
                }

                result = result.And(propertyName, propertyValue, eop);
            }

            return result;
        }
        #endregion
    }
    public class ExpressionConfig<T>
    {
        public ExpressionConfig(string propertyName, Expression<Func<T, bool>> expression)
        {
            PropertyName = propertyName;
            Expression = expression;
        }

        public ExpressionConfig(string propertyName, ExpressionOperator @operator, string fieldName = "")
        {
            PropertyName = propertyName;
            Operator = @operator;
            FieldName = fieldName;
        }

        public ExpressionConfig(string propertyName)
        {
            PropertyName = propertyName;
            Ignore = true;
        }

        public string PropertyName { get; private set; }

        public string FieldName { get; set; }

        public Expression<Func<T, bool>> Expression { get; private set; }

        public ExpressionOperator Operator { get; private set; } = ExpressionOperator.Equal;

        public bool Ignore { get; private set; } = false;
    }

    public class ExpressionModel
    {
        public static Expression<Func<T, bool>> Generate<T>(object data)
            where T : class => new ExpressionModel<T>().Generate(data);
    }
}
