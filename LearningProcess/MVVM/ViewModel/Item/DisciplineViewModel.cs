using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using LearningProcess.ORM;
using System.IO;
using System.Windows.Input;
using LearningProcess.MVVM.ViewModel.Item;
using LearningProcess.Specification.MaterialType;

namespace LearningProcess.MVVM.ViewModel
{
    public class DisciplineViewModel : AItemViewModel<Entity.Discipline>
    {
        public DisciplineViewModel(Entity.Discipline discipline, ViewModelSettings viewModelSettings) : base(discipline, viewModelSettings)
        {
            Name = discipline.Name;

            if (IsArrayEmpty(discipline.Image))
            {
                Uri uri = new Uri("pack://application:,,,/Resources/DisciplineEmptyImage.png");

                Image = new BitmapImage();
                Image.BeginInit();
                Image.UriSource = uri;
                Image.EndInit();
            }
            else
            { Image = ToImage(discipline.Image); }
        }

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
        /// Конвертировать массив байтов в BitmapImage
        /// </summary>
        /// <param name="array">Массив байтов</param>
        /// <returns></returns>
        public static BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            { return ToImage(ms); }
        }

        /// <summary>
        /// Конвертировать поток байтов в BitmapImage
        /// </summary>
        /// <param name="stream">Поток байтов</param>
        /// <returns></returns>
        public static BitmapImage ToImage(Stream stream)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad; // here
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        /// <summary>
        /// Получить измененную сущность
        /// </summary>
        /// <returns></returns>
        public override Entity.Discipline GetEntity()
        {
            Entity.Name = Name;

            var bytes = BufferFromImage(Image);

            Entity.Image = bytes;

            return Entity;
        }

        protected override void ClickedInner()
        { }

        protected override void DoubleClickedInner()
        {
            var materialTypeModel = Binds.DI.GetInstance<Model.MaterialTypeModel>() as Model.MaterialTypeModel;

            materialTypeModel.DefaultSpecification = new ByDiscipline(this.Entity.Key);

            (Binds.DI.GetInstance<View.LinkWindow>() as ViewModel.Link.LinkViewModel).AddLink(WindowManager.GetLinkControl<View.MaterialTypesView>(materialTypeModel));
        }

        private static byte[] BufferFromImage(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

        private static bool IsArrayEmpty(byte[] array)
        {
            return array == null || array.Length == 0 || array.FirstOrDefault(x => x != 0) == 0;
        }
    }
}