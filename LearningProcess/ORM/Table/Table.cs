using Dapper.Contrib.Extensions;
using LearningProcess.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.ORM.Table
{
    public class Table
    {
        public Table(Type type, string tableNameDB)
        {
            NameDB = tableNameDB;
            _type = type;
            _columnDictionary = new Dictionary<string, Column>();
            _columnDBDictionary = new Dictionary<string, Column>();

            //TODO получить атрибуты с колонками
            var columns = new List<Column>();
            var props = type.GetProperties();

            foreach (var property in props)
            {
                bool isPrimary;
                int length;
                string nameDB;

                {
                    var attribute = GetAttribute<ColumnAttribute>(property);

                    nameDB = (attribute == null ? null : attribute.Name);
                }

                {
                    var attribute = GetAttribute<KeyAttribute>(property);

                    isPrimary = (attribute != null);
                }

                {
                    var attribute = GetAttribute<DataLengthAttribute>(property);

                    length = (attribute == null ? 0 : attribute.Length);
                }

                if (!String.IsNullOrEmpty(nameDB))
                {
                    CheckParams(property, nameDB, length, isPrimary);

                    var column = new Column(property, nameDB, length, isPrimary);

                    columns.Add(column);
                    _columnDictionary.Add(column.Name, column);
                    _columnDBDictionary.Add(column.NameDB, column);
                }
            }

            Columns = columns.ToArray();
        }

        private static void CheckParams(PropertyInfo propertyInfo, string nameDB, int length, bool isPrimary)
        {
            if ((propertyInfo.PropertyType == typeof(string) || propertyInfo.PropertyType == typeof(byte[])) && length < 1)
            { throw new Exception($"Length({length}) is invalid for property {propertyInfo.Name}"); }
        }

        private static T GetAttribute<T>(PropertyInfo property)
            where T : Attribute
        {
            var attributes = property.GetCustomAttributes(typeof(T), true);

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

        private readonly Type _type;
        
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public string Name => _type.Name;
        
        /// <summary>
        /// Имя таблицы в БД
        /// </summary>
        public string NameDB { get; private set; }

        private readonly Dictionary<string, Column> _columnDictionary;

        private readonly Dictionary<string, Column> _columnDBDictionary;

        /// <summary>
        /// Колонки
        /// </summary>
        public IEnumerable<Column> Columns { get; private set; }

        /// <summary>
        /// Получить колонку по имени свойства
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns></returns>
        public Column ColumnByProperyName(string propertyName)
        { return _columnDictionary[propertyName]; }

        /// <summary>
        /// Получить колонку по имени свойства
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns></returns>
        public Column ColumnByNameDB(string propertyName)
        { return _columnDBDictionary[propertyName]; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
