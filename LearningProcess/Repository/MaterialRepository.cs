using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.Specification.Material;
using LearningProcess.ORM.Context;
using LearningProcess.ORM;
using LearningProcess.Entity;

namespace LearningProcess.Repository
{
    public class MaterialRepository : BaseRepository<Material>
    {
        public MaterialRepository(IContext context)
            : base(context)
        { }

        /// <summary>
        /// Найти материал по ключу типа материала
        /// </summary>
        /// <param name="materialType">Ключ типа материала</param>
        /// <returns></returns>
        public IEnumerable<Material> Find(int materialType)
        {
            var specification = new ByMaterialType(materialType);

            return Find(specification);
        }

        /// <summary>
        /// Найти материал по ключу
        /// </summary>
        /// <param name="materialKey">Ключ материала</param>
        /// <returns></returns>
        public Material FirstOrDefault(int materialKey)
        {
            var specification = new ByKey(materialKey);

            return base.FirstOrDefault(specification);
        }
    }
}
