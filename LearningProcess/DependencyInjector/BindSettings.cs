using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.DependencyInjector
{
    public class BindSettings
    {
        public BindSettings()
        { }

        public Type TFrom { get; set; }
        public Type TTo { get; set; }
        public bool IsSingleInstance { get; set; }

        public object Instance { get; set; }
    }
}
