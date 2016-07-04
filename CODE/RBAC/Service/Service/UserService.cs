using MyFrame.Infrastructure.Extension;
using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;
using MyFrame.IRepository;
using MyFrame.IRepository.RBAC;
using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
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
        IUserRepository _usrRepository;
        const string Msg_Error_BeforeAdd = "保存前校验";
        public UserService(IUserRepository usrRep)
            : base(usrRep)
        {
            _usrRepository = usrRep;
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
                result.Message = string.Format("{0}失败，{1}", Msg_Error_BeforeAdd, "实体不能为空");
                return result;
            }
            //1、校验用户是否已存在
            bool check = _usrRepository.Exists(u => u.UserName == entity.UserName);
            if (check)
            {
                result.ResultType = OperationResultType.CheckFailedBeforeProcess;
                result.Message = string.Format("{0}失败，{1}", Msg_Error_BeforeAdd, "用户名已存在");
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
                var query = from usr in userPaged
                            join creator in _usrRepository.Entities on usr.Creator equals creator.Id into creatorQuery
                            from c in creatorQuery.DefaultIfEmpty()
                            join lastmodifier in _usrRepository.Entities on usr.LastModifier equals lastmodifier.Id into lastmodifierQuery
                            from m in lastmodifierQuery.DefaultIfEmpty()
                            select new
                            {
                                Id = usr.Id,
                                UserName = usr.UserName,
                                Email = usr.Email,
                                Phone = usr.Phone,
                                Address = usr.Address,
                                IsDeleted = usr.IsDeleted,
                                Enabled = usr.Enabled,
                                Remark = usr.Remark,
                                Creator = usr.Creator,
                                CreatorName = c.UserName,
                                CreateTime = usr.CreateTime,
                                LastModifier = usr.LastModifier,
                                LastModifierName = m.UserName,
                                LastModifyTime = usr.LastModifyTime,
                            };
                result.ResultType = OperationResultType.Success;
                result.AppendData = query.ToList();
            }
            catch (Exception ex)
            {
                ProcessException(result, string.Format("分页获取{0}用户详细信息失败", EntityType), ex);
            }
            return result;
        }
    }
}
