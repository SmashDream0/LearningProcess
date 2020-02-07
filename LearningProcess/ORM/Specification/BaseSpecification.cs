using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.ORM.Specification
{
    public abstract class BaseSpecification: BaseData
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
