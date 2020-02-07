using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.ORM;
using LearningProcess.MVVM.ViewModel.Misc;

namespace LearningProcess.MVVM.ViewModel.Items
{
    public class MaterialsViewModel : BaseItemsViewModel<Entity.Material, View.MaterialEditorView, MaterialViewModel>
    {
        public MaterialsViewModel(Model.MaterialModel materialModel)
            : base(materialModel)
        { }

        public override string Name => "Материалы";
    }
}