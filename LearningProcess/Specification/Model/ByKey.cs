using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Specification.Entity
{
    public class ByKey : Specification<LearningProcess.Entity.AModel>
    {
        public ByKey(int key)
            : base(new Operation<LearningProcess.Entity.AModel>(p => p.Key, EOperation.Equal, key))
        { }
    }
}
