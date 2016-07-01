using MyFrame.Infrastructure.OptResult;
using MyFrame.IRepository.RBAC;
using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
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
                result.ResultType = OperationResultType.Error;
                result.Message = "用户名查询失败";
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
                entity.Password = "123456";
            }
            result.ResultType = OperationResultType.Success;
            return result;
        }


        public OperationResult UpdateDetail(User usr)
        {
            OperationResult result = new OperationResult();
            if(usr==null)
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
                      IsDeleted = usr.IsDeleted,
                      Enabled = usr.Enabled,
                      Remark = usr.Remark,
                      LastModifier = usr.LastModifier,
                      LastModifyTime = usr.LastModifyTime
                  });
        }
    }
}
