using MyFrame.Infrastructure.Expression;
using MyFrame.Infrastructure.Pagination;
using MyFrame.IRepository;
using MyFrame.Repository.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using EntityFramework.Extensions;
using MyFrame.Infrastructure.Logger;

namespace MyFrame.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 当前数据库上下文
        /// </summary>
        protected EFDbContext dbContext = EFDbContextFactory.GetCurrentContext();
        ILogHelper<TEntity> _logger;
        public BaseRepository()
        {
            _logger = LogHelperFactory.GetLogHelper<TEntity>();
        }

        public IQueryable<TEntity> Entities
        {
            get
            {
                return dbContext.Set<TEntity>();
            }
        }

        public TEntity Add(TEntity entity)
        {
            try
            {
                dbContext.Entry<TEntity>(entity).State = EntityState.Added;
                dbContext.SaveChanges();
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
                count = dbContext.Set<TEntity>().Count();
            }
            else
            {
                count = dbContext.Set<TEntity>().Count(where);
            }
            return count;
        }

        public bool Delete(Expression<Func<TEntity, bool>> where)
        {
            if (where == null)
            {
                return false;
            }
            int rst = Entities.Where(where).Delete();
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
                rst = Entities.Where(where).Update(update);
            }
            else
            {
                rst = Entities.Update(update);
            }
            return rst > 0;
        }
        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            return Entities.Any(where);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where)
        {
            return Entities.Where(where);
        }

        public IQueryable<TEntity> FindByPage(Expression<Func<TEntity, bool>> where, IList<OrderByArgs<TEntity>> orderByList, PageArgs pageArgs)
        {
            //1、处理分页参数
            var _list = Entities.Where(where);
            pageArgs.RecordsCount = _list.Count();
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
            if (orderByList != null && orderByList.Count > 0)
            {
                for (int i = 0; i < orderByList.Count; i++)
                {
                    if (orderByList[i].OrderByType == OrderByType.Asc)
                    {
                        _list = _list.OrderBy(orderByList[i].Expression);
                    }
                    else
                    {
                        _list = _list.OrderByDescending(orderByList[i].Expression);
                    }
                }
            }
            //3、分页获取
            _list = _list.Skip((pageArgs.PageIndex - 1) * pageArgs.PageSize).Take(pageArgs.PageSize);
            //4、返回
            return _list;
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

    }
}
