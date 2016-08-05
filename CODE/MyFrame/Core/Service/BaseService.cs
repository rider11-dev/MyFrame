using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MyFrame.Infrastructure.Extension;
using MyFrame.Infrastructure.Logger;

using MyFrame.Core.Model;
using MyFrame.Core.Repository;
using MyFrame.Core.UnitOfWork;

namespace MyFrame.Core.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        IBaseRepository<TEntity> _currentRepository;
        protected IBaseRepository<TEntity> CurrentRepository
        {
            get
            {
                if (_currentRepository == null)
                {
                    _currentRepository = UnitOfWork == null ? new BaseRepository<TEntity>() : new BaseRepository<TEntity>(UnitOfWork);
                }
                return _currentRepository;
            }
        }
        protected IUnitOfWork UnitOfWork;
        ILogHelper<TEntity> _logger;
        public BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _logger = LogHelperFactory.GetLogHelper<TEntity>();
        }

        /// <summary>
        /// 数据实体类名称
        /// </summary>
        protected string EntityType = typeof(TEntity).FullName;

        public OperationResult Add(TEntity entity)
        {
            OperationResult result = new OperationResult();
            if (entity == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("新增{0}实体失败，实体不能为null", EntityType);
                return result;
            }
            //1、新增前处理
            result = OnBeforeAdd(entity);
            if (result.ResultType != OperationResultType.Success)
            {
                return result;
            }
            //2、新增
            try
            {
                var data = CurrentRepository.Add(entity);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(ref result, string.Format("新增{0}数据实体集出错", EntityType), ex);
            }
            //3、新增后处理
            result = OnAfterAdd(entity);
            return result;
        }

        public OperationResult Count(Expression<Func<TEntity, bool>> where = null)
        {
            OperationResult result = new OperationResult();
            try
            {
                var data = CurrentRepository.Count(where);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(ref result, string.Format("获取{0}数据实体总数失败", EntityType), ex);
            }
            return result;
        }
        public OperationResult Delete(Expression<Func<TEntity, bool>> where)
        {
            OperationResult result = new OperationResult();
            if (where == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("删除{0}实体失败，删除条件不能为空", EntityType);
                return result;
            }
            //1、删除前校验
            result = OnBeforeDelete(where);
            if (result.ResultType != OperationResultType.Success)
            {
                return result;
            }
            //2、删除
            try
            {
                var data = CurrentRepository.Delete(where);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(ref result, string.Format("删除{0}数据实体失败", EntityType), ex);
            }
            //3、删除后操作
            result = OnAfterDelete();
            return result;
        }

        public OperationResult Update(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update)
        {
            OperationResult result = new OperationResult();
            if (update == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("更新{0}实体失败，更新表达式不能为空", EntityType);
                return result;
            }
            try
            {
                var data = CurrentRepository.Update(where, update);

                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(ref result, string.Format("更新{0}数据实体失败", EntityType), ex);
            }
            return result;
        }
        public OperationResult Exists(Expression<Func<TEntity, bool>> where)
        {
            OperationResult result = new OperationResult();
            try
            {
                var data = CurrentRepository.Exists(where);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(ref result, string.Format("检查{0}数据实体是否存在时出错", EntityType), ex);
            }
            return result;
        }

        public OperationResult Find(Expression<Func<TEntity, bool>> where)
        {
            OperationResult result = new OperationResult();
            try
            {
                var data = CurrentRepository.Find(where);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data.ToList();
            }
            catch (Exception ex)
            {
                ProcessException(ref result, string.Format("获取{0}数据实体集出错", EntityType), ex);
            }
            return result;
        }
        public OperationResult FindBySelector(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, dynamic>> selector)
        {
            OperationResult result = new OperationResult();
            try
            {
                var data = CurrentRepository.FindBySelector(where, selector);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data.ToList();
            }
            catch (Exception ex)
            {
                ProcessException(ref result, string.Format("获取{0}数据实体集出错", EntityType), ex);
            }
            return result;

        }
        public OperationResult FindByPage(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, PageArgs pageArgs)
        {
            OperationResult result = new OperationResult();
            if (pageArgs == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("分页获取{0}实体集失败，分页参数不能为空", EntityType);
                return result;
            }
            try
            {
                var data = CurrentRepository.FindByPage(where, orderBy, pageArgs);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data.ToList();
            }
            catch (Exception ex)
            {
                ProcessException(ref result, string.Format("分页获取{0}实体集失败", EntityType), ex);
            }
            return result;
        }
        public OperationResult FindBySelectorByPage(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, dynamic>> selector, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, PageArgs pageArgs)
        {
            OperationResult result = new OperationResult();
            if (pageArgs == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("分页获取{0}实体集失败，分页参数不能为空", EntityType);
                return result;
            }
            try
            {
                var data = CurrentRepository.FindBySelectorByPage(where, selector, orderBy, pageArgs);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data.ToList();
            }
            catch (Exception ex)
            {
                ProcessException(ref result, string.Format("分页获取{0}实体集失败", EntityType), ex);
            }
            return result;
        }
        protected void ProcessException(ref OperationResult result, string msg, Exception ex)
        {
            if (result == null)
            {
                return;
            }
            result.ResultType = OperationResultType.Error;
            result.Exception = ex.GetDeepestException();
            result.Message = msg + "。" + ex.Message;
            //记录日志
            if (LogHelperFactory.Log)
            {
                _logger.LogError(ex);
            }
        }

        protected virtual OperationResult OnBeforeAdd(TEntity entity)
        {
            return new OperationResult { ResultType = OperationResultType.Success };
        }

        protected virtual OperationResult OnAfterAdd(TEntity entity)
        {
            return new OperationResult { ResultType = OperationResultType.Success };
        }

        protected virtual OperationResult OnBeforeDelete(Expression<Func<TEntity, bool>> where)
        {
            return new OperationResult { ResultType = OperationResultType.Success };
        }

        protected virtual OperationResult OnAfterDelete()
        {
            return new OperationResult { ResultType = OperationResultType.Success };
        }
    }
}
