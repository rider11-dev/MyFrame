using MyFrame.Infrastructure.OptResult;
using MyFrame.IRepository;
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

namespace MyFrame.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        IBaseRepositoryWrapper<TEntity> _baseRepositoryWrapper = null;

        public BaseService(IBaseRepositoryWrapper<TEntity> repositoryWrapper)
        {
            _baseRepositoryWrapper = repositoryWrapper;
        }

        public TEntity Add(TEntity entity)
        {
            OperationResult result = _baseRepositoryWrapper.Add(entity);
            return result.Parse<TEntity, BaseService<TEntity>>();
        }

        public int Count(Expression<Func<TEntity, bool>> where)
        {
            OperationResult result = _baseRepositoryWrapper.Count(where);
            return result.Parse<int, BaseService<TEntity>>();
        }

        public bool Delete(TEntity entity)
        {
            OperationResult result = _baseRepositoryWrapper.Delete(entity);
            return result.Parse<bool, BaseService<TEntity>>();
        }

        public bool Update(TEntity entity)
        {
            OperationResult result = _baseRepositoryWrapper.Update(entity);
            return result.Parse<bool, BaseService<TEntity>>();
        }

        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            OperationResult result = _baseRepositoryWrapper.Exists(where);
            return result.Parse<bool, BaseService<TEntity>>();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where)
        {
            OperationResult result = _baseRepositoryWrapper.Find(where);
            return result.Parse<IQueryable<TEntity>, BaseService<TEntity>>();
        }

        public TEntity Find(string id)
        {
            OperationResult result = _baseRepositoryWrapper.Find(id);
            return result.Parse<TEntity, BaseService<TEntity>>();
        }

        public IQueryable<TEntity> FindByPage(Expression<Func<TEntity, bool>> where, IList<OrderByArgs<TEntity>> orderByList, PageArgs pageArgs)
        {
            OperationResult result = _baseRepositoryWrapper.FindByPage(where, orderByList, pageArgs);
            return result.Parse<IQueryable<TEntity>, BaseService<TEntity>>();
        }
    }
}
