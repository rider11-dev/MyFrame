using MyFrame.Infrastructure.OptResult;
using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Service.RBAC
{
    public class UserServiceWrapper : BaseServiceWrapper<User>, IUserServiceWrapper
    {
        IUserService usrSrv;
        public UserServiceWrapper(IUserService _usrSrv)
            : base(_usrSrv)
        {
            usrSrv = _usrSrv;
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
                var data = usrSrv.FindByUserName(userName);
                result.ResultType = OperationResultType.Success;
                result.AppendData = data;
            }
            catch (Exception ex)
            {
                base.ProcessException(result, string.Format("根据用户名获取{0}数据实体出错", base.EntityType), ex);
            }
            return result;
        }
    }
}
