using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Table
{
    public class Column
    {
        public Column(PropertyInfo propertyInfo, string nameDB, int length, bool isPrimary)
        {
            _propertyInfo = propertyInfo;
            NameDB = nameDB;
            Length = length;
            IsPrimary = isPrimary;
        }

        private readonly PropertyInfo _propertyInfo;

        /// <summary>
        /// Имя свойства
        /// </summary>
        public string Name => _propertyInfo.Name;

        /// <summary>
        /// Имя колонки в БД
        /// </summary>
        public string NameDB { get; private set; }

        /// <summary>
        /// Длина данных
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Является ключем
        /// </summary>
        public bool IsPrimary { get; private set; }
    }
}
