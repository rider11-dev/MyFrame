using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;
using MyFrame.Model.Unit;
using MyFrame.RBAC.Model;
using MyFrame.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Repository
{
    public class RolePermissionRepository : BaseRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        public void Add(IEnumerable<RolePermission> entities)
        {
            if (entities != null && entities.Count() > 0)
            {
                entities.ToList().ForEach(r => base.Add(r));
            }
        }
    }
}
