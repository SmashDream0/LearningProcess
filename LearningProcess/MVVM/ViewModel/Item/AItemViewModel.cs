using LearningProcess.MVVM.ViewModel.Misc;
using LearningProcess.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearningProcess.MVVM.ViewModel.Item
{
    public abstract class AItemViewModel<TEntity> : AItem, ISettings
        where TEntity : class, IEntity
    {
        public AItemViewModel(TEntity entity, ViewModelSettings viewModelSettings) : this(viewModelSettings)
        { Entity = entity; }

        private AItemViewModel(ViewModelSettings viewModelSettings)
        {
            ViewModelSettings = viewModelSettings;

            PreInitialization();

            ItemDoubleClickCommand = new Command(() =>
            {//чтобы можно было динамически менять параметры

                if (ViewModelSettings.AllowDoubleClick)
                {
                    if (ViewModelSettings.ElementDoubleClick != null)
                    { ViewModelSettings.ElementDoubleClick(this.Entity); }
                    else
                    { DoubleClickedInner(); }
                }
            });
        }

        /// <summary>
        /// Режим только для чтения
        /// </summary>
        public override bool IsReadOnly => ViewModelSettings.IsReadOnly;

        /// <summary>
        /// Ключ сущности
        /// </summary>
        public int Key => Entity.Key;

        /// <summary>
        /// Сущность
        /// </summary>
        protected TEntity Entity
        { get; private set; }

        /// <summary>
        /// Двойное нажатие мышки
        /// </summary>
        public ICommand ItemDoubleClickCommand
        { get; private set; }

        /// <summary>
        /// Настройки экрана
        /// </summary>
        public ViewModelSettings ViewModelSettings
        { get; private set; }

        /// <summary>
        /// Получить сущность, с измененными значениями
        /// </summary>
        /// <returns></returns>
        public abstract TEntity GetEntity();

        protected virtual void PreInitialization()
        { }

        protected abstract void DoubleClickedInner();

        protected override void Clicked()
        {
            if (ViewModelSettings.AllowSingleClick)
            {
                if (ViewModelSettings.ElementClick != null)
                { ViewModelSettings.ElementClick(this.Entity); }
                else
                { ClickedInner(); }
            }
        }

        protected abstract void ClickedInner();
    }
}