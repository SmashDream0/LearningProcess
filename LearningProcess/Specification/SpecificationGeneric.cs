using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Specification
{
    public class Specification<T> : Specification
    {
        public Specification(AData innerOperation)
        { _innerOperation = innerOperation; }

        /// <summary>
        /// Кеш списков колонок типов
        /// </summary>
        private static Dictionary<Type, string> _selectDictionary = new Dictionary<Type, string>();
        /// <summary>
        /// Список имен таблиц, по аттрибуту или по имени типа
        /// </summary>
        private static Dictionary<Type, string> _tableNameDictionary = new Dictionary<Type, string>();

        private List<KeyValuePair<ECondition, Specification<T>>> _specifications = new List<KeyValuePair<ECondition, Specification<T>>>();

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

        protected AData _innerOperation { get; private set; }

        private static void AddDelete(StringBuilder sb)
        {
            sb.Append("DEPETE");
        }

        private static void AddSelect(StringBuilder sb)
        {
            sb.Append("SELECT ");

            var type = typeof(T);

            if (!_selectDictionary.ContainsKey(type))
            {
                var props = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .Where(x => x.GetMethod != null && x.SetMethod != null)
                    .Select(x => x.Name)
                    .ToArray();

                var txtProps = String.Join(",", props);

                _selectDictionary.Add(type, txtProps);
            }

            sb.Append(_selectDictionary[type]);
        }

        private static void AddTableName(StringBuilder sb)
        {
            var type = typeof(T);

            if (!_tableNameDictionary.ContainsKey(type))
            {
                var tableAattribute = type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;

                if (tableAattribute != null)
                { _tableNameDictionary.Add(type, tableAattribute.Name); }
                else
                { _tableNameDictionary.Add(type, type.Name); }
            }

            sb.Append("FROM ["); sb.Append(_tableNameDictionary[type]); sb.Append("]");
        }

        private static void AddWhere(StringBuilder sb, Specification<T> spec)
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

        public static Specification<T> operator &(Specification<T> operaion1, Specification<T> operation2)
        {
            operaion1._specifications.Add(new KeyValuePair<ECondition, Specification<T>>(ECondition.AND, operation2));

            return operaion1;
        }

        public static Specification<T> operator |(Specification<T> operaion1, Specification<T> operation2)
        {
            operaion1._specifications.Add(new KeyValuePair<ECondition, Specification<T>>(ECondition.OR, operation2));

            return operaion1;
        }
    }

    public enum ECondition { None, AND, OR };
}