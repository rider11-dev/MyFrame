using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Core.Repository
{
    public interface IBaseRepositoryBatch<TEntity> where TEntity : class
    {
        void AddBatch(IEnumerable<TEntity> entities);

        void AddBatch(Func<IEnumerable<TEntity>> entityBuilder);
    }
}
