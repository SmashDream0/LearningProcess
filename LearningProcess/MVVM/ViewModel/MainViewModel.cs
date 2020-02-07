using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LearningProcess.MVVM.ViewModel.Misc;
using System.Windows.Controls;
using System.IO;
using System.Threading;
using Microsoft.Win32;
using LearningProcess.MVVM.Model;
using LearningProcess.ORM;

namespace LearningProcess.MVVM.ViewModel
{
    public class MainViewModel : Link.LinkChild
    {
        public MainViewModel()
        {
            _installerModel = new InstallerModel();

            EditDataCommand = new Command(EditData);
            MaterialsReadCommand = new Command(MaterialsRead);
            MakeReadOnlyCopyCommand = new Command(MakeReadOnlyCopy);
        }

        private InstallerModel _installerModel;
        
        /// <summary>
        /// Имя экрана
        /// </summary>
        public override string Name => "Главная страница";

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title
        {
            get
            {
                if (Program.SettingsInstance.IsReadOnly)
                {
                    var repository = Binds.DI.GetInstance<Repository.DisciplineRepository>() as Repository.DisciplineRepository;
                    var discipline = repository.FirstOrDefault(new Specification.Discipline.ByKey(Program.SettingsInstance.DisciplineKey));

                    return "ПО ДИСЦИПЛИНЕ " + discipline.Name;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// Команда открытия экрана редактирования данных
        /// </summary>
        public ICommand EditDataCommand
        { get; private set; }

        /// <summary>
        /// Команда открытия экрана чтения материалов
        /// </summary>
        public ICommand MaterialsReadCommand
        { get; private set; }

        /// <summary>
        /// Сделать копию только для чтения
        /// </summary>
        public ICommand MakeReadOnlyCopyCommand
        { get; private set; }

        private void EditData()
        {
            //var pasWindow = WindowManager.GetWindow<View.PasswordView>();

            //if (pasWindow.ShowDialog().Value)
            {
                var childLink = WindowManager.GetLinkControl<View.DisciplinesView>();

                MainLink.AddLink(childLink);
            }
        }

        private void MaterialsRead()
        {
            if (Program.SettingsInstance.IsReadOnly)
            {
                var repository = Binds.DI.GetInstance<Repository.DisciplineRepository>() as Repository.DisciplineRepository;
                var discipline = repository.FirstOrDefault(new Specification.Discipline.ByKey(Program.SettingsInstance.DisciplineKey));
                ShowReadOnlyView(discipline);
            }
            else
            {
                var childLink = WindowManager.GetLinkControl<View.DisciplinesListView>();

                MainLink.AddLink(childLink);
            }
        }

        private void ShowReadOnlyView(IEntity discipline)
        {
            var materialsModel = Binds.DI.GetInstance<MaterialModel>() as MaterialModel;
            var materialTypeModel = Binds.DI.GetInstance<MaterialTypeModel>() as MaterialTypeModel;

            materialTypeModel.DefaultSpecification = new Specification.MaterialType.ByDiscipline(discipline.Key);

            var link = WindowManager.GetLinkControl<View.MaterialsReadView>(materialsModel, materialTypeModel);

            MainLink.AddLink(link);
        }

        private void MakeReadOnlyCopy()
        {
            var window = WindowManager.GetWindow<View.SelectDisciplineView>();

            window.ShowDialog();

            var viewModel = window.DataContext as SelectDisciplineViewModel;

            if (viewModel.SelectedItem != null)
            {
                Program.SettingsInstance.DisciplineKey = viewModel.SelectedItem.Key;
                Program.SaveSettings();

                var sfd = new SaveFileDialog();

                sfd.Title = "Выберите куда сохранить файл";
                sfd.FileName = $"ЭУМК {viewModel.SelectedItem.Name}.exe";
                sfd.Filter = "Executable|*.exe";

                if (sfd.ShowDialog().Value)
                {
                    MessageBox.Show("Чтобы продолжить программе нужно закрыться." +
                                    "Далее будет видно черное окно с прогрессом, не закрывайте его." +
                                    "Потом программа сново запустится");

                    _installerModel.MakeSingleExecutable(sfd.FileName);
                }
            }
        }
    }
}