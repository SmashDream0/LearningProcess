using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.MVVM.ViewModel.Misc
{
    public abstract class BaseNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        internal void propertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                if (!notUpdateProperties.Contains(propertyName))
                { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
            }
        }

        protected List<string> notUpdateProperties = new List<string>();
    }
}
