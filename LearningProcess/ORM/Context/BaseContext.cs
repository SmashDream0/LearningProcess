using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.Repository;
using System.Reflection;
using Dapper;
using LearningProcess.ORM;
using Dapper.Contrib.Extensions;

namespace LearningProcess.ORM.Context
{
    public abstract class BaseContext : IContext
    {
        public BaseContext(EContextType contextType)
        {
            ContextType = contextType;
        }

        static BaseContext()
        {
            Initialize();
        }

        public EContextType ContextType { get; private set; }

        private static Dictionary<Type, Table.Table> _typeTables = new Dictionary<Type, Table.Table>();

        protected static IEnumerable<Table.Table> Tables
        { get; private set; }

        public abstract IEnumerable<T> Query<T>(Specification.BaseSpecification specification) where T : class, IEntity;

        public abstract void Delete(Specification.BaseSpecification specification);

        public abstract void Insert<T>(T[] entities) where T : class, IEntity;

        public abstract void Update<T>(T entity) where T : class, IEntity;

        public static Table.Table TableByType(Type type)
        { return _typeTables[type]; }

        private static void Initialize()
        {
            var tables = new List<Table.Table>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    var attribute = GetAttribute<TableAttribute>(type);

                    if (attribute != null)
                    {
                        var table = new Table.Table(type, attribute.Name);

                        SqlMapper.SetTypeMap(type, new CustomPropertyTypeMap(
                            type, (tp, columnName) =>
                            { return table.ColumnByNameDB(columnName).PropertyInfo; }));

                        tables.Add(table);

                        _typeTables.Add(type, table);
                    }
                }
            }

            Tables = tables.ToArray();
        }

        private static T GetAttribute<T>(Type type)
            where T : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(T), true);

            if (attributes != null)
            {
                switch (attributes.Length)
                {
                    case 0:
                        break;
                    case 1:
                        return (T)attributes[0];
                    default:
                        throw new Exception($"Need only one attribute type: {typeof(T).Name}");
                }
            }

            return null;
        }
    }
}