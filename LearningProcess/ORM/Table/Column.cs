using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.ORM.Table
{
    public class Column
    {
        public Column(PropertyInfo propertyInfo, string nameDB, int length, bool isPrimary)
        {
            PropertyInfo = propertyInfo;
            NameDB = nameDB;
            Length = length;
            IsPrimary = isPrimary;
        }

        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// Имя свойства
        /// </summary>
        public string Name => PropertyInfo.Name;

        public object GetValue(IEntity entity)
        { return PropertyInfo.GetValue(entity); }

        /// <summary>
        /// Типа
        /// </summary>
        public Type Type => PropertyInfo.PropertyType;

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

        public override string ToString()
        {
            return $"{Name} {PropertyInfo.PropertyType.Name}{(IsPrimary ? "-primary" : String.Empty)}{(Length == 0 ? String.Empty: $"({Length})")}";
        }
    }
}
