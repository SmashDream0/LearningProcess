using LearningProcess.ORM;
using LearningProcess.MVVM.ViewModel;
using LearningProcess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.MVVM.ViewModel.Item;

namespace LearningProcess.MVVM.Model
{
    public abstract class AModel<TEntity, TEntityViewModel>
        where TEntity: class, IEntity
        where TEntityViewModel : AItemViewModel<TEntity>
    {
        public AModel(BaseRepository<TEntity> repository)
        {
            _repository = repository;
            DefaultSpecification = null;
        }

        private readonly BaseRepository<TEntity> _repository;

        public ORM.Specification.BaseGenericSpecification<TEntity> DefaultSpecification { get; set; }
        
        /// <summary>
        /// Добавить сущность
        /// </summary>
        /// <param name="itemViewModel">Сущность</param>
        public void Add(TEntityViewModel itemViewModel)
        { _repository.Insert(itemViewModel.GetEntity()); }

        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <param name="itemViewModel">Сущность</param>
        public void Remove(TEntityViewModel itemViewModel)
        { _repository.Remove(itemViewModel.GetEntity()); }

        /// <summary>
        /// Обновить сущность
        /// </summary>
        /// <param name="itemViewModel">Сущность</param>
        public void Update(TEntityViewModel itemViewModel)
        { _repository.Update(itemViewModel.GetEntity()); }

        /// <summary>
        /// Получить список сущностей
        /// </summary>
        /// <param name="viewModelSettings"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntityViewModel> GetItems(ViewModelSettings viewModelSettings)
        {
            var result = _repository.Find(DefaultSpecification);

            return result.Select(x => Activator.CreateInstance(typeof(TEntityViewModel), new object[] { x, viewModelSettings }) as TEntityViewModel).ToArray();
        }
    }
}
