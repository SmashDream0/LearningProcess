using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.ORM;
using LearningProcess.MVVM.ViewModel.Misc;
using LearningProcess.Specification.MaterialType;

namespace LearningProcess.MVVM.ViewModel.Items
{
    public class DisciplinesListViewModel : BaseItemsViewModel<Entity.Discipline, View.DisciplineEditorView, DisciplineViewModel>
    {
        public DisciplinesListViewModel(Model.DisciplineModel disciplineModel)
            : base(disciplineModel)
        {
            ViewModelSettings.IsReadOnly = true;
            ViewModelSettings.AllowDoubleClick = false;
            ViewModelSettings.AllowSingleClick = true;
            ViewModelSettings.ElementClick = ShowReadOnlyView;
        }

        public override string Name => "Дисциплины";

        private void ShowReadOnlyView(IEntity discipline)
        {
            var materialsModel = Binds.DI.GetInstance<Model.MaterialModel>() as Model.MaterialModel;
            var materialTypeModel = Binds.DI.GetInstance<Model.MaterialTypeModel>() as Model.MaterialTypeModel;

            materialTypeModel.DefaultSpecification = new ByDiscipline(discipline.Key);

            var link = WindowManager.GetLinkControl<View.MaterialsReadView>(materialsModel, materialTypeModel);

            MainLink.AddLink(link);
        }
    }
}