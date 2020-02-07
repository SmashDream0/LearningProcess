using LearningProcess.MVVM.ViewModel.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LearningProcess.MVVM.ViewModel.Item
{
    public class EmptyItem : AItem
    {
        public EmptyItem()
        {
            Uri uri = new Uri("pack://application:,,,/Resources/AddNewImage.png");

            Image = new BitmapImage();
            Image.BeginInit();
            Image.UriSource = uri;
            Image.DecodePixelHeight = 100;
            Image.DecodePixelWidth = 100;
            Image.EndInit();
        }

        public event Action<EmptyItem> OnClick;

        /// <summary>
        /// Картинка
        /// </summary>
        public BitmapImage Image
        { get; set; }

        public override bool IsReadOnly => true;

        protected override void Clicked()
        { OnClick?.Invoke(this); }
    }
}
