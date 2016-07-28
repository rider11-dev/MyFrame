using MyFrame.Infrastructure.OptResult;
using MyFrame.IService;
using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Service
{
    public interface IRolePermissionService : IBaseService<RolePermission>
    {
        OperationResult AssignPermissions(int roleId, int[] permissionIds, int type);
    }
}
