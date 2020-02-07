using LearningProcess.ORM.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Specification.Discipline
{
    public class ByKey : ByKey<Entity.Discipline>
    {
        public ByKey(int key) : base(key)
        { }
    }
}