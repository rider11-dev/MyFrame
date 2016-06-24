using MyFrame.Infrastructure.OptResult;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.IService.RBAC
{
    public interface IUserServiceWrapper : IBaseServiceWrapper<User>
    {
        OperationResult FindByUserName(string userName);
    }
}
