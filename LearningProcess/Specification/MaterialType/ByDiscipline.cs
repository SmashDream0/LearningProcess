using LearningProcess.ORM.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.ORM.Specification.Operand;

namespace LearningProcess.Specification.MaterialType
{
    public class ByDiscipline : BaseGenericSpecification<Entity.MaterialType>
    {
        public ByDiscipline(int disciplineKey)
            : base(new Operation<Entity.MaterialType>(p => p.DisciplineKey, EOperation.Equal, disciplineKey))
        { DisciplineKey = disciplineKey; }

        public int DisciplineKey
        { get; private set; }
    }
}
