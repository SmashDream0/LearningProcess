using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.LicenseMaker
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var mainView = new MainView();
            var viewModel = new MainViewModel();

            mainView.DataContext = viewModel;

            mainView.ShowDialog();
        }
    }
}
