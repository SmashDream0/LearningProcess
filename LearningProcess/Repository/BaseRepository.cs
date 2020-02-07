using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.ORM.Context;
using Dapper;
using LearningProcess.ORM;
using LearningProcess.ORM.Table;
using LearningProcess.ORM.Specification;
using LearningProcess.Specification;

namespace LearningProcess.Repository
{
    public abstract class BaseRepository<T>
        where T : class, IEntity
    {
        public BaseRepository(IContext context)
        {
            this.Context = context;
            this._cache = new Dictionary<int, T>();
        }

        public IContext Context { get; internal set; }

        protected Dictionary<int, T> _cache;

        protected virtual void AddToCache(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                if (_cache.ContainsKey(entity.Key))
                { _cache[entity.Key] = entity; }
                else
                { _cache.Add(entity.Key, entity); }
            }
        }

        protected virtual void RemoveFromCache(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                if (_cache.ContainsKey(entity.Key))
                { _cache.Remove(entity.Key); }
            }
        }

        /// <summary>
        /// Выбрать все записи таблицы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> All()
        { return Find(null); }

        /// <summary>
        /// Выбрать записи по спецификациям
        /// </summary>
        /// <param name="specification">Спецификация</param>
        /// <returns></returns>
        public IEnumerable<T> Find(BaseGenericSpecification<T> specification = null)
        {
            if (specification == null)
            { specification = new BaseGenericSpecification<T>(null); }

            IEnumerable<T> result = Context.Query<T>(specification);

            AddToCache(result);

            return result;
        }

        /// <summary>
        /// Выбрать первую запись соответствующую спецификациям
        /// </summary>
        /// <param name="specification">Спецификация</param>
        /// <returns></returns>
        public T FirstOrDefault(BaseGenericSpecification<T> specification = null)
        {
            T result = Context.Query<T>(specification).FirstOrDefault();

            if (result != null)
            { AddToCache(new[] { result }); }

            return result;
        }

        /// <summary>
        /// Проверить существуют ли записи по спецификациям
        /// </summary>
        /// <param name="specification">Спецификация</param>
        /// <returns></returns>
        public bool Exist(BaseGenericSpecification<T> specification = null)
        {
            bool result;

            result = !Comparer<T>.Equals(default(T), FirstOrDefault(specification));

            return result;
        }

        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(T entity)
        {
            Context.Insert<T>(new[] { entity });

            AddToCache(new[] { entity });
        }

        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            Context.Delete(new ByKey<T>(entity.Key));

            RemoveFromCache(new[] { entity });
        }

        /// <summary>
        /// Обновить запись
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        { Context.Update<T>(entity); }
    }
}