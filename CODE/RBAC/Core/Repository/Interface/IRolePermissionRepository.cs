using MyFrame.RBAC.Model;
using MyFrame.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Repository.Interface
{
    public interface IRolePermissionRepository : IBaseRepository<RolePermission>, IBaseRepositoryExtend<RolePermission>
    {
    }
}
