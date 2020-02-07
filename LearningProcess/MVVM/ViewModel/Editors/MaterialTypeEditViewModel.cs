using LearningProcess.MVVM.ViewModel.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LearningProcess.MVVM.ViewModel.Editors
{
    public class MaterialTypeEditViewModel : BaseEditViewModel<Entity.MaterialType, MaterialTypeViewModel>
    {
        public MaterialTypeEditViewModel(Model.MaterialTypeModel materialTypeModel, MaterialTypeViewModel materialType)
            : base(materialTypeModel, materialType)
        { Name = EntityViewModel.Name; }

        public MaterialTypeEditViewModel(Model.MaterialTypeModel materialTypeModel, ViewModelSettings viewModelSettings)
            : base(materialTypeModel, viewModelSettings)
        { Name = EntityViewModel.Name; }

        protected override void Save()
        {
            EntityViewModel.Name = Name;
            EntityViewModel.DisciplineKey = ((Specification.MaterialType.ByDiscipline)EntityModel.DefaultSpecification).DisciplineKey;
        }
    }
}
