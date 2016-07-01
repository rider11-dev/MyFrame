using MyFrame.Infrastructure.Expression;
using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.Pagination;
using MyFrame.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MyFrame.Infrastructure.Extension;
using MyFrame.IService;
using MyFrame.Infrastructure.Logger;

namespace MyFrame.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        protected IBaseRepository<TEntity> _repository = null;
        ILogHelper<TEntity> _logger;
        public BaseService(IBaseRepository<TEntity> repository)
        {
            _repository = repository;
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
            //新增前处理
            result = OnBeforeAdd(entity);
            if (result.ResultType != OperationResultType.Success)
            {
                return result;
            }
            //新增
            try
            {
                var data = _repository.Add(entity);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("新增{0}数据实体集出错", EntityType), ex);
            }
            return result;
        }

        public OperationResult Count(Expression<Func<TEntity, bool>> where = null)
        {
            OperationResult result = new OperationResult();
            try
            {
                var data = _repository.Count(where);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("获取{0}数据实体总数失败", EntityType), ex);
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
            try
            {
                var data = _repository.Delete(where);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("删除{0}数据实体失败", EntityType), ex);
            }
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
                var data = _repository.Update(where, update);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("更新{0}数据实体失败", EntityType), ex);
            }
            return result;
        }
        public OperationResult Exists(Expression<Func<TEntity, bool>> where)
        {
            OperationResult result = new OperationResult();
            try
            {
                var data = _repository.Exists(where);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("检查{0}数据实体是否存在时出错", EntityType), ex);
            }
            return result;
        }

        public OperationResult Find(Expression<Func<TEntity, bool>> where)
        {
            OperationResult result = new OperationResult();
            try
            {
                var data = _repository.Find(where);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data.ToList();
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("获取{0}数据实体集出错", EntityType), ex);
            }
            return result;
        }

        public OperationResult FindByPage(Expression<Func<TEntity, bool>> where, IList<OrderByArgs<TEntity>> orderByList, PageArgs pageArgs)
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
                var data = _repository.FindByPage(where, orderByList, pageArgs);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data.ToList();
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("分页获取{0}实体集失败", EntityType), ex);
            }
            return result;
        }

        protected void ProcessException(OperationResult result, string msg, Exception ex)
        {
            if (result == null)
            {
                return;
            }
            result.ResultType = OperationResultType.Error;
            result.Exception = ex.GetDeepestException();
            result.Message = msg;
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
    }
}
