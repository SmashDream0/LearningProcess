using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.ORM.Specification.Operand
{
    public class Column<T> : BaseData
        where T: class, IEntity
    {
        public Column(Expression<Func<T, object>> field)
        {
            var propertyInfo = GetPropInfo(field);
            var table = Context.BaseContext.TableByType(typeof(T));

            _columnName = table.ColumnByProperyName(propertyInfo.Name).NameDB;
        }

        private string _columnName;

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