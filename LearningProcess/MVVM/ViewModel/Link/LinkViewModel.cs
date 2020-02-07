using LearningProcess.MVVM.ViewModel.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LearningProcess.MVVM.ViewModel.Link
{
    public class LinkViewModel : Misc.BaseNotifier
    {
        public LinkViewModel(LinkChild firstChild)
        {
            Links = new ObservableCollection<LinkChild>();
            BackCommand = new Command(CloseLast);

            AddLink(firstChild);
        }

        public string Title => GetTitle();

        public string BackImage => GetBackImage();

        public ObservableCollection<LinkChild> Links
        { get; private set; }

        public LinkChild CurrentLink 
            => Links.LastOrDefault();

        public ICommand BackCommand
        { get;private set; }

        public double MinHeight
        { get => (CurrentLink == null ? 0 : CurrentLink.Control.MinHeight) + 100; }

        public double MinWidth
        { get => (CurrentLink == null ? 0 : CurrentLink.Control.MinWidth); }

        public Visibility BackVisibility => (Links.Count == 1 ? Visibility.Collapsed : Visibility.Visible);

        /// <summary>
        /// Добавить родителя
        /// </summary>
        /// <param name="control"></param>
        public void AddLink(LinkChild linkChild)
        {
            Links.Add(linkChild);
            propertyChanged("BackVisibility");
            propertyChanged("CurrentLink");
            propertyChanged("CurrentLink.Control");
            propertyChanged("MinHeight");
            propertyChanged("MinWidth");
        }

        public void GotoLink(object link)
        {
            while (CurrentLink != link)
            { CloseLast(); }
        }

        /// <summary>
        /// Удалить последний экран
        /// </summary>
        public void CloseLast()
        {
            if (Links.Any())
            {
                Links.RemoveAt(Links.Count - 1);
                propertyChanged("BackVisibility");
                propertyChanged("CurrentLink");
                propertyChanged("CurrentLink.Control");
                propertyChanged("MinHeight");
                propertyChanged("MinWidth");

                if (Links.Any())
                { Links.Last().UpdateControl(); }
                else
                { WindowManager.CloseLastWindow(); }
            }
        }

        private string GetTitle()
        {
            if (Program.SettingsInstance.IsReadOnly)
            {
                var repository = Binds.DI.GetInstance<Repository.DisciplineRepository>() as Repository.DisciplineRepository;
                var discipline = repository.FirstOrDefault(new Specification.Discipline.ByKey(Program.SettingsInstance.DisciplineKey));

                return discipline.Name;
            }
            else
            { return "Электронный учебно-методический комплекс"; }
        }

        private string GetBackImage()
        {
            if (Program.SettingsInstance.IsReadOnly)
            {
                var repository = Binds.DI.GetInstance<Repository.DisciplineRepository>() as Repository.DisciplineRepository;
                var discipline = repository.FirstOrDefault(new Specification.Discipline.ByKey(Program.SettingsInstance.DisciplineKey));

                var path = Path.GetTempPath() + "\\" + Guid.NewGuid().ToString() + ".png";

                using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fs.SetLength(0);
                    fs.Write(discipline.Image, 0, discipline.Image.Length);
                }

                return path;
            }
            else
            { return "/LearningProcess;component/Resources/background.jpg"; }
        }
    }
}