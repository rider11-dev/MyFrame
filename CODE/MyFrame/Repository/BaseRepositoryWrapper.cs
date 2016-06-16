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

namespace MyFrame.Repository
{
    public class BaseRepositoryWrapper<TEntity> : IBaseRepositoryWrapper<TEntity> where TEntity : class
    {
        IBaseRepository<TEntity> _repository;

        string EntityType = typeof(TEntity).FullName;

        public BaseRepositoryWrapper(IBaseRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public OperationResult Entities
        {
            get
            {
                OperationResult result = new OperationResult();
                try
                {
                    var data = _repository.Entities;
                    result.ResultType = OperationResultType.Success;
                    result.AppendData = data;
                }
                catch (Exception ex)
                {
                    ProcessException(result, string.Format("获取{0}数据实体集出错", EntityType), ex);
                }
                return result;
            }
        }

        public OperationResult Add(TEntity entity)
        {
            OperationResult result = new OperationResult();
            if (entity == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("新增{0}实体失败，实体不能为null", EntityType);
                return result;
            }
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

        public OperationResult Count(Expression<Func<TEntity, bool>> where)
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

        public OperationResult Delete(TEntity entity)
        {
            OperationResult result = new OperationResult();
            if (entity == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("删除{0}实体失败，实体不能为null", EntityType);
                return result;
            }
            try
            {
                var data = _repository.Delete(entity);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("删除{0}数据实体失败", EntityType), ex);
            }
            return result;
        }
        public OperationResult Delete(string id)
        {
            OperationResult result = new OperationResult();
            if (string.IsNullOrEmpty(id))
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("删除{0}实体失败，主键不能为空", EntityType);
                return result;
            }
            try
            {
                TEntity entity = _repository.Find(id);
                if (entity == null)
                {
                    result.ResultType = OperationResultType.ParamError;
                    result.Message = string.Format("删除{0}实体失败，指定主键的实体不存在", EntityType);
                }
                else
                {
                    _repository.Delete(entity);
                }
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("删除{0}数据实体失败", EntityType), ex);
            }
            return result;
        }

        public OperationResult Update(TEntity entity)
        {
            OperationResult result = new OperationResult();
            if (entity == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("更新{0}实体失败，实体不能为null", EntityType);
                return result;
            }
            try
            {
                var data = _repository.Update(entity);
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
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("获取{0}数据实体集出错", EntityType), ex);
            }
            return result;
        }

        public OperationResult Find(string id)
        {
            OperationResult result = new OperationResult();
            if (string.IsNullOrEmpty(id))
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("根据主键获取{0}实体失败，参数不能为空", EntityType);
                return result;
            }
            try
            {
                var data = _repository.Find(id);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("根据主键获取{0}实体失败", EntityType), ex);
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
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("分页获取{0}实体集失败", EntityType), ex);
            }
            return result;
        }

        private void ProcessException(OperationResult result, string msg, Exception ex)
        {
            if (result == null)
            {
                return;
            }
            result.ResultType = OperationResultType.Error;
            result.Exception = ex.GetDeepestException();
            result.Message = msg;
        }

    }
}
