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

namespace MyFrame.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 当前数据库上下文
        /// </summary>
        protected EFDbContext dbContext = EFDbContextFactory.GetCurrentContext();
        public virtual IQueryable<TEntity> Entities
        {
            get
            {
                return dbContext.Set<TEntity>();
            }
        }

        public virtual TEntity Add(TEntity entity)
        {
            try
            {
                dbContext.Entry<TEntity>(entity).State = EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entity;
        }

        public virtual int Count(Expression<Func<TEntity, bool>> where)
        {
            return dbContext.Set<TEntity>().Count(where);
        }

        public virtual bool Delete(TEntity entity)
        {
            int rst = 0;
            try
            {
                dbContext.Set<TEntity>().Attach(entity);
                dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                rst = dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rst > 0;
        }

        public virtual bool Update(TEntity entity)
        {
            int rst = 0;
            try
            {
                dbContext.Set<TEntity>().Attach(entity);
                dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
                rst = dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rst > 0;
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> where)
        {
            return Entities.Any(where);
        }

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where)
        {
            return Entities.Where(where);
        }


        public virtual IQueryable<TEntity> FindByPage(Expression<Func<TEntity, bool>> where, IList<OrderByArgs<TEntity>> orderByList, PageArgs pageArgs)
        {
            //1、处理分页参数
            var _list = Entities.Where(where);
            pageArgs.RecordsCount = _list.Count();
            pageArgs.PageCount = Convert.ToInt32(Math.Ceiling(pageArgs.RecordsCount * 1.0 % pageArgs.PageSize));
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
            _list = _list.Skip((pageArgs.PageIndex - 1) * pageArgs.PageSize).Take(pageArgs.PageCount);
            //4、返回
            return _list;
        }
    }
}
