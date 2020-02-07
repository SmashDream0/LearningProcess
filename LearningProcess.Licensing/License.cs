using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Licensing
{
    public class License
    {
        public DateTime ExpiredDate
        { get; set; }

        public int ExpiredSessionSeconds
        { get; set; }

        public string ClientName
        { get; set; }
    }
}
