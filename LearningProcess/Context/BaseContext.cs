using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.Repository;

namespace LearningProcess.Context
{
    public abstract class BaseContext : IContext
    {
        public BaseContext(IContextType contextType)
        { ContextType = contextType; }

        public IContextType ContextType { get; private set; }

        public abstract IEnumerable<T> Query<T>(Specification.Specification specification);

        public abstract void Delete(Specification.Specification specification);

        public abstract void Insert<T>(T[] entities);
    }
}