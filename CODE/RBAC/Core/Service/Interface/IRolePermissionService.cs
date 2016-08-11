using MyFrame.Infrastructure.OptResult;
using MyFrame.Core.Service;
using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Service.Interface
{
    public interface IRolePermissionService : IBaseService<RolePermission>
    {
        OperationResult AssignModulePermissions(int roleId, int[] moduleIds);
        OperationResult AssignAllModulePermissions(int roleId);
        OperationResult AssignOptPermissions(int roleId, int moduleId, int[] optIds);
        OperationResult AssignAllOptPermissions(int roleId);
        OperationResult CheckPermission(int? moduleId, int? optId = null, bool enableRbac = false);
    }
}
