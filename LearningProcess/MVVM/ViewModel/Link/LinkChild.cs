using LearningProcess.MVVM.ViewModel.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LearningProcess.MVVM.ViewModel.Link
{
    public abstract class LinkChild : BaseNotifier, ISettings
    {
        public LinkChild()
        {
            ViewModelSettings = new ViewModelSettings();

            GotoLinkCommand = new Command(GotoLink);
        }

        /// <summary>
        /// Экран только для чтения или нет
        /// </summary>
        public bool IsReadOnly => ViewModelSettings.IsReadOnly;

        /// <summary>
        /// Не отображать, если программа в режиме только для чтения
        /// </summary>
        public Visibility VisibilityShow => (Program.SettingsInstance.IsReadOnly ? Visibility.Collapsed : Visibility.Visible);

        /// <summary>
        /// Отображать, если программа в режиме только для чтения
        /// </summary>
        public Visibility VisibilityHide => (Program.SettingsInstance.IsReadOnly ? Visibility.Visible : Visibility.Collapsed);

        /// <summary>
        /// Настройки экрана
        /// </summary>
        public ViewModelSettings ViewModelSettings
        { get; private set; }

        /// <summary>
        /// Клавный экрана
        /// </summary>
        public LinkViewModel MainLink => Binds.DI.GetInstance<View.LinkWindow>() as LinkViewModel;

        /// <summary>
        /// Экран для отображения
        /// </summary>
        public Control Control
        { get; set; }

        /// <summary>
        /// Имя экрана
        /// </summary>
        public abstract string Name
        { get; }

        /// <summary>
        /// Кнопка перейти по ссылке экранов
        /// </summary>
        public ICommand GotoLinkCommand
        { get; private set; }

        /// <summary>
        /// Обновить визуальное представление отображения
        /// </summary>
        public virtual void UpdateControl()
        {
            Control.DataContext = null;
            Control.DataContext = this;
            Control.UpdateLayout();
        }

        private void GotoLink()
        { MainLink.GotoLink(this); }
    }
}
