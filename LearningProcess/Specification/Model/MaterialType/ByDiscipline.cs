using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Specification.Entity.MaterialType
{
    public class ByDiscipline : Specification<LearningProcess.Entity.MaterialType>
    {
        public ByDiscipline(int disciplineKey)
            : base(new Operation<LearningProcess.Entity.Material>(p => p.MaterialTypeKey, EOperation.Equal, disciplineKey))
        { }
    }
}
