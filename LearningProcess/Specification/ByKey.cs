using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.ORM.Specification.Operand;
using LearningProcess.ORM.Specification;
using LearningProcess.ORM;

namespace LearningProcess.Specification
{
    public class ByKey<T> : BaseGenericSpecification<T>
        where T: class, IEntity
    {
        public ByKey(int key)
            : base(new Operation<T>(p => p.Key, EOperation.Equal, key))
        { }
    }
}