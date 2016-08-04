using MyFrame.Infrastructure.Pagination;
using MyFrame.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using EntityFramework.Extensions;
using MyFrame.Infrastructure.Logger;

using MyFrame.Core.Model;
using MyFrame.Core.UnitOfWork;
using System.Data;

namespace MyFrame.Core.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        IUnitOfWork _unitOfWork;
        protected DbContext _dbContext;
        ILogHelper<TEntity> _logger;
        public BaseRepository(IUnitOfWork unitOfWork)
            : this()
        {
            _unitOfWork = unitOfWork;
            _dbContext = unitOfWork.DbContext;

            _logger = LogHelperFactory.GetLogHelper<TEntity>();
        }

        public BaseRepository()
        {
            // TODO: Complete member initialization
        }

        public IQueryable<TEntity> Entities
        {
            get
            {
                return _dbContext.Set<TEntity>();
            }
        }

        public TEntity Add(TEntity entity)
        {
            try
            {
                _dbContext.Entry<TEntity>(entity).State = System.Data.Entity.EntityState.Added;
                if (_unitOfWork.AutoCommit)
                {
                    _dbContext.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                var msg = BuildValidationErrorMessage(ex);
                _logger.LogError(msg);

                throw ex;
            }
            return entity;
        }

        public int Count(Expression<Func<TEntity, bool>> where = null)
        {
            int count = 0;
            if (where == null)
            {
                count = _dbContext
                    .Set<TEntity>()
                    .AsNoTracking()
                    .Count();
            }
            else
            {
                count = _dbContext
                    .Set<TEntity>()
                    .AsNoTracking()
                    .Count(where);
            }
            return count;
        }

        public bool Delete(Expression<Func<TEntity, bool>> where)
        {
            if (where == null)
            {
                return false;
            }
            int rst = Entities
                .Where(where)
                .AsNoTracking()
                .Delete();
            return rst > 0;
        }
        public bool Update(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update)
        {
            if (update == null)
            {
                return false;
            }
            int rst = 0;
            if (where != null)
            {
                rst = Entities
                    .Where(where)
                    .AsNoTracking()
                    .Update(update);
            }
            else
            {
                rst = Entities
                    .AsNoTracking()
                    .Update(update);
            }
            return rst > 0;
        }

        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            return Find(where)
                .AsNoTracking()
                .Count() > 0;
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where)
        {
            var query = Entities
                .AsNoTracking()
                .Where(where);

            return query;
        }

        /// <summary>
        /// 构造ef校验异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected string BuildValidationErrorMessage(DbEntityValidationException ex)
        {
            StringBuilder sbErrors = new StringBuilder();
            var valResults = ex.EntityValidationErrors;
            foreach (var result in valResults)
            {
                var errors = result.ValidationErrors;
                foreach (var error in errors)
                {
                    sbErrors.AppendFormat("{0}:{1}", error.PropertyName, error.ErrorMessage);
                }
            }
            return sbErrors.ToString();
        }

        public IQueryable<TEntity> FindByPage(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, PageArgs pageArgs)
        {
            var query = Find(where);
            return QueryByPage(query, orderBy, pageArgs);
        }

        public IQueryable<TEntity> QueryByPage(IQueryable<TEntity> querable, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, PageArgs pageArgs)
        {
            pageArgs.RecordsCount = querable.Count();
            pageArgs.PageCount = Convert.ToInt32(Math.Ceiling(pageArgs.RecordsCount * 1.0 / pageArgs.PageSize));
            if (pageArgs.PageIndex > pageArgs.PageCount)
            {
                //页索引不能超过总页数
                pageArgs.PageIndex = pageArgs.PageCount - 1;
            }
            //页索引最小为1
            if (pageArgs.PageIndex < 1)
            {
                pageArgs.PageIndex = 1;
            }
            //2、处理排序
            var query = orderBy(querable);
            //3、分页获取
            var _list = query
                .Skip((pageArgs.PageIndex - 1) * pageArgs.PageSize)
                .Take(pageArgs.PageSize);
            return _list;
        }


        public IQueryable<dynamic> FindBySelector(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, dynamic>> selector)
        {
            return Entities
                .Where(where)
                .Select(selector);
        }

        public IQueryable<dynamic> FindBySelectorByPage(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, dynamic>> selector, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, PageArgs pageArgs)
        {
            return FindByPage(where, orderBy, pageArgs)
                .Select(selector);
        }
    }
}
