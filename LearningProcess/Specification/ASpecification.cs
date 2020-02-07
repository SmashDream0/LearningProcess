using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Specification
{
    public abstract class ASpecification<T> : AData
    {
        public ASpecification(AData innerOperation)
        { _innerOperation = innerOperation; }

        public override string Data
        {
            get
            {
                var sb = new StringBuilder();
                
                AddWhere(sb, this);

                return sb.ToString();
            }
        }

        public static string Querry(ASpecification<T> spec)
        {
            var sb = new StringBuilder(spec._innerOperation.Data);

            AddSelect(sb); sb.Append(' ');
            AddTableName(sb); sb.Append(" WHERE ");
            AddWhere(sb, spec);

            return sb.ToString();
        }

        private static Dictionary<Type, string> _selectDictionary = new Dictionary<Type, string>();

        private static void AddSelect(StringBuilder sb)
        {
            sb.Append("SELECT ");

            if (!_selectDictionary.ContainsKey(typeof(T)))
            {
                var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .Where(x => x.GetMethod != null && x.SetMethod != null)
                    .Select(x => x.Name)
                    .ToArray();

                var txtProps = String.Join(",", props);

                _selectDictionary.Add(typeof(T), txtProps);
            }

            sb.Append(_selectDictionary[typeof(T)]);
        }

        private static void AddTableName(StringBuilder sb)
        {
            sb.Append("FROM ["); sb.Append(typeof(T).Name); sb.Append("]");
        }

        private static void AddWhere(StringBuilder sb, ASpecification<T> spec)
        {
            bool needScobe = spec._specifications.Count > 1;

            if (needScobe)
            { sb.Append('('); }

            sb.Append(spec._innerOperation);

            foreach (var specification in spec._specifications)
            {
                switch (specification.Key)
                {
                    case EOperation.AND:
                        sb.Append("AND ");
                        break;
                    case EOperation.OR:
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

        protected AData _innerOperation { get; private set; }

        private List<KeyValuePair<EOperation, ASpecification<T>>> _specifications = new List<KeyValuePair<EOperation, ASpecification<T>>>();

        public enum EOperation { None, AND, OR };

        public static ASpecification<T> operator &(ASpecification<T> operaion1, ASpecification<T> operation2)
        {
            operaion1._specifications.Add(new KeyValuePair<EOperation, ASpecification<T>>(EOperation.AND, operation2));

            return operaion1;
        }

        public static ASpecification<T> operator |(ASpecification<T> operaion1, ASpecification<T> operation2)
        {
            operaion1._specifications.Add(new KeyValuePair<EOperation, ASpecification<T>>(EOperation.OR, operation2));

            return operaion1;
        }
    }
}