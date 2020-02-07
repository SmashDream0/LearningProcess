using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.ORM;
using LearningProcess.MVVM.ViewModel.Misc;

namespace LearningProcess.MVVM.ViewModel.Items
{
    public class MaterialTypesViewModel : BaseItemsViewModel<Entity.MaterialType, View.MaterialTypeEditorView, MaterialTypeViewModel>
    {
        public MaterialTypesViewModel(Model.MaterialTypeModel materialTypeModel)
            : base(materialTypeModel)
        { }

        public override string Name => "Типы материалов";
    }
}