using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.ORM.Specification
{
    public abstract class BaseData
    {
        /// <summary>
        /// Строковое представление
        /// </summary>
        public abstract string Data { get; }
    }
}
