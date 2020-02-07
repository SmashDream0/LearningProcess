using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearningProcess.MVVM.ViewModel.Misc
{
    public interface IExecuteCommand
        : ICommand
    {
        bool CanExecuteLast { get; set; }
        event Action<object, CanExecuteEventArg> OnCanExecute;
        void UpdateCanExecute();
    }

    public class TypedEventArg<T>
    {
        public TypedEventArg(object sender, T value)
            : this()
        {
            Value = value;
            Sender = sender;
        }
        public TypedEventArg()
        { }

        public T Value { get; set; }
        public object Sender { get; set; }
    }

    public class CanExecuteEventArg
        : TypedEventArg<bool>
    { }

    public class Command : BaseNotifier, ICommand, IExecuteCommand
    {
        public Command(Action action) : this(action, null)
        { }
        public Command(Action action, Func<bool> canExcute)
        {
            this._action = action;
            this._canExcute = canExcute;
        }

        public event Action<object, CanExecuteEventArg> OnCanExecute;
        public event EventHandler CanExecuteChanged;

        bool _canExecuteLast;
        public bool CanExecuteLast
        {
            get { return _canExecuteLast; }
            set
            {
                _canExecuteLast = value;
                propertyChanged("CanExecuteLast");
            }
        }

        protected Action _action;
        private Func<bool> _canExcute;
        public bool CanExecute(object parameter)
        {
            if (_canExcute == null)
            { return true; }
            else
            { return _canExcute(); }
        }
        public void Execute(object parameter)
        {
            _action();
        }

        public void UpdateCanExecute()
        {
            CanExecute(null);
            if (CanExecuteChanged != null)
            { CanExecuteChanged(this, EventArgs.Empty); }
        }
    }
}