using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Table
{
    class Table
    {
        public Table(Type type)
        {
            _type = type;
            _columnDictionary = new Dictionary<string, Column>();

            //TODO получить атрибуты с колонками
            var columns = new List<Column>();
            var props = type.GetProperties();

            foreach (var prop in props)
            {

            }

            foreach (var column in columns)
            { _columnDictionary.Add(column.Name, column); }

            Columns = columns.ToArray();
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

        /// <summary>
        /// Колонки
        /// </summary>
        public IEnumerable<Column> Columns { get; private set; }

        /// <summary>
        /// Получить колонку по имени свойства
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns></returns>
        public Column FindColumn(string propertyName)
        { return _columnDictionary[propertyName]; }
    }
}
