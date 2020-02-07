using LearningProcess.MVVM.ViewModel.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearningProcess.MVVM.ViewModel.Item
{
    public abstract class AItem : BaseNotifier
    {
        public AItem()
        {
            ItemClickCommand = new Command(Clicked);
        }

        private string _name;

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
        /// Только для чтения
        /// </summary>
        public abstract bool IsReadOnly
        { get; }

        /// <summary>
        /// Единственное нажатие мышкой
        /// </summary>
        public ICommand ItemClickCommand
        { get; private set; }

        /// <summary>
        /// Событие нажатия на кнопку мышки
        /// </summary>
        protected abstract void Clicked();
    }
}