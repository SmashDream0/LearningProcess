using LearningProcess.ORM.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.ORM.Specification.Operand;

namespace LearningProcess.Specification.Material
{
    public class ByKey : ByKey<Entity.Material>
    {
        public ByKey(int key) : base(key)
        { }
    }
}