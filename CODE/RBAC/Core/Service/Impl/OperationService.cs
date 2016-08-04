using MyFrame.Core.Service;
using MyFrame.Core.UnitOfWork;
using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.Pagination;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository.Interface;
using MyFrame.RBAC.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Service.Impl
{
    public class OperationService : BaseService<Operation>, IOperationService
    {
        const string Msg_CheckBeforeAdd = "新增前校验";
        const string Msg_FindByOptCode = "根据编号查找操作";
        const string Msg_FindByPageWithFullInfo = "分页获取操作详细信息";
        const string Msg_FindByRolesWithSimpleInfo = "根据角色获取操作精简信息";
        const string Msg_UpdateDetail = "更新操作详细信息";

        IOperationRepository _optRepository;
        IRoleRepository _roleRepository;
        IModuleRepository _moduleRepository;
        IRolePermissionRepository _rolePerRepository;
        public OperationService(IUnitOfWork unitOfWork, IOperationRepository optRepository, IRoleRepository roleRepository, IModuleRepository moduleRepository, IRolePermissionRepository rolePerRepository)
            : base(unitOfWork)
        {
            _optRepository = optRepository;
            _roleRepository = roleRepository;
            _moduleRepository = moduleRepository;
            _rolePerRepository = rolePerRepository;
        }

        public OperationResult FindByOptCode(int moduleId, string optCode)
        {
            OperationResult result = new OperationResult();
            if (string.IsNullOrEmpty(optCode))
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = "参数错误，操作编号不能为空";
                return result;
            }
            try
            {
                var data = _optRepository.Find(o => o.ModuleId == moduleId && o.OptCode == optCode);
                result.ResultType = OperationResultType.Success;
                result.Message = "操作编号查询成功";
                result.AppendData = data.ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                base.ProcessException(ref result, Msg_FindByOptCode + "出错", ex);
            }
            return result;
        }

        public OperationResult FindByPageWithFullInfo(Expression<Func<Operation, bool>> where, Func<IQueryable<Operation>, IOrderedQueryable<Operation>> orderBy, PageArgs pageArgs)
        {
            OperationResult result = new OperationResult();
            try
            {
                var optPaged = _optRepository.FindByPage(where, orderBy, pageArgs);
                var query = from opt in optPaged
                            join module in _moduleRepository.Entities on opt.ModuleId equals module.Id into moduleQuery
                            from m in moduleQuery.DefaultIfEmpty()
                            select new
                            {
                                Id = opt.Id,
                                OptCode = opt.OptCode,
                                OptName = opt.OptName,
                                SubmitUrl = opt.SubmitUrl,
                                Icon = opt.Icon,
                                ModuleId = opt.ModuleId,
                                ModuleName = m.Name,
                                SortOrder = opt.SortOrder,
                                Enabled = opt.Enabled,
                                Remark = opt.Remark
                            };
                result.ResultType = OperationResultType.Success;
                result.AppendData = query.ToList();
            }
            catch (Exception ex)
            {
                base.ProcessException(ref result, Msg_FindByPageWithFullInfo + "错误", ex);
            }
            return result;
        }

        public OperationResult FindByRolesWithSimpleInfo(int[] roleIds)
        {
            OperationResult result = new OperationResult();
            if (roleIds == null || roleIds.Length < 1)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_FindByRolesWithSimpleInfo + ",参数错误，角色不能为空";
                return result;
            }
            try
            {
                var query = from opt in _optRepository.Entities
                            join per in _rolePerRepository.Entities on opt.Id equals per.PermissionId
                            join role in _roleRepository.Entities on per.RoleId equals role.Id
                            where roleIds.Contains(per.RoleId) && opt.Enabled && role.Enabled
                            select new
                            {
                                Id = opt.Id,
                                OptCode = opt.OptCode,
                                OptName = opt.OptName,
                                ModuleId = opt.ModuleId
                            };
                result.ResultType = OperationResultType.Success;
                result.AppendData = query.ToList();
            }
            catch (Exception ex)
            {
                base.ProcessException(ref result, Msg_FindByRolesWithSimpleInfo + "错误", ex);
            }
            return result;
        }

        public OperationResult UpdateDetail(Operation opt)
        {
            OperationResult result = new OperationResult();
            if (opt == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_UpdateDetail + ",参数错误，操作实体不能为空";
                return result;
            }

            return base.Update(o => o.Id == opt.Id, o => new Operation
            {
                OptName = opt.OptName,
                SubmitUrl = opt.SubmitUrl,
                Icon = opt.Icon,
                SortOrder = opt.SortOrder,
                Enabled = opt.Enabled,
                Remark = opt.Remark
            });
        }

        protected override OperationResult OnBeforeAdd(Operation entity)
        {
            OperationResult result = new OperationResult { ResultType = OperationResultType.Success };
            if (entity == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_CheckBeforeAdd + ",参数错误，实体不能为空";
                return result;
            }

            //校验指定模块下，操作编号是否已存在
            try
            {
                var exists = _optRepository.Exists(o => o.ModuleId == entity.ModuleId && o.OptCode == entity.OptCode);
                if (exists)
                {
                    result.ResultType = OperationResultType.CheckFailedBeforeProcess;
                    result.Message = Msg_CheckBeforeAdd + string.Format(",指定模块下已存在编号为{0}的操作", entity.OptCode);
                }
            }
            catch (Exception ex)
            {
                base.ProcessException(ref result, Msg_CheckBeforeAdd, ex);
            }
            return result;
        }
    }
}
