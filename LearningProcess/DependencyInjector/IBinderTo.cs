using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.DependencyInjector
{
    public interface IBinderTo
    {
        /// <summary>
        /// Привязать один тип к другому
        /// </summary>
        /// <param name="isSingleInstance">Использовать единственный экземпляр</param>
        /// <typeparam name="T">Тип привязки к</typeparam>
        /// <returns></returns>
        SimpleDI To<T>(bool isSingleInstance = false);

        /// <summary>
        /// Привязать к конкретному экземпляру
        /// </summary>
        /// <param name="instance">Экземпляр</param>
        /// <returns></returns>
        SimpleDI To(object instance);
    }
}
