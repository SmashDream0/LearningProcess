using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Specification.Entity.Material
{
    public class ByKey :  Specification<LearningProcess.Entity.Material>
    {
        public ByKey(int key)
            : base(new Operation<LearningProcess.Entity.Material>(p => p.Key, EOperation.Equal, key))
        { }
    }
}
