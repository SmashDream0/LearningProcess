using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LearningProcess.MVVM.View
{
    /// <summary>
    /// Interaction logic for LinkWindow.xaml
    /// </summary>
    public partial class LinkWindow : Window
    {
        public LinkWindow()
        {
            InitializeComponent();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            //var childLink = GetValue(this.DataContext, "CurrentLink");

            //SetValue(ActualWidth, childLink, "WindowWidth");
            //SetValue(ActualHeight, childLink, "WindowHeight");
        }

        private void SetValue(object value, object dataContext, string propertyName)
        {
            var property = GetProperty(dataContext, propertyName);

            if (property != null)
            { property.SetValue(dataContext, value); }
        }
        private object GetValue(object dataContext, string propertyName)
        {
            var propertyInfo = GetProperty(dataContext, propertyName);

            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(dataContext);

                return value;
            }
            else
            { return null; }
        }

        private PropertyInfo GetProperty(object dataContext, string name)
        {
            if (dataContext != null)
            {
                var tp = dataContext.GetType();

                var prprts = tp.GetProperties();

                var propertyInfo = tp.GetProperties()
                    .FirstOrDefault(x => x.Name == name);

                return propertyInfo;
            }
            else
            { return null; }
        }
    }
}
