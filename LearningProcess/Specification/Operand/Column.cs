using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Specification.Operand
{
    public class Column<T> : AData
    {
        public Column(Expression<Func<T, object>> field)
        {
            _columnName = GetPropInfo(field).Name;
            _tableName = typeof(T).Name;
        }

        private readonly string _columnName;
        private readonly string _tableName;

        public override string Data => _columnName;

        public static PropertyInfo GetPropInfo(Expression<Func<T, object>> field)
        {
            var expressionBody = field.Body as MemberExpression;
            if (expressionBody == null) // Для свойств, которые имеют тип отличный от String
            {
                var operand = ((UnaryExpression)field.Body).Operand;
                expressionBody = (MemberExpression)operand;
            }
            return (PropertyInfo)expressionBody.Member;
        }
    }
}