using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Repository
{
    public interface IBaseRepositoryExtend<TEntity> where TEntity : class
    {
        void Add(IEnumerable<TEntity> entities);
    }
}
