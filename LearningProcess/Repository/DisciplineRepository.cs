using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.ORM;
using LearningProcess.ORM.Context;

namespace LearningProcess.Repository
{
    public class DisciplineRepository : BaseRepository<Entity.Discipline>
    {
        public DisciplineRepository(IContext context)
            : base(context)
        { }
    }
}
