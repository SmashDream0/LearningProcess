using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.ORM
{
    public interface IEntity
    {
        /// <summary>
        /// Основной ключ
        /// </summary>
        int Key { get; set; }
    }

    /// <summary>
    /// Атрибут длины данных
    /// </summary>
    public class DataLengthAttribute : Attribute
    {
        public DataLengthAttribute(int length)
        { Length = length; }

        /// <summary>
        /// Длина данных
        /// </summary>
        public int Length { get; private set; }
    }
}
