using LearningProcess.MVVM.Model;
using LearningProcess.MVVM.ViewModel.Item;
using LearningProcess.MVVM.ViewModel.Misc;
using LearningProcess.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearningProcess.MVVM.ViewModel.Editors
{
    public abstract class BaseEditViewModel<TEntity, TEntityViewModel> : BaseNotifier, ISettings
        where TEntity : class, IEntity, new()
        where TEntityViewModel : AItemViewModel<TEntity>
    {
        /// <summary>
        /// Конструктор для добавления
        /// </summary>
        public BaseEditViewModel(AModel<TEntity, TEntityViewModel> entityModel, ViewModelSettings viewModelSettings)
        {
            ViewModelSettings = viewModelSettings;
            EntityModel = entityModel;
            Mode = EMode.Add;
            ContinueButtonName = "Добавить";

            var entity = Activator.CreateInstance<TEntity>();

            EntityViewModel = Activator.CreateInstance(typeof(TEntityViewModel), new object[] { entity, ViewModelSettings }) as TEntityViewModel;
            Initialize(Add);
        }

        /// <summary>
        /// Конструктор для изменения
        /// </summary>
        /// <param name="entity"></param>
        public BaseEditViewModel(AModel<TEntity, TEntityViewModel> entityModel, TEntityViewModel entityViewModel)
        {
            ViewModelSettings = entityViewModel.ViewModelSettings;
            EntityModel = entityModel;
            Mode = EMode.Edit;
            ContinueButtonName = "Сохранить";
            EntityViewModel = entityViewModel;
            Initialize(Continue);
        }

        private string _name;

        /// <summary>
        /// Объект бизнес логики
        /// </summary>
        protected AModel<TEntity, TEntityViewModel> EntityModel
        { get; private set; }

        /// <summary>
        /// Объект настройки экрана
        /// </summary>
        public ViewModelSettings ViewModelSettings
        { get; private set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                propertyChanged("Name");
            }
        }

        /// <summary>
        /// Режим работы
        /// </summary>
        public EMode Mode
        { get; private set; }

        /// <summary>
        /// Изменения подтверждены
        /// </summary>
        public bool IsEdited
        { get; private set; }

        /// <summary>
        /// Редактируемая view model сущности
        /// </summary>
        public TEntityViewModel EntityViewModel
        { get; private set; }

        /// <summary>
        /// Имя кнопки изменения
        /// </summary>
        public string ContinueButtonName
        { get; private set; }

        /// <summary>
        /// Кнопка добавить/сохранить
        /// </summary>
        public ICommand ContinueCommand
        { get; private set; }

        /// <summary>
        /// Кнопка отмены
        /// </summary>
        public ICommand CancelCommand
        { get; private set; }

        protected virtual void Initialize(Action continueAction)
        {
            ContinueCommand = new Command(() =>
            {
                continueAction();
                IsEdited = true;
            });
            CancelCommand = new Command(() =>
            {
                Close();
                IsEdited = false;
            });
        }

        protected abstract void Save();

        protected void Add()
        {
            Save();
            EntityModel.Add(EntityViewModel);
            Close();
        }

        protected void Continue()
        {
            Save();
            EntityModel.Update(EntityViewModel);
            Close();
        }

        private void Close()
        { WindowManager.CloseLastWindow(); }
    }
}