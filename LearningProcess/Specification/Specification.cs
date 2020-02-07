using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Specification
{
    public abstract class Specification: AData
    {
        /// <summary>
        /// Запрос на выборку
        /// </summary>
        public abstract string SelectQuery { get; }

        /// <summary>
        /// Запрос на удаление
        /// </summary>
        public abstract string DeleteQuery { get; }
    }
}
