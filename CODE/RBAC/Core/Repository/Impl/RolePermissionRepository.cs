using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;
using MyFrame.Core.Model;
using MyFrame.RBAC.Model;
using MyFrame.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyFrame.Core.UnitOfWork;
using MyFrame.RBAC.Repository.Interface;

namespace MyFrame.RBAC.Repository.Impl
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
