using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LearningProcess.ORM;
using LearningProcess.Specification.Material;
using LearningProcess.MVVM.ViewModel.Item;

namespace LearningProcess.MVVM.ViewModel
{
    public class MaterialTypeViewModel : AItemViewModel<Entity.MaterialType>
    {
        public MaterialTypeViewModel(Entity.MaterialType materialType, ViewModelSettings viewModelSettings) : base(materialType, viewModelSettings)
        {
            Name = Entity.Name;
            DisciplineKey = Entity.DisciplineKey;
        }

        public int DisciplineKey
        { get; set; }

        public Visibility ImageVisibility
        { get { return Visibility.Collapsed; } }

        public override Entity.MaterialType GetEntity()
        {
            Entity.Name = Name;
            Entity.DisciplineKey = DisciplineKey;

            return Entity;
        }

        protected override void ClickedInner()
        {

        }

        protected override void DoubleClickedInner()
        {
            var materialModel = Binds.DI.GetInstance<Model.MaterialModel>() as Model.MaterialModel;

            materialModel.DefaultSpecification = new ByMaterialType(this.Entity.Key);

            (Binds.DI.GetInstance<View.LinkWindow>() as ViewModel.Link.LinkViewModel).AddLink(WindowManager.GetLinkControl<View.MaterialsView>(materialModel));
        }
    }
}
