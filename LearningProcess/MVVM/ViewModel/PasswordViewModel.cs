using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Input;
using LearningProcess.MVVM.ViewModel.Misc;
using System.Windows;

namespace LearningProcess.MVVM.ViewModel
{
    public class PasswordViewModel : BaseNotifier
    {
        public PasswordViewModel()
        {
            _passwordInner = Program.SettingsInstance.Password;
            OkCommand = new Command(Ok);
            CancelCommand = new Command(Cancel);
        }

        private string _passwordInner;
        private bool? _dialogResult;

        public bool? DialogResult
        {
            get => _dialogResult;
            set
            {
                _dialogResult = value;
                propertyChanged("DialogResult");
            }
        }

        public string Password
        { get; set; }

        public ICommand OkCommand
        { get; private set; }
        public ICommand CancelCommand
        { get; private set; }

        private void Ok()
        {
            if (String.Equals(Password, _passwordInner))
            { DialogResult = true; }
            else
            { MessageBox.Show("Пароль не верный"); }
        }

        private void Cancel()
        {
            DialogResult = false;
        }
    }
}