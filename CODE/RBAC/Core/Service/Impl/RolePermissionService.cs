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
                var perIdsByModule = from per in _rolePermissionRep.Entities
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
        /// 检查当前用户的操作权限
        /// </summary>
        /// <param name="optId"></param>
        /// <returns></returns>
        public OperationResult CheckOptPermission(int optId)
        {
            OperationResult rst = new OperationResult();
            try
            {
                var query = from usr in _usrRep.Entities
                            join usrRoles in _usrRoleRelRep.Entities on usr.Id equals usrRoles.UserId
                            join rolePer in _rolePermissionRep.Entities on usrRoles.RoleId equals rolePer.RoleId
                            where usr.Id == RBACContext.CurrentUser.Id && rolePer.PermissionId == optId
                            select rolePer.PermissionId;
                var result = query.Count() > 0;
                if (result)
                {
                    rst.ResultType = OperationResultType.Success;
                }
                else
                {
                    rst.ResultType = OperationResultType.PermissionDenied;
                    rst.Message = "权限不足";
                }
            }
            catch (Exception ex)
            {
                base.ProcessException(ref rst, Msg_CeckOptPermission + "失败", ex);
            }
            return rst;
        }
    }
}
