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

namespace LearningProcess.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            switch (Program.SettingsInstance.SelectedThemeIndex)
            {
                case 0:
                    BureauBlack_Click(null, null);
                    break;
                case 1:
                    BureauBlue_Click(null, null);
                    break;
                case 2:
                    ExpressionDark_Click(null, null);
                    break;
                case 3:
                    ExpressionLight_Click(null, null);
                    break;
                case 4:
                    ShinyBlue_Click(null, null);
                    break;
                case 5:
                    ShinyRed_Click(null, null);
                    break;
                case 6:
                    WhistlerBlue_Click(null, null);
                    break;
                case 7:
                    Button_Click(null, null);
                    break;
            }
        }

        private FrameworkElement GetMain()
        {
            var fe = this as FrameworkElement;
            FrameworkElement result = fe;

            while (true)
            {
                fe = fe.Parent as FrameworkElement;

                if (fe == null)
                { break; }
                else
                { result = fe; }
            }

            return result;
        }

        private void BureauBlack_Click(object sender, RoutedEventArgs e)
        {
            Program.SettingsInstance.SelectedThemeIndex = 0;
            MkThemeSelector.SetCurrentThemeDictionary(GetMain(), new Uri("/Themes/BureauBlack.xaml", UriKind.Relative));
        }

        private void BureauBlue_Click(object sender, RoutedEventArgs e)
        {
            Program.SettingsInstance.SelectedThemeIndex = 1;
            MkThemeSelector.SetCurrentThemeDictionary(GetMain(), new Uri("/Themes/BureauBlue.xaml", UriKind.Relative));
        }

        private void ExpressionDark_Click(object sender, RoutedEventArgs e)
        {
            Program.SettingsInstance.SelectedThemeIndex = 2;
            MkThemeSelector.SetCurrentThemeDictionary(GetMain(), new Uri("/Themes/ExpressionDark.xaml", UriKind.Relative));
        }

        private void ExpressionLight_Click(object sender, RoutedEventArgs e)
        {
            Program.SettingsInstance.SelectedThemeIndex = 3;
            MkThemeSelector.SetCurrentThemeDictionary(GetMain(), new Uri("/Themes/ExpressionLight.xaml", UriKind.Relative));
        }

        private void ShinyBlue_Click(object sender, RoutedEventArgs e)
        {
            Program.SettingsInstance.SelectedThemeIndex = 4;
            MkThemeSelector.SetCurrentThemeDictionary(GetMain(), new Uri("/Themes/ShinyBlue.xaml", UriKind.Relative));
        }

        private void ShinyRed_Click(object sender, RoutedEventArgs e)
        {
            Program.SettingsInstance.SelectedThemeIndex = 5;
            MkThemeSelector.SetCurrentThemeDictionary(GetMain(), new Uri("/Themes/ShinyRed.xaml", UriKind.Relative));
        }

        private void WhistlerBlue_Click(object sender, RoutedEventArgs e)
        {
            Program.SettingsInstance.SelectedThemeIndex = 6;
            MkThemeSelector.SetCurrentThemeDictionary(GetMain(), new Uri("/Themes/WhistlerBlue.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Program.SettingsInstance.SelectedThemeIndex = 7;
            MkThemeSelector.SetCurrentThemeDictionary(null, null);
        }
    }
}
