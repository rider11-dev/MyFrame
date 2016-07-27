using MyFrame.Infrastructure.Extension;
using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;
using MyFrame.IRepository;
using MyFrame.IRepository.RBAC;
using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using MyFrame.Model.Unit;
using MyFrame.Repository.RBAC;
using MyFrame.ViewModel.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFrame.Service.RBAC
{
    public class UserService : BaseService<User>, IUserService
    {
        IUserRoleRelRepository _usrRoleRelRepository;
        IRoleRepository _roleRepository;
        IUserRepository _usrRepository;
        const string Msg_BeforeAdd = "保存前校验";
        const string Msg_SetRoles = "设置用户角色";
        public UserService(IUnitOfWork unitOfWork, IUserRepository usrRep, IUserRoleRelRepository usrRoleRelRep, IRoleRepository roleRep)
            : base(unitOfWork)
        {
            _usrRepository = usrRep;
            _usrRoleRelRepository = usrRoleRelRep;
            _roleRepository = roleRep;
        }

        public OperationResult FindByUserName(string userName)
        {
            OperationResult result = new OperationResult();
            if (string.IsNullOrEmpty(userName))
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = "参数错误，用户名不能为空";
                return result;
            }
            try
            {
                var data = _usrRepository.FindByUserName(userName);
                result.ResultType = OperationResultType.Success;
                result.Message = "用户名查询成功";
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                base.ProcessException(result, string.Format("根据用户名获取{0}数据实体出错", base.EntityType), ex);
            }
            return result;
        }

        protected override OperationResult OnBeforeAdd(User entity)
        {
            OperationResult result = new OperationResult();
            if (entity == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("{0}失败，{1}", Msg_BeforeAdd, "实体不能为空");
                return result;
            }
            //1、校验用户是否已存在
            bool check = _usrRepository.Exists(u => u.UserName == entity.UserName);
            if (check)
            {
                result.ResultType = OperationResultType.CheckFailedBeforeProcess;
                result.Message = string.Format("{0}失败，{1}", Msg_BeforeAdd, "用户名已存在");
                return result;
            }
            //2、设置默认密码
            if (entity.Password == null || string.IsNullOrEmpty(entity.Password.Trim()))
            {
                entity.Password = EncryptionHelper.GetMd5Hash("123456");
            }
            result.ResultType = OperationResultType.Success;
            return result;
        }


        public OperationResult UpdateDetail(User usr)
        {
            OperationResult result = new OperationResult();
            if (usr == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = "参数错误，用户实体不能为空";
                return result;
            }
            return base.Update(u => u.Id == usr.Id,
                  u => new User
                  {
                      Email = usr.Email,
                      Phone = usr.Phone,
                      Address = usr.Address,
                      Enabled = usr.Enabled,
                      Remark = usr.Remark,
                      LastModifier = usr.LastModifier,
                      LastModifyTime = usr.LastModifyTime
                  });
        }

        public OperationResult FindByPageWithFullInfo(Expression<Func<User, bool>> where, Action<IOrderable<User>> orderBy, PageArgs pageArgs)
        {
            OperationResult result = new OperationResult();
            try
            {
                //先分页
                var userPaged = _usrRepository.FindByPage(where, orderBy, pageArgs);
                //再连接
                var usrList = (from usr in userPaged
                               join creator in _usrRepository.Entities on usr.Creator equals creator.Id into creatorQuery
                               from c in creatorQuery.DefaultIfEmpty()
                               join lastmodifier in _usrRepository.Entities on usr.LastModifier equals lastmodifier.Id into lastmodifierQuery
                               from m in lastmodifierQuery.DefaultIfEmpty()
                               select new UserDetailViewModel
                               {
                                   Id = usr.Id,
                                   UserName = usr.UserName,
                                   Email = usr.Email,
                                   Phone = usr.Phone,
                                   Address = usr.Address,
                                   Enabled = usr.Enabled,
                                   Remark = usr.Remark,
                                   Creator = usr.Creator,
                                   CreatorName = c.UserName,
                                   CreateTime = usr.CreateTime,
                                   LastModifier = usr.LastModifier,
                                   LastModifierName = m.UserName,
                                   LastModifyTime = usr.LastModifyTime
                               }).ToList();

                var usrIds = usrList.Select(u => u.Id);
                if (usrList != null && usrList.Count > 0)
                {
                    //获取用户角色
                    var roles = (from urRel in _usrRoleRelRepository.Entities
                                 join role in _roleRepository.Entities on urRel.RoleId equals role.Id into roleQuery
                                 from r in roleQuery.DefaultIfEmpty()
                                 where usrIds.Contains(urRel.UserId)
                                 select new { urRel.UserId, r.RoleName })
                                      .ToList();

                    usrList.ForEach(a => a.Roles =
                        string.Join("|",
                        roles.Where(r => r.UserId == a.Id)
                        .Select(r => r.RoleName)
                        .ToArray()));
                }
                result.ResultType = OperationResultType.Success;
                result.AppendData = usrList;
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("分页获取{0}用户详细信息失败", EntityType), ex);
            }
            return result;
        }


        public OperationResult SetRoles(int[] usrIds, int[] roleIds)
        {
            OperationResult result = new OperationResult();
            if (usrIds == null || usrIds.Length < 1)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_SetRoles + "失败，usrIds不能为空";
                return result;
            }
            if (roleIds == null || roleIds.Length < 1)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_SetRoles + "失败，roleIds不能为空";
                return result;
            }
            /*
             * 1、删除用户现有角色
             * 2、添加新角色
             * */
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
                _usrRoleRelRepository.Delete(r => usrIds.Contains(r.UserId));
                _usrRoleRelRepository.Add(query.ToList());
                base.UnitOfWork.Commit();

                result.ResultType = OperationResultType.Success;
            }
            catch (Exception ex)
            {
                base.UnitOfWork.Rollback();
                base.ProcessException(result, Msg_SetRoles + "失败", ex);
            }

            return result;
        }

    }
}
