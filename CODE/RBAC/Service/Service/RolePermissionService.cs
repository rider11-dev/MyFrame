using MyFrame.Infrastructure.OptResult;
using MyFrame.Model.Unit;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository;
using MyFrame.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFrame.Infrastructure.Extension;

namespace MyFrame.RBAC.Service
{
    public class RolePermissionService : BaseService<RolePermission>, IRolePermissionService
    {
        IRolePermissionRepository _rolePermissionRep;
        IRoleRepository _roleRep;

        const string Msg_Assign = "分配模块权限";

        public RolePermissionService(IUnitOfWork unitOfWork, IRolePermissionRepository rolePermissionRep, IRoleRepository roleRep)
            : base(unitOfWork)
        {
            _roleRep = roleRep;
            _rolePermissionRep = rolePermissionRep;
        }

        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionIds"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public OperationResult AssignPermissions(int roleId, int[] permissionIds, int type)
        {
            OperationResult rst = new OperationResult();

            if (permissionIds == null || permissionIds.Length < 1)
            {
                rst.ResultType = OperationResultType.ParamError;
                rst.Message = "权限不能为空";
                return rst;
            }
            if (!_roleRep.Exists(r => r.Id == roleId))
            {
                rst.ResultType = OperationResultType.ParamError;
                rst.Message = "指定角色不存在";
                return rst;
            }
            /*
             * 1、删除指定角色指定权限
             * 2、给指定角色添加新权限
             */
            var query = from per in permissionIds
                        select new RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = per,
                            PerType = type
                        };
            base.UnitOfWork.AutoCommit = false;
            base.UnitOfWork.BeginTransaction();
            try
            {
                _rolePermissionRep.Delete(r =>
                    r.RoleId == roleId &&
                    r.PerType == type &&
                    permissionIds.Contains(r.PermissionId));

                _rolePermissionRep.Add(query.ToList());

                base.UnitOfWork.Commit();

                rst.ResultType = OperationResultType.Success;
            }
            catch (Exception ex)
            {
                base.UnitOfWork.Rollback();
                base.ProcessException(rst, Msg_Assign + "失败", ex);
            }

            return rst;
        }
    }
}
