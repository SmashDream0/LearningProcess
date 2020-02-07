using System;
using System.Collections.Generic;
using System.Linq;
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
using CefSharp.Wpf;

namespace LearningProcess.MVVM.View
{
    /// <summary>
    /// Interaction logic for BindableWebBrowser.xaml
    /// </summary>
    public partial class BindableWebBrowser : UserControl
    {
        public BindableWebBrowser()
        {
            InitializeComponent();

            cefChromeContainer.Content = browser;
            browser.Address = "https://stackoverflow.com";
        }

        private ChromiumWebBrowser browser = new ChromiumWebBrowser("File://");
        public static readonly DependencyProperty HtmlTextProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(BindableWebBrowser));

        public string FilePath
        {
            get { return (string)GetValue(HtmlTextProperty); }
            set { SetValue(HtmlTextProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == HtmlTextProperty)
            {
                DoBrowse();
            }
        }
        private void DoBrowse()
        {
            if (!string.IsNullOrEmpty(FilePath))
            { browser.Load("file://" + FilePath); }
            else
            { browser.Load("File://"); }
        }
    }
}
