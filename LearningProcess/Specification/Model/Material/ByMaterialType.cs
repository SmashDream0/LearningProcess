using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.Specification.Entity.Material
{
    public class ByMaterialType : Specification<LearningProcess.Entity.Material>
    {
        public ByMaterialType(int materialTypeKey)
            : base(new Operation<LearningProcess.Entity.Material>(p => p.MaterialTypeKey, EOperation.Equal, materialTypeKey))
        { }
    }
}
