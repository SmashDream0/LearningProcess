using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.ORM.Context
{
    public enum EContextType
    {
        /// <summary>
        /// Источник данных - оперативная память
        /// </summary>
        Local,
        /// <summary>
        /// Источник данных - файл
        /// </summary>
        File
    }
}