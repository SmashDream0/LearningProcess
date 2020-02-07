using LearningProcess.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.MVVM.Model
{
    public class MaterialTypeModel : AModel<Entity.MaterialType, MaterialTypeViewModel>
    {
        public MaterialTypeModel(Repository.MaterialTypeRepository materialTypeRepository) : base(materialTypeRepository)
        { }

        public override IEnumerable<MaterialTypeViewModel> GetItems(ViewModelSettings viewModelSettings)
        {
            if (DefaultSpecification == null)
            { return new MaterialTypeViewModel[0]; }
            else
            { return base.GetItems(viewModelSettings); }
        }
    }
}