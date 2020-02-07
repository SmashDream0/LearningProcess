using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearningProcess.MVVM.ViewModel
{
    public abstract class BaseItemsViewModel<T>
        where T:Entity.AModel
    {
        public T SelectedItem { get; set; }

        public abstract IEnumerable<T> Items { get; }

        public ICommand AddItemCommand { get; protected set; }
        public ICommand DeleteItemCommand { get; protected set; }
    }
}
