using MyFrame.Infrastructure.OptResult;
using MyFrame.Core.Model;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository;
using MyFrame.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFrame.Infrastructure.Extension;
using MyFrame.RBAC.Repository.Interface;
using MyFrame.Core.UnitOfWork;
using MyFrame.RBAC.Service.Interface;

namespace MyFrame.RBAC.Service.Impl
{
    public class RolePermissionService : BaseService<RolePermission>, IRolePermissionService
    {
        IRolePermissionRepository _rolePermissionRep;
        IRoleRepository _roleRep;
        IModuleRepository _moduleRep;
        IOperationRepository _optRep;
        IUserRepository _usrRep;
        IUserRoleRelRepository _usrRoleRelRep;

        const string Msg_AssignModule = "分配模块权限";
        const string Msg_AssignOpt = "分配操作权限";
        const string Msg_AssignAllModule = "分配所有模块权限";
        const string Msg_AssignAllOpt = "分配所有操作权限";
        const string Msg_CeckOptPermission = "检查当前用户操作权限";

        public RolePermissionService(IUnitOfWork unitOfWork,
            IRolePermissionRepository rolePermissionRep,
            IRoleRepository roleRep,
            IModuleRepository moduleRep,
            IOperationRepository optRep,
            IUserRepository usrRep,
            IUserRoleRelRepository usrRoleRelRep)
            : base(unitOfWork)
        {
            _roleRep = roleRep;
            _rolePermissionRep = rolePermissionRep;
            _moduleRep = moduleRep;
            _optRep = optRep;
            _usrRep = usrRep;
            _usrRoleRelRep = usrRoleRelRep;
        }

        /// <summary>
        /// 分配模块权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="moduleIds"></param>
        /// <returns></returns>
        public OperationResult AssignModulePermissions(int roleId, int[] moduleIds)
        {
            OperationResult rst = new OperationResult();
            if (!_roleRep.Exists(r => r.Id == roleId))
            {
                rst.ResultType = OperationResultType.ParamError;
                rst.Message = Msg_AssignModule + "失败，指定角色不存在";
                return rst;
            }
            /*
             * 1、删除指定角色所有模块权限
             * 2、给指定角色添加新模块权限
             */
            base.UnitOfWork.AutoCommit = false;
            base.UnitOfWork.BeginTransaction();
            try
            {
                _rolePermissionRep.Delete(r => r.RoleId == roleId);

                if (moduleIds != null && moduleIds.Length > 0)
                {
                    var queryAdd = from mId in moduleIds
                                   select new RolePermission
                                   {
                                       RoleId = roleId,
                                       PermissionId = mId,
                                       PerType = PermissionType.Module.ConvertTo<int>()
                                   };
                    _rolePermissionRep.AddBatch(queryAdd.ToList());
                }
                base.UnitOfWork.Commit();

                rst.ResultType = OperationResultType.Success;
            }
            catch (Exception ex)
            {
                base.UnitOfWork.Rollback();
                base.ProcessException(ref rst, Msg_AssignModule + "失败", ex);
            }

            return rst;
        }

        /// <summary>
        /// 分配操作权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="moduleId"></param>
        /// <param name="optIds"></param>
        /// <returns></returns>
        public OperationResult AssignOptPermissions(int roleId, int moduleId, int[] optIds)
        {
            OperationResult rst = new OperationResult();

            if (!_roleRep.Exists(r => r.Id == roleId))
            {
                rst.ResultType = OperationResultType.ParamError;
                rst.Message = Msg_AssignOpt + "失败，指定角色不存在";
                return rst;
            }

            /*
            * 1、删除指定角色指定模块下所有操作权限
            * 2、给指定角色添加指定模块操作权限
            */
            base.UnitOfWork.AutoCommit = false;
            base.UnitOfWork.BeginTransaction();
            try
            {
                //删除指定角色、指定模块下所有操作权限
                var perType = PermissionType.Operation.ToInt();
                var perIdsByModule = from per in _rolePermissionRep.Entities
                                     where per.PerType == perType
                                     join opt in _optRep.Entities on per.PermissionId equals opt.Id
                                     join module in _moduleRep.Entities on opt.ModuleId equals module.Id
                                     where per.RoleId == roleId && module.Id == moduleId
                                     select per.Id;
                _rolePermissionRep.Delete(per => perIdsByModule.Contains(per.Id));
                if (optIds != null && optIds.Length > 0)
                {
                    //添加
                    var queryAdd = from optId in optIds
                                   select new RolePermission
                                   {
                                       RoleId = roleId,
                                       PermissionId = optId,
                                       PerType = PermissionType.Operation.ConvertTo<int>()
                                   };
                    _rolePermissionRep.AddBatch(queryAdd.ToList());
                }

                base.UnitOfWork.Commit();
                rst.ResultType = OperationResultType.Success;
            }
            catch (Exception ex)
            {
                base.UnitOfWork.Rollback();
                base.ProcessException(ref rst, Msg_AssignModule + "失败", ex);
            }

            return rst;
        }

        /// <summary>
        /// 分配所有模块权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public OperationResult AssignAllModulePermissions(int roleId)
        {
            OperationResult rst = new OperationResult();
            if (!_roleRep.Exists(r => r.Id == roleId))
            {
                rst.ResultType = OperationResultType.ParamError;
                rst.Message = Msg_AssignAllModule + "失败，指定角色不存在";
                return rst;
            }
            /*
             * 1、删除指定角色所有模块权限
             * 2、给指定角色添加所有模块权限
             */
            base.UnitOfWork.AutoCommit = false;
            base.UnitOfWork.BeginTransaction();
            try
            {
                var perType = PermissionType.Module.ConvertTo<int>();
                _rolePermissionRep.Delete(r => r.RoleId == roleId && r.PerType == perType);
                //所有模块id
                var moduleIds = _moduleRep.FindBySelector(m => true, m => new { Id = m.Id }).ToList();
                if (moduleIds.Count() > 0)
                {
                    _rolePermissionRep.AddBatch(() =>
                    {
                        var pers = new List<RolePermission>();
                        moduleIds.ForEach((moduleId) =>
                        {
                            pers.Add(new RolePermission { RoleId = roleId, PerType = perType, PermissionId = moduleId.Id });
                        });
                        return pers;
                    });
                }
                base.UnitOfWork.Commit();

                rst.ResultType = OperationResultType.Success;
            }
            catch (Exception ex)
            {
                base.UnitOfWork.Rollback();
                base.ProcessException(ref rst, Msg_AssignAllModule + "失败", ex);
            }

            return rst;
        }
        /// <summary>
        /// 分配所有操作权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public OperationResult AssignAllOptPermissions(int roleId)
        {
            OperationResult rst = new OperationResult();
            if (!_roleRep.Exists(r => r.Id == roleId))
            {
                rst.ResultType = OperationResultType.ParamError;
                rst.Message = Msg_AssignAllOpt + "失败，指定角色不存在";
                return rst;
            }
            /*
             * 1、删除指定角色所有模块权限
             * 2、给指定角色添加所有模块权限
             */
            base.UnitOfWork.AutoCommit = false;
            base.UnitOfWork.BeginTransaction();
            try
            {
                var perType = PermissionType.Operation.ConvertTo<int>();
                _rolePermissionRep.Delete(r => r.RoleId == roleId && r.PerType == perType);

                //所有操作id
                var optIds = _optRep.FindBySelector(o => true, o => new { Id = o.Id }).ToList();
                if (optIds.Count() > 0)
                {
                    _rolePermissionRep.AddBatch(() =>
                    {
                        var pers = new List<RolePermission>();
                        optIds.ForEach((optId) =>
                        {
                            pers.Add(new RolePermission { RoleId = roleId, PerType = perType, PermissionId = optId.Id });
                        });
                        return pers;
                    });
                }

                base.UnitOfWork.Commit();

                rst.ResultType = OperationResultType.Success;
            }
            catch (Exception ex)
            {
                base.UnitOfWork.Rollback();
                base.ProcessException(ref rst, Msg_AssignAllOpt + "失败", ex);
            }

            return rst;
        }

        /// <summary>
        /// 检查当前用户权限
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="optId">optId为null则只校验模块权限</param>
        /// <returns></returns>
        public OperationResult CheckPermission(int? moduleId, int? optId = null)
        {
            OperationResult rst = new OperationResult();
            if (moduleId == null)
            {
                rst.ResultType = OperationResultType.PermissionDenied;
                rst.Message = "权限不足";
                return rst;
            }

            try
            {
                //1、模块权限
                //用户所有权限
                var queryPer = _usrRep.Entities.Where(ur => ur.Id == RBACContext.CurrentUser.Id)
                            .Join(_usrRoleRelRep.Entities, u => u.Id, r => r.UserId, (u, ur) => new { ur.UserId, ur.RoleId })
                            .Join(_rolePermissionRep.Entities, r => r.RoleId, p => p.RoleId, (r, p) => new { p.PermissionId, p.PerType });

                var perType = PermissionType.Module.ToInt();
                if (queryPer.Where(p => p.PermissionId == moduleId && p.PerType == perType).Count() < 1)
                {
                    rst.ResultType = OperationResultType.PermissionDenied;
                    rst.Message = "模块权限不足";
                    return rst;
                }
                //2、操作权限
                if (optId.HasValue)
                {
                    perType = PermissionType.Operation.ToInt();
                    if (queryPer.Where(p => p.PermissionId == optId && p.PerType == perType).Count() < 1)
                    {
                        rst.ResultType = OperationResultType.PermissionDenied;
                        rst.Message = "操作权限不足";
                        return rst;
                    }
                }
                rst.ResultType = OperationResultType.Success;
            }
            catch (Exception ex)
            {
                base.ProcessException(ref rst, Msg_CeckOptPermission + "失败", ex);
            }
            return rst;
        }
    }
}
