using LearningProcess.ORM.Specification;
using LearningProcess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.ORM.Specification.Operand;

namespace LearningProcess.Specification.Material
{
    public class ByMaterialType : BaseGenericSpecification<Entity.Material>
    {
        public ByMaterialType(int materialTypeKey)
            : base(new Operation<Entity.Material>(p => p.MaterialTypeKey, EOperation.Equal, materialTypeKey))
        { MaterialTypeKey = materialTypeKey; }

        public int MaterialTypeKey
        { get; private set; }
    }
}
