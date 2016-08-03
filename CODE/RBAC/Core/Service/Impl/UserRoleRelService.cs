using MyFrame.Infrastructure.OptResult;
using MyFrame.Core.Model;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository;
using MyFrame.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyFrame.RBAC.Repository.Interface;
using MyFrame.Core.UnitOfWork;
using MyFrame.RBAC.Service.Interface;

namespace MyFrame.RBAC.Service.Impl
{
    public class UserRoleRelService : BaseService<UserRoleRelation>, IUserRoleRelService
    {
        const string Msg_SetRoles = "设置用户角色";
        const string Msg_Assign = "角色分配";

        IUserRoleRelRepository _usrRoleRelRepository;

        public UserRoleRelService(IUnitOfWork unitOfWork, IUserRoleRelRepository usrRoleRelRep)
            : base(unitOfWork)
        {
            _usrRoleRelRepository = usrRoleRelRep;
        }

        public OperationResult SetRoles(int[] usrIds, int[] roleIds)
        {
            return SaveRelations(roleIds, usrIds, false);
        }

        public OperationResult AssignToUsers(int[] roleIds, int[] usrIds)
        {
            return SaveRelations(roleIds, usrIds, true);
        }

        /// <summary>
        /// 保存用户角色关联关系
        /// </summary>
        /// <param name="roleIds">角色</param>
        /// <param name="usrIds">用户</param>
        /// <param name="delStrictly">是否精确删除（即是否按照roleId和usrId删除）</param>
        /// <returns></returns>
        private OperationResult SaveRelations(int[] roleIds, int[] usrIds, bool delStrictly)
        {
            OperationResult result = new OperationResult();
            if (roleIds == null || roleIds.Length < 1)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_Assign + "失败，roleIds不能为空";
                return result;
            }
            if (usrIds == null || usrIds.Length < 1)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_Assign + "失败，usrIds不能为空";
                return result;
            }
            /*
             * 1、删除指定用户指定角色
             * 2、给指定用户添加新角色
             */
            var query = from usr in usrIds
                        join role in roleIds on 1 equals 1
                        select new UserRoleRelation
                        {
                            UserId = usr,
                            RoleId = role
                        };
            base.UnitOfWork.AutoCommit = false;
            base.UnitOfWork.BeginTransaction();
            try
            {
                _usrRoleRelRepository.Delete(r => usrIds.Contains(r.UserId) && (delStrictly ? roleIds.Contains(r.RoleId) : true));
                _usrRoleRelRepository.Add(query.ToList());

                base.UnitOfWork.Commit();

                result.ResultType = OperationResultType.Success;
            }
            catch (Exception ex)
            {
                base.UnitOfWork.Rollback();
                base.ProcessException(result, Msg_Assign + "失败", ex);
            }

            return result;
        }
    }
}
