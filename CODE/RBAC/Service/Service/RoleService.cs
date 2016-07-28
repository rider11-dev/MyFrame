using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;

using MyFrame.RBAC.Model;
using MyFrame.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MyFrame.RBAC.Repository;
using MyFrame.Service;

namespace MyFrame.RBAC.Service
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        IRoleRepository _roleRepository;
        IUserRepository _userRepository;
        const string Msg_SearchByName = "根据角色名称查询";
        const string Msg_BeforeAdd = "保存角色前校验";
        const string Msg_UpdateDetail = "更新角色信息";
        const string Msg_SearchFullInfoByPage = "分页获取角色详细信息";
        const string Msg_SearchSimpleInfoByPage = "分页获取角色精简信息";
        public RoleService(IUnitOfWork unitOfWork, IRoleRepository roleRep, IUserRepository userRep)
            : base(unitOfWork)
        {
            _roleRepository = roleRep;
            _userRepository = userRep;
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
    }
}
