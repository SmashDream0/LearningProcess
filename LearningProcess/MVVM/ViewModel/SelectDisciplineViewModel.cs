using LearningProcess.MVVM.ViewModel.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearningProcess.MVVM.ViewModel
{
    public class SelectDisciplineViewModel : BaseNotifier
    {
        public SelectDisciplineViewModel(Model.DisciplineModel disciplineModel)
        {
            Items = disciplineModel.GetItems(new ViewModelSettings()).Select(x => new Item() { Key = x.Key, Name = x.Name }).ToArray();

            OkCommand = new Command(Ok, CanOk);
            CancelCommand = new Command(Cancel);
        }

        private bool? _dialogResult;
        private Item _selectedItem;
        private Item _savedSelectedItem;

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                _savedSelectedItem = value;
                propertyChanged("SelectedItem");
                OkCommand.UpdateCanExecute();
            }
        }

        public IEnumerable<Item> Items
        { get; private set; }

        public bool? DialogResult
        {
            get => _dialogResult;
            set
            {
                _dialogResult = value;
                propertyChanged("DialogResult");
            }
        }

        public IExecuteCommand OkCommand
        { get; private set; }
        public ICommand CancelCommand
        { get; private set; }

        private void Ok()
        {
            _selectedItem = _savedSelectedItem;
            DialogResult = true;
        }
        private bool CanOk()
        { return _savedSelectedItem != null; }

        private void Cancel()
        {
            DialogResult = true;
        }

        public class Item
        {
            public int Key { get; set; }
            public string Name{ get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}