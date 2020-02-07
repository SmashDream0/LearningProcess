using LearningProcess.MVVM.ViewModel.Misc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LearningProcess.MVVM.ViewModel.Editors
{
    public class DisciplineEditViewModel : BaseEditViewModel<Entity.Discipline, DisciplineViewModel>
    {
        public DisciplineEditViewModel(Model.DisciplineModel disciplineModel, DisciplineViewModel discipline)
            : base(disciplineModel, discipline)
        { }

        public DisciplineEditViewModel(Model.DisciplineModel disciplineModel, ViewModelSettings viewModelSettings)
            : base(disciplineModel, viewModelSettings)
        { }

        private BitmapImage _image;

        /// <summary>
        /// Картинка
        /// </summary>
        public BitmapImage Image
        {
            get => _image;
            set
            {
                _image = value;
                propertyChanged("Image");
            }
        }

        /// <summary>
        /// Команда выбора изображения
        /// </summary>
        public ICommand SelectFileCommand
        { get; private set; }

        protected override void Initialize(Action continueAction)
        {
            base.Initialize(continueAction);

            Name = EntityViewModel.Name;
            Image = EntityViewModel.Image;

            SelectFileCommand = new Command(SelectFile);
        }

        protected override void Save()
        {
            EntityViewModel.Name = Name;
            EntityViewModel.Image = Image;
        }

        private void SelectFile()
        {
            var ofd = new OpenFileDialog();

            ofd.Title = "Выберите картинку";

            ofd.Filter = "Image files|*.bmp;*.jpg;*.png";

            if (ofd.ShowDialog().Value)
            {
                using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    Image = DisciplineViewModel.ToImage(fs);
                }
            }
        }
    }
}
