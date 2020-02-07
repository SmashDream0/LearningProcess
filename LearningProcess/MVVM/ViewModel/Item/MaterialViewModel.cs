using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LearningProcess.ORM;
using System.Windows.Input;
using LearningProcess.MVVM.ViewModel.Misc;
using System.IO;
using LearningProcess.MVVM.ViewModel.Item;

namespace LearningProcess.MVVM.ViewModel
{
    public class MaterialViewModel : AItemViewModel<Entity.Material>
    {
        public MaterialViewModel(Entity.Material material, ViewModelSettings viewModelSettings) : base(material, viewModelSettings)
        {
            Name = material.Name;
            FileName = material.FileName;
            Body = material.Body;
            MaterialTypeKey = material.MaterialTypeKey;
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

        public int MaterialTypeKey
        { get; set; }

        public Visibility ImageVisibility
        { get { return Visibility.Collapsed; } }

        public static string OpenFile(string fileName, byte[] data)
        {
            var fullFileName = SaveFile(fileName, data);

            if (!String.IsNullOrEmpty(fullFileName))
            { System.Diagnostics.Process.Start(fullFileName); }

            return fullFileName;
        }

        public void OpenFile()
        { OpenFile(FileName, Body); }

        public string SaveFile()
        { return SaveFile(FileName, Body); }

        public static string SaveFile(string fileName, byte[] data)
        {
            if (data != null && data.Length > 0 && !String.IsNullOrEmpty(fileName))
            {
                var path = $"{Path.GetTempPath()}{Guid.NewGuid()}";

                if (!Directory.Exists(path))
                { Directory.CreateDirectory(path); }

                var fullFileName = $"{path}\\{fileName}";

                if (File.Exists(fullFileName))
                { File.Delete(fullFileName); }

                using (var fs = new FileStream(fullFileName, FileMode.Create))
                { fs.Write(data, 0, data.Length); }

                return fullFileName;
            }
            else
            { return null; }
        }

        public override Entity.Material GetEntity()
        {
            Entity.Name = Name;
            Entity.FileName = FileName;
            Entity.Body = this.Body;
            Entity.MaterialTypeKey = MaterialTypeKey;

            return Entity;
        }

        protected override void ClickedInner()
        { }

        protected override void DoubleClickedInner()
        { }

        private void SelectFile()
        { }
    }
}