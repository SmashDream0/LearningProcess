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

namespace LearningProcess.MVVM.View.Control
{
    /// <summary>
    /// Interaction logic for ItemsControl.xaml
    /// </summary>
    public partial class ItemsControl : UserControl
    {
        public ItemsControl()
        {
            InitializeComponent();

            this.ListItems.SizeChanged += ListItems_SizeChanged;
            this.Loaded += ItemsControl_Loaded;
            ListItems.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void ItemsControl_Loaded(object sender, RoutedEventArgs e)
        { UpdateWidth(); }

        private void ListItems_SizeChanged(object sender, SizeChangedEventArgs e)
        { UpdateWidth(); }

        private void UpdateWidth()
        { SetValue(ActualWidth, DataContext, "WindowWidth"); }

        public static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
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

        private void SelectCurrentItem(object sender, MouseButtonEventArgs e)
        {
            {
                ListViewItem item = (ListViewItem)sender;
                item.IsSelected = true;

                SetBackGroundColor(item, Colors.LightSkyBlue);
            }

            for (int i = 0; i < ListItems.Items.Count; i++)
            {
                var control = ListItems.ItemContainerGenerator.ContainerFromIndex(i);

                if (control != sender)
                {
                    var item = control as ListViewItem;

                    SetBackGroundColor(item, Colors.LightGray);
                    item.IsSelected = false;
                }
            }
        }

        private void SetBackGroundColor(ListViewItem item, Color color)
        {
            var button = FindVisualChild<Button>(item);

            if (color == default(Color))
            { button.Background = null; }
            else
            { button.Background = new SolidColorBrush(color); }
        }

        private void ListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            { SetBackGroundColor(e.RemovedItems[0] as ListViewItem, Colors.Transparent); }

            SetBackGroundColor(ListItems.SelectedItem as ListViewItem, Colors.Blue);
        }
    }
}
