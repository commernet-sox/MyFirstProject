using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExpressionPropertyAttribute:Attribute
    {
        public ExpressionPropertyAttribute(ExpressionOperator @operator = ExpressionOperator.Equal) => Operator = @operator;

        /// <summary>
        /// 操作符
        /// </summary>
        public ExpressionOperator Operator { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
    }
}
