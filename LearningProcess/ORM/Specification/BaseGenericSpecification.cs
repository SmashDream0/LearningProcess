using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.ORM;
using LearningProcess.ORM.Table;
using LearningProcess.ORM.Specification.Operand;

namespace LearningProcess.ORM.Specification
{
    public class BaseGenericSpecification<T> : BaseSpecification
    {
        public BaseGenericSpecification(BaseData innerOperation)
        { _innerOperation = innerOperation; }
        
        private List<KeyValuePair<ECondition, BaseGenericSpecification<T>>> _specifications = new List<KeyValuePair<ECondition, BaseGenericSpecification<T>>>();

        /// <summary>
        /// Условие запроса
        /// </summary>
        public override string Data
        {
            get
            {
                var sb = new StringBuilder();
                
                AddWhere(sb, this);

                return sb.ToString();
            }
        }

        /// <summary>
        /// Запрос на выборку
        /// </summary>
        public override string SelectQuery
        {
            get
            {
                var sb = new StringBuilder();

                AddSelect(sb); sb.Append(' ');
                AddTableName(sb);

                if (_innerOperation != null)
                {
                    sb.Append(" WHERE ");
                    AddWhere(sb, this);
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Запрос на удаление
        /// </summary>
        public override string DeleteQuery
        {
            get
            {
                var sb = new StringBuilder();

                AddDelete(sb); sb.Append(' ');
                AddTableName(sb);

                if (_innerOperation != null)
                {
                    sb.Append(" WHERE ");
                    AddWhere(sb, this);
                }

                return sb.ToString();
            }
        }

        protected BaseData _innerOperation
        { get; private set; }

        private static void AddDelete(StringBuilder sb)
        { sb.Append("DELETE"); }

        private static void AddSelect(StringBuilder sb)
        {
            sb.Append("SELECT ");

            var type = typeof(T);

            var table = Context.BaseContext.TableByType(type);

            foreach (var column in table.Columns)
            { sb.Append(column.NameDB); sb.Append(','); }

            sb.Length--;
        }

        private static void AddTableName(StringBuilder sb)
        {
            var type = typeof(T);

            var table = Context.BaseContext.TableByType(type);

            sb.Append("FROM ["); sb.Append(table.NameDB); sb.Append("]");
        }

        private static void AddWhere(StringBuilder sb, BaseGenericSpecification<T> spec)
        {
            bool needScobe = spec._specifications.Count > 1;

            if (needScobe)
            { sb.Append('('); }

            sb.Append(spec._innerOperation.Data);

            foreach (var specification in spec._specifications)
            {
                switch (specification.Key)
                {
                    case ECondition.AND:
                        sb.Append("AND ");
                        break;
                    case ECondition.OR:
                        sb.Append("OR ");
                        break;
                    default:
                        throw new Exception($"Unknown operation {specification.Key}");
                }

                AddWhere(sb, specification.Value);
            }

            if (needScobe)
            { sb.Append(')'); }
        }

        public static BaseGenericSpecification<T> operator &(BaseGenericSpecification<T> operaion1, BaseGenericSpecification<T> operation2)
        {
            operaion1._specifications.Add(new KeyValuePair<ECondition, BaseGenericSpecification<T>>(ECondition.AND, operation2));

            return operaion1;
        }

        public static BaseGenericSpecification<T> operator |(BaseGenericSpecification<T> operaion1, BaseGenericSpecification<T> operation2)
        {
            operaion1._specifications.Add(new KeyValuePair<ECondition, BaseGenericSpecification<T>>(ECondition.OR, operation2));

            return operaion1;
        }
    }
}