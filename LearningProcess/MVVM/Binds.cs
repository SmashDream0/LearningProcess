using LearningProcess.ORM.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.MVVM
{
    public static class Binds
    {
        static Binds()
        { DI = new DependencyInjector.SimpleDI(); }

        /// <summary>
        /// Ссылка на контейнер зависимостей
        /// </summary>
        public static DependencyInjector.SimpleDI DI
        { get; private set; }
        
        /// <summary>
        /// Произвести связку
        /// </summary>
        /// <param name="context">Контекст данных</param>
        public static void MakeBinds(IContext context)
        {
            DI.Bind<IContext>().To(context);

            //В перспективе Bind<Тип>, в качестве типа можно использовать интерфейсы, для того, чтобы отвязать поведение от типа
            DI.Bind<Repository.DisciplineRepository>().To<Repository.DisciplineRepository>(true);
            DI.Bind<Repository.MaterialTypeRepository>().To<Repository.MaterialTypeRepository>(true);
            DI.Bind<Repository.MaterialRepository>().To<Repository.MaterialRepository>(true);

            DI.Bind<Model.DisciplineModel>().To<Model.DisciplineModel>();
            DI.Bind<Model.MaterialModel>().To<Model.MaterialModel>();
            DI.Bind<Model.MaterialTypeModel>().To<Model.MaterialTypeModel>();

            DI.Bind<View.LinkWindow>().To<ViewModel.Link.LinkViewModel>(true);

            DI.Bind<View.MainView>().To<ViewModel.MainViewModel>();
            DI.Bind<View.MaterialsReadView>().To<ViewModel.MaterialsReadViewModel>();

            DI.Bind<View.MaterialTypesView>().To<ViewModel.Items.MaterialTypesViewModel>();
            DI.Bind<View.MaterialsView>().To<ViewModel.Items.MaterialsViewModel>();
            DI.Bind<View.DisciplinesView>().To<ViewModel.Items.DisciplinesViewModel>();
            DI.Bind<View.DisciplinesListView>().To<ViewModel.Items.DisciplinesListViewModel>();
            DI.Bind<View.PasswordView>().To<ViewModel.PasswordViewModel>();
            DI.Bind<View.SelectDisciplineView>().To<ViewModel.SelectDisciplineViewModel>();

            DI.Bind<View.MaterialTypeEditorView>().To<ViewModel.Editors.MaterialTypeEditViewModel>();
            DI.Bind<View.MaterialEditorView>().To<ViewModel.Editors.MaterialEditViewModel>();
            DI.Bind<View.DisciplineEditorView>().To<ViewModel.Editors.DisciplineEditViewModel>();
        }
    }
}
