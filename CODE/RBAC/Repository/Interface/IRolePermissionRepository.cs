using MyFrame.RBAC.Model;
using MyFrame.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Repository
{
    public interface IRolePermissionRepository : IBaseRepository<RolePermission>, IBaseRepositoryExtend<RolePermission>
    {
    }
}
