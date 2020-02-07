using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.ORM;
using LearningProcess.MVVM.ViewModel.Misc;

namespace LearningProcess.MVVM.ViewModel.Items
{
    public class DisciplinesViewModel : BaseItemsViewModel<Entity.Discipline, View.DisciplineEditorView, DisciplineViewModel>
    {
        public DisciplinesViewModel(Model.DisciplineModel disciplineModel)
            : base(disciplineModel)
        { }

        public override string Name => "Дисциплины";
    }
}