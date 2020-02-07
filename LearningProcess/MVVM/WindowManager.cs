using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LearningProcess.MVVM
{
    public static class WindowManager
    {
        static WindowManager()
        {
            OpenedWindows = new List<System.Windows.Window>();
        }

        /// <summary>
        /// Цепочка открытых экранов
        /// </summary>
        public static List<System.Windows.Window> OpenedWindows
        { get; private set; }

        /// <summary>
        /// Получить экран и занести в него контекст
        /// </summary>
        /// <typeparam name="T">Тип экрана</typeparam>
        /// <param name="parameters">Параметры конструктора для viewModel</param>
        /// <returns></returns>
        public static T GetWindow<T>(params object[] parameters)
            where T : System.Windows.Window
        {
            var window = Activator.CreateInstance<T>();

            var viewModel = Binds.DI.GetInstance<T>(parameters);

            window.DataContext = viewModel;

            window.Closed += (s, e) => { OpenedWindows.Remove(window); };

            OpenedWindows.Add(window);

            return window;
        }

        /// <summary>
        /// Получить контрол и занести в него контекст
        /// </summary>
        /// <typeparam name="T">Тип экрана</typeparam>
        /// <param name="parameters">Параметры конструктора для viewModel</param>
        /// <returns></returns>
        public static ViewModel.Link.LinkChild GetLinkControl<T>(params object[] parameters)
            where T : Control
        {
            var window = Activator.CreateInstance<T>();

            var viewModel = Binds.DI.GetInstance<T>(parameters) as ViewModel.Link.LinkChild;

            viewModel.Control = window;
            window.DataContext = viewModel;

            return viewModel;
        }

        /// <summary>
        /// Закрыть последний экран
        /// </summary>
        public static void CloseLastWindow()
        {
            if (OpenedWindows.Any())
            { OpenedWindows.Last().Close(); }
        }

        /// <summary>
        /// Показать ветку экранов только для чтения
        /// </summary>
        /// <returns></returns>
        public static Window GetReadOnlyWindow()
        { return GetMainWindow<View.DisciplinesListView>(); }

        /// <summary>
        /// Показать ветку экранов для редактирования и чтения
        /// </summary>
        /// <returns></returns>
        public static Window GetEditWindow()
        { return GetMainWindow<View.MainView>(); }

        public static Window GetMainWindow<TControl>(params object[] parameters)
            where TControl: Control
        {
            var childLink = GetLinkControl<TControl>(parameters);

            var linkWindow = GetWindow<View.LinkWindow>(childLink);

            return linkWindow;
        }
    }
}
