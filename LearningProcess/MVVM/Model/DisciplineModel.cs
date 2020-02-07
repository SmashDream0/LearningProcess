using LearningProcess.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.MVVM.Model
{
    public class DisciplineModel : AModel<Entity.Discipline, DisciplineViewModel>
    {
        public DisciplineModel(Repository.DisciplineRepository disciplineRepository)
            : base(disciplineRepository)
        { }
    }
}