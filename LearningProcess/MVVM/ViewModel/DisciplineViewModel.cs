using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.Entity;
using LearningProcess.MVVM.ViewModel.Misc;

namespace LearningProcess.MVVM.ViewModel
{
    public class DisciplineViewModel: BaseItemsViewModel<Entity.Discipline>
    {
        public DisciplineViewModel(Model.DisciplineModel disciplineModel)
            : base()
        {
            _disciplineModel = disciplineModel;
            Initialize();
        }

        #region Поля и свойства

        private readonly Model.DisciplineModel _disciplineModel;
        private IEnumerable<Entity.Discipline> _disciplines;

        /// <summary>
        /// Выбранная дисциплина
        /// </summary>
        public Discipline SelectedDiscipline { get; set; }

        /// <summary>
        /// Список дисциплин
        /// </summary>
        public override IEnumerable<Discipline> Items => _disciplines;

        #endregion

        #region Методы

        private void Initialize()
        {
            _disciplines = _disciplineModel.Disciplines;

            AddItemCommand = new Command(Add);
            DeleteItemCommand = new Command(Delete, CanDelete);
        }

        private void Add()
        { }

        private void Delete()
        { _disciplineModel.RemoveDiscipline(SelectedDiscipline); }

        private bool CanDelete()
        { return SelectedDiscipline != null; }

        #endregion
    }
}
