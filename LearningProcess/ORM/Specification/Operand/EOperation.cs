using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.ORM.Specification.Operand
{
    public enum EOperation
    {
        /// <summary>
        /// Равно
        /// </summary>
        Equal,
        /// <summary>
        /// Больше
        /// </summary>
        More,
        /// <summary>
        /// Меньше
        /// </summary>
        Less,
        /// <summary>
        /// Эквивалент Like '%text%'
        /// </summary>
        Like
    };
}