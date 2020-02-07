using LearningProcess.MVVM.Model;
using LearningProcess.MVVM.ViewModel.Editors;
using LearningProcess.MVVM.ViewModel.Misc;
using LearningProcess.ORM;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System;
using System.Windows;
using LearningProcess.MVVM.ViewModel.Item;

namespace LearningProcess.MVVM.ViewModel.Items
{
    public abstract class BaseItemsViewModel<TEntity, TWindowEditor, TEntityViewModel> : Link.LinkChild, INotifyPropertyChanged, ISettings
        where TEntity : class, IEntity, new()
        where TWindowEditor : Window
        where TEntityViewModel : AItemViewModel<TEntity>
    {
        public BaseItemsViewModel(AModel<TEntity, TEntityViewModel> entityModel)
        {
            var settingName = typeof(TEntity).Name;

            if (!Program.SettingsInstance.BlockSize.ContainsKey(settingName))
            { Program.SettingsInstance.BlockSize.Add(settingName, 100); }

            _entityModel = entityModel;
            Initialize();
        }

        private readonly AModel<TEntity, TEntityViewModel> _entityModel;
        private AItem _selectedItem;
        private int _maxColumnCount;
        private double _windowWidth;
        private double _wndowHeight;
        private bool _afterAdd = false;

        /// <summary>
        /// Событие изменения выделенного элемента
        /// </summary>
        public event Action<TEntityViewModel> OnSelectedItemChanged;

        /// <summary>
        /// Размер стороны блока
        /// </summary>
        public double BlockSize
        {
            get => Program.SettingsInstance.BlockSize[typeof(TEntity).Name];
            set
            {
                Program.SettingsInstance.BlockSize[typeof(TEntity).Name] = value;
                propertyChanged("BlockSize");
                UpdatemaxItemsColumn();
            }
        }

        /// <summary>
        /// Максимальное количество плиток по ширине
        /// </summary>
        public int MaxColumnCount
        {
            get => _maxColumnCount;
            set
            {
                var itemsCount = GetItemsCount();

                if (itemsCount > 0)
                {
                    if (value > itemsCount)
                    { value = itemsCount; }
                }
                else
                { value = 1; }

                if (_afterAdd || MaxColumnCount != value)
                {
                    _maxColumnCount = value;
                    //FillEmpties();
                    propertyChanged("MaxColumnCount");
                    _afterAdd = false;
                }
            }
        }

        /// <summary>
        /// Ширина экрана
        /// </summary>
        public double WindowWidth
        {
            get => _windowWidth;
            set
            {
                _windowWidth = value;
                propertyChanged("WindowWidth");
                UpdatemaxItemsColumn();
            }
        }

        /// <summary>
        /// Высота экрана
        /// </summary>
        public double WindowHeight
        {
            get => _wndowHeight;
            set
            {
                _wndowHeight = value;
                propertyChanged("WindowHeight");
            }
        }

        /// <summary>
        /// Выбранная сущность
        /// </summary>
        public AItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                propertyChanged("SelectedItem");
                EditCommand.UpdateCanExecute();
                DeleteCommand.UpdateCanExecute();

                OnSelectedItemChanged?.Invoke(SelectedItem as TEntityViewModel);
            }
        }

        /// <summary>
        /// Список сущностей
        /// </summary>
        public ObservableCollection<AItem> Items
        { get; private set; }

        /// <summary>
        /// Команда добавления
        /// </summary>
        public ICommand AddCommand
        { get; protected set; }

        /// <summary>
        /// Команда удаления
        /// </summary>
        public IExecuteCommand DeleteCommand
        { get; protected set; }

        /// <summary>
        /// Команда изменения
        /// </summary>
        public IExecuteCommand EditCommand
        { get; protected set; }

        /// <summary>
        /// Обновить список элементов
        /// </summary>
        public void UpdateItems()
        {
            var items = _entityModel.GetItems(ViewModelSettings);

            Items.Clear();

            foreach (var item in items)
            { Items.Add(item); }

            SelectedItem = null;

            EditCommand.UpdateCanExecute();
            DeleteCommand.UpdateCanExecute();
        }

        /// <summary>
        /// Обновить визуальное представление
        /// </summary>
        public override void UpdateControl()
        {
            UpdatemaxItemsColumn();
            base.UpdateControl();
        }

        /// <summary>
        /// Инициализировать
        /// </summary>
        protected virtual void Initialize()
        {
            Items = new ObservableCollection<AItem>();
            AddCommand = new Command(Add, CanAdd);
            DeleteCommand = new Command(Delete, CanDelete);
            EditCommand = new Command(Edit, CanEdit);

            UpdateItems();
        }

        protected void Add()
        {
            var view = WindowManager.GetWindow<TWindowEditor>(_entityModel, ViewModelSettings);

            view.ShowDialog();

            var viewModel = view.DataContext as BaseEditViewModel<TEntity, TEntityViewModel>;

            if (viewModel.IsEdited)
            {
                var itemCount = GetItemsCount();

                for (int c = itemCount; c < Items.Count;)
                { Items.RemoveAt(c); }

                Items.Add(viewModel.EntityViewModel);
                _afterAdd = true;

                UpdatemaxItemsColumn();
            }
        }

        protected void Delete()
        {
            var item = SelectedItem as TEntityViewModel;

            if (item != null)
            {
                _entityModel.Remove(item);
                Items.Remove(SelectedItem);
                SelectedItem = null;
                DeleteCommand.UpdateCanExecute();
                EditCommand.UpdateCanExecute();

                UpdatemaxItemsColumn();
            }
        }

        protected void Edit()
        {
            var item = SelectedItem as TEntityViewModel;

            if (item != null)
            {
                var view = WindowManager.GetWindow<TWindowEditor>(_entityModel, item);

                view.ShowDialog();
            }
        }

        private bool CanAdd()
        { return !ViewModelSettings.IsReadOnly; }

        private bool CanEdit()
        { return SelectedItem != null && !ViewModelSettings.IsReadOnly && SelectedItem as TEntityViewModel != null; }

        private bool CanDelete()
        { return CanEdit(); }

        private void UpdatemaxItemsColumn()
        { MaxColumnCount = (int)((WindowWidth - 25) / (BlockSize + 20)); }

        private void FillEmpties()
        {
            var itemsCount = GetItemsCount();

            if (itemsCount == 0)
            {
                Items.Clear();

                var newEmptyElement = new EmptyItem();

                newEmptyElement.OnClick += (ei) =>
                {
                    if (!IsReadOnly)
                    {
                        Add();

                        FillEmpties();
                    }
                };

                Items.Add(newEmptyElement);
            }
            else
            {
                var trueElements = (int)Math.Ceiling((decimal)itemsCount / MaxColumnCount) * MaxColumnCount;
                var factElements = Items.Count;

                if (trueElements > factElements)
                {//добавляю пустых
                    for (int i = trueElements - factElements; i > 0; i--)
                    {
                        var newEmptyElement = new EmptyItem();

                        newEmptyElement.OnClick += (ei) =>
                        {
                            if (!IsReadOnly)
                            {
                                Add();

                                FillEmpties();
                            }
                        };

                        Items.Add(newEmptyElement);
                    }
                }
                else if (trueElements < factElements)
                {//удаляю пустых
                    for (int i = factElements - trueElements; i > 0; i--)
                    { Items.RemoveAt(Items.Count - 1); }
                }
            }
        }

        private int GetItemsCount()
        {
            int count = 0;

            foreach (var item in Items)
            {
                if ((item as TEntityViewModel) == null)
                { break; }

                count++;
            }

            return count;
        }
    }
}