using LearningProcess.MVVM.ViewModel.Misc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearningProcess.MVVM.ViewModel.Editors
{
    public class MaterialEditViewModel : BaseEditViewModel<Entity.Material, MaterialViewModel>
    {
        public MaterialEditViewModel(Model.MaterialModel materialModel, MaterialViewModel material)
            : base(materialModel, material)
        {
            Name = this.EntityViewModel.Name;
            FileName = this.EntityViewModel.FileName;
            Body = this.EntityViewModel.Body;
            SelectFileCommand = new Command(SelectFile);
            OpenFileCommand = new Command(() => MaterialViewModel.OpenFile(FileName, Body));
        }
        public MaterialEditViewModel(Model.MaterialModel materialModel, ViewModelSettings viewModelSettings) : base(materialModel, viewModelSettings)
        {
            Name = this.EntityViewModel.Name;
            FileName = this.EntityViewModel.FileName;
            Body = this.EntityViewModel.Body;
            SelectFileCommand = new Command(SelectFile);
            OpenFileCommand = new Command(() => MaterialViewModel.OpenFile(FileName, Body));
        }

        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                propertyChanged("FileName");
            }
        }

        public byte[] Body
        { get; set; }

        public ICommand SelectFileCommand
        { get; private set; }

        public ICommand OpenFileCommand
        { get; private set; }
        
        private void SelectFile()
        {
            var ofd = new OpenFileDialog();

            ofd.Title = "Выберите йа";

            ofd.Filter = "All files|*.*";

            if (ofd.ShowDialog().Value)
            {
                using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    Body = new byte[fs.Length];

                    fs.Read(Body, 0, Body.Length);
                }

                FileName = Path.GetFileName(ofd.FileName);
            }
        }

        protected override void Save()
        {
            this.EntityViewModel.Name = Name;
            this.EntityViewModel.FileName = FileName;
            this.EntityViewModel.Body = Body;

            EntityViewModel.MaterialTypeKey = ((Specification.Material.ByMaterialType)EntityModel.DefaultSpecification).MaterialTypeKey;
        }
    }
}