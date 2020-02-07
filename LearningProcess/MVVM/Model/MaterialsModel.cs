using LearningProcess.MVVM.ViewModel;
using LearningProcess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.MVVM.Model
{
    public class MaterialModel : AModel<Material, MaterialViewModel>
    {
        public MaterialModel(Repository.MaterialRepository materialRepository) : base(materialRepository)
        { }

        public override IEnumerable<MaterialViewModel> GetItems(ViewModelSettings viewModelSettings)
        {
            if (DefaultSpecification == null)
            { return new MaterialViewModel[0]; }
            else
            { return base.GetItems(viewModelSettings); }
        }
    }
}
