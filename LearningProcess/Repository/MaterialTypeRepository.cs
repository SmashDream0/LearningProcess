using LearningProcess.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.Specification.MaterialType;
using LearningProcess.ORM.Context;

namespace LearningProcess.Repository
{
    public class MaterialTypeRepository : BaseRepository<Entity.MaterialType>
    {
        public MaterialTypeRepository(IContext context)
            : base(context)
        { }

        /// <summary>
        /// Найти тип материал по ключу дисциплины
        /// </summary>
        /// <param name="disciplineKey"></param>
        /// <returns></returns>
        public IEnumerable<Entity.MaterialType> Find(int disciplineKey)
        {
            var specification = new ByDiscipline(disciplineKey);

            return Find(specification);
        }
    }
}
