using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LearningProcess.ORM.Context
{
    public interface IContext
    {
        /// <summary>
        /// Тип контекста
        /// </summary>
        EContextType ContextType { get; }

        /// <summary>
        /// Запрос возвращающий набор сущностей
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="specification">Спецификация</param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(Specification.BaseSpecification specification) where T : class, IEntity;

        /// <summary>
        /// Добавить сущности в таблицу
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="entities"></param>
        void Insert<T>(T[] entities) where T : class, IEntity;

        /// <summary>
        /// Удалить сущности по условию
        /// </summary>
        /// <param name="specification">Супификация</param>
        void Delete(Specification.BaseSpecification specification);

        /// <summary>
        /// Изменить сущности в таблице
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="entity">Сущност</param>
        void Update<T>(T entity) where T : class, IEntity;
    }
}