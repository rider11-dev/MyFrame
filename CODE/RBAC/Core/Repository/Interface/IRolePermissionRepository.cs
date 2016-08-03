using MyFrame.RBAC.Model;
using MyFrame.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace MyFrame.RBAC.Repository.Interface
{
    public interface IRolePermissionRepository : IBaseRepository<RolePermission>, IBaseRepositoryBatch<RolePermission>
    {
    }
}
