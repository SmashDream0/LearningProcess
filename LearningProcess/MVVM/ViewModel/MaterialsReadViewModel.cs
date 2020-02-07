using LearningProcess.MVVM.ViewModel.Items;
using LearningProcess.MVVM.ViewModel.Misc;
using LearningProcess.Specification.Material;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearningProcess.MVVM.ViewModel
{
    public class MaterialsReadViewModel : Link.LinkChild
    {
        public MaterialsReadViewModel(Model.MaterialModel materialsModel, Model.MaterialTypeModel materialTypeModel)
        {
            var viewModelSettings = new ViewModelSettings();

            viewModelSettings.AllowDoubleClick = false;
            viewModelSettings.AllowSingleClick = true;
            viewModelSettings.ElementClick = (material) =>
            {
                var typedMaterial = new MaterialViewModel((Entity.Material)material, viewModelSettings);
                var fileName = typedMaterial.SaveFile();
                FilePath = fileName;
            };

            var materialTypes = materialTypeModel.GetItems(viewModelSettings);

            var materialTypeViewModels = new List<MaterialTypeVM>();

            foreach (var materialType in materialTypes)
            {
                materialsModel.DefaultSpecification = new ByMaterialType(materialType.GetEntity().Key);

                var materials = materialsModel.GetItems(viewModelSettings);

                materialTypeViewModels.Add(new MaterialTypeVM(materialType, materials.Select(x => new MaterialVM(OpenFile)
                {
                    Name = x.Name,
                    FileName = x.FileName,
                    Data = x.Body,
                }).ToArray()));
            }

            Items = materialTypeViewModels.ToArray();
        }

        private string _filePath;

        /// <summary>
        /// Имя экрана
        /// </summary>
        public override string Name => "Для студента";

        /// <summary>
        /// Список типов материалов
        /// </summary>
        public IEnumerable<MaterialTypeVM> Items
        { get; private set; }

        /// <summary>
        /// Имя файла с путем для браузера файлов
        /// </summary>
        public string FilePath
        {
            get
            { return _filePath; }
            set
            {
                _filePath = value;
                propertyChanged("FilePath");
            }
        }

        private void OpenFile(string fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                var ext = Path.GetExtension(fileName).ToLower();

                switch (ext)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".bmp":
                    case ".txt":
                    case ".pdf":
                    case ".html":
                    case ".htm":
                        FilePath = fileName;
                        break;
                    default:
                        Process.Start(fileName);
                        FilePath = String.Empty;
                        break;
                }
            }
        }
    }

    public class MaterialTypeVM
    {
        public MaterialTypeVM(MaterialTypeViewModel materialTypeViewModel, IEnumerable<MaterialVM> materialViewModels)
        {
            Name = materialTypeViewModel.Name;
            Items = materialViewModels;
        }

        /// <summary>
        /// Тимя типа мериала
        /// </summary>
        public string Name
        { get; private set; }

        /// <summary>
        /// Список ассоциированных материалов
        /// </summary>
        public IEnumerable<MaterialVM> Items
        { get; private set; }
    }

    public class MaterialVM
    {
        public MaterialVM(Action<string> openFile)
        {
            _openFile = openFile;
            OpenCommand = new Command(Open);
            PrintCommand = new Command(Print);
            CopyCommand = new Command(Copy);
        }

        /// <summary>
        /// Имя материала
        /// </summary>
        public string Name
        { get; set; }

        /// <summary>
        /// Имя файла, без пути
        /// </summary>
        public string FileName
        { get; set; }

        /// <summary>
        /// Данные файла
        /// </summary>
        public byte[] Data
        { get; set; }

        public ICommand OpenCommand
        { get; private set; }
        public ICommand PrintCommand
        { get; private set; }
        public ICommand CopyCommand
        { get; private set; }

        private string _fileName;
        private Action<string> _openFile;

        private void Open()
        {
            if (_fileName == null)
            { _fileName = MaterialViewModel.SaveFile(FileName, Data); }

            _openFile?.Invoke(_fileName);
        }

        private void Print()
        {
            if (_fileName == null)
            { _fileName = MaterialViewModel.SaveFile(FileName, Data); }

            var info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = _fileName;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            try
            {
                var p = new Process();
                p.StartInfo = info;
                p.Start();

                p.WaitForInputIdle();
                System.Threading.Thread.Sleep(3000);
                if (false == p.CloseMainWindow())
                { p.Kill(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Copy()
        {
            if (_fileName == null)
            { _fileName = MaterialViewModel.SaveFile(FileName, Data); }

            var paths = new StringCollection();
            paths.Add(_fileName);

            Clipboard.SetFileDropList(paths);

            MessageBox.Show("Файл помещен в буфер обмена");
        }
    }
}