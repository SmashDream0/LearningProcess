using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LearningProcess.Context
{
    public interface IContext
    {
        IContextType ContextType { get; }

        IEnumerable<T> Query<T>(Specification.Specification specification);

        void Insert<T>(T[] entities);

        void Delete(Specification.Specification specification);
    }

    public enum IContextType { SqlLite }
}
