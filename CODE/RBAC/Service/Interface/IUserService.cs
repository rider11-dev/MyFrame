using MyFrame.Infrastructure.OptResult;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.IService.RBAC
{
    public interface IUserService : IBaseService<User>
    {
        OperationResult FindByUserName(string userName);

        OperationResult UpdateDetail(User usr);
    }
}
