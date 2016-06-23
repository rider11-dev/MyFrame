using MyFrame.Infrastructure.OptResult;
using MyFrame.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFrame.Infrastructure.Extension;
using MyFrame.Infrastructure.Logger;
using System.Linq.Expressions;
using MyFrame.Infrastructure.Pagination;
using MyFrame.Infrastructure.Expression;
using MyFrame.IRepository;

namespace MyFrame.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        IBaseRepository<TEntity> _repository = null;

        public BaseService(IBaseRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual TEntity Add(TEntity entity)
        {
            var result = _repository.Add(entity);
            return result;
        }

        public virtual int Count(Expression<Func<TEntity, bool>> where = null)
        {
            var result = _repository.Count(where);
            return result;
        }

        public virtual bool Delete(TEntity entity)
        {
            var result = _repository.Delete(entity);
            return result;
        }
        public bool Delete(Expression<Func<TEntity, bool>> where)
        {
            var result = _repository.Delete(where);
            return result;
        }

        public virtual bool Update(TEntity entity)
        {
            var result = _repository.Update(entity);
            return result;
        }
        public bool Update(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update)
        {
            var result = _repository.Update(where, update);
            return result;
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> where)
        {
            var result = _repository.Exists(where);
            return result;
        }

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where)
        {
            var result = _repository.Find(where);
            return result;
        }

        public virtual IQueryable<TEntity> FindByPage(Expression<Func<TEntity, bool>> where, IList<OrderByArgs<TEntity>> orderByList, PageArgs pageArgs)
        {
            var result = _repository.FindByPage(where, orderByList, pageArgs);
            return result;
        }

    }
}
