using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;
using MyFrame.RBAC.Repository.Interface;
using MyFrame.Core.UnitOfWork;
using MyFrame.RBAC.Model;
using MyFrame.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MyFrame.RBAC.Repository;
using MyFrame.Core.Service;
using MyFrame.RBAC.Service.Interface;

namespace MyFrame.RBAC.Service.Impl
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        IRoleRepository _roleRepository;
        IUserRepository _userRepository;
        IRolePermissionRepository _rolePermissionRep;
        IUserRoleRelRepository _usrRoleRelRep;
        const string Msg_SearchByName = "根据角色名称查询";
        const string Msg_BeforeAdd = "保存角色前校验";
        const string Msg_UpdateDetail = "更新角色信息";
        const string Msg_SearchFullInfoByPage = "分页获取角色详细信息";
        const string Msg_SearchSimpleInfoByPage = "分页获取角色精简信息";
        const string Msg_DeleteWithRelations = "删除角色（包含关系数据）";
        public RoleService(IUnitOfWork unitOfWork, IRoleRepository roleRep,
            IUserRepository userRep,
            IRolePermissionRepository rolePermissionRep,
            IUserRoleRelRepository usrRoleRelRep)
            : base(unitOfWork)
        {
            _roleRepository = roleRep;
            _userRepository = userRep;
            _rolePermissionRep = rolePermissionRep;
            _usrRoleRelRep = usrRoleRelRep;
        }

        public OperationResult FindByName(string roleName)
        {
            OperationResult rst = new OperationResult();
            if (string.IsNullOrEmpty(roleName))
            {
                rst.ResultType = OperationResultType.ParamError;
                rst.Message = Msg_SearchByName + "失败，角色名不能为空";
                return rst;
            }
            try
            {
                var data = _roleRepository.Find(r => r.RoleName == roleName).ToList();
                rst.ResultType = OperationResultType.Success;
                rst.Message = Msg_SearchByName + "成功";
            }
            catch (Exception ex)
            {
                base.ProcessException(rst, Msg_SearchByName + "失败,角色名称：" + roleName, ex);
            }
            return rst;
        }

        protected override OperationResult OnBeforeAdd(Role entity)
        {
            OperationResult result = new OperationResult();
            if (entity == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("{0}失败，{1}", Msg_BeforeAdd, "角色实体不能为空");
                return result;
            }
            if (string.IsNullOrEmpty(entity.RoleName))
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("{0}失败，{1}", Msg_BeforeAdd, "角色名称不能为空");
                return result;
            }
            //1、校验角色是否已存在
            result = this.FindByName(entity.RoleName);
            if (result.ResultType != OperationResultType.Success)
            {
                return result;
            }
            if (result.AppendData != null && (result.AppendData as Role) != null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("{0}失败，{1}", Msg_BeforeAdd, "角色已存在");
                return result;
            }
            result.ResultType = OperationResultType.Success;
            return result;
        }

        public OperationResult UpdateDetail(Role role)
        {
            OperationResult result = new OperationResult();
            if (role == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_UpdateDetail + "失败，角色实体不能为空";
                return result;
            }
            result = base.Exists(r => r.Id == role.Id);
            if (result.ResultType != OperationResultType.Success)
            {
                return result;
            }
            if (Convert.ToBoolean(result.AppendData) == false)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_UpdateDetail + "失败，指定角色不存在";
                return result;
            }
            return base.Update(r => r.Id == role.Id,
                r => new Role
                {
                    Remark = role.Remark,
                    Enabled = role.Enabled,
                    SortOrder = role.SortOrder,
                    LastModifier = role.LastModifier,
                    LastModifyTime = role.LastModifyTime
                });
        }

        public OperationResult FindByPageWithFullInfo(Expression<Func<Role, bool>> where, Action<IOrderable<Role>> orderBy, PageArgs pageArgs)
        {
            OperationResult result = new OperationResult();
            try
            {
                //先分页
                var rolePaged = _roleRepository.FindByPage(where, orderBy, pageArgs);
                //再连接
                var query = from role in rolePaged
                            join creator in _userRepository.Entities on role.Creator equals creator.Id into queryCreator
                            from c in queryCreator.DefaultIfEmpty()
                            join modifier in _userRepository.Entities on role.LastModifier equals modifier.Id into queryModifier
                            from m in queryModifier.DefaultIfEmpty()
                            select new
                            {
                                Id = role.Id,
                                RoleName = role.RoleName,
                                Remark = role.Remark,
                                Enabled = role.Enabled,
                                SortOrder = role.SortOrder,
                                Creator = role.Creator,
                                CreatorName = c.UserName,
                                CreateTime = role.CreateTime,
                                LastModifier = role.LastModifier,
                                LastModifierName = m.UserName,
                                LastModifyTime = role.LastModifyTime
                            };
                result.ResultType = OperationResultType.Success;
                result.AppendData = query.ToList();
            }
            catch (Exception ex)
            {
                base.ProcessException(result, string.Format(Msg_SearchSimpleInfoByPage + ",失败"), ex);
            }
            return result;
        }

        public OperationResult FindByPageWithSimpleInfo(Expression<Func<Role, bool>> where, Action<IOrderable<Role>> orderBy, PageArgs pageArgs)
        {
            OperationResult result = new OperationResult();
            try
            {
                //先分页
                var rolePaged = _roleRepository.FindByPage(where, orderBy, pageArgs);
                //再连接
                var query = from role in rolePaged
                            select new
                            {
                                Id = role.Id,
                                RoleName = role.RoleName,
                                Remark = role.Remark
                            };
                result.ResultType = OperationResultType.Success;
                result.AppendData = query.ToList();
            }
            catch (Exception ex)
            {
                base.ProcessException(result, string.Format(Msg_SearchSimpleInfoByPage + ",失败"), ex);
            }
            return result;
        }

        /// <summary>
        /// 级联删除
        /// </summary>
        /// <param name="roleIds"></param>
        public OperationResult DeleteWithRelations(int[] roleIds)
        {
            OperationResult result = new OperationResult();
            if (roleIds == null || roleIds.Length < 1)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("{0}失败，{1}", Msg_DeleteWithRelations, "角色id不能为空");
                return result;
            }
            if (_usrRoleRelRep.Exists(r => roleIds.Contains(r.RoleId)))
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("{0}失败，{1}", Msg_DeleteWithRelations, "要删除的角色已分配到用户");
                return result;
            }
            base.UnitOfWork.AutoCommit = false;
            try
            {
                base.UnitOfWork.BeginTransaction();
                _rolePermissionRep.Delete(r => roleIds.Contains(r.RoleId));
                _roleRepository.Delete(r => roleIds.Contains(r.Id));
                base.UnitOfWork.Commit();

                result.ResultType = OperationResultType.Success;
                result.Message = Msg_DeleteWithRelations + "成功";
            }
            catch (Exception ex)
            {
                base.UnitOfWork.Rollback();
                base.ProcessException(result, Msg_DeleteWithRelations + "失败", ex);
            }
            return result;
        }
    }
}
