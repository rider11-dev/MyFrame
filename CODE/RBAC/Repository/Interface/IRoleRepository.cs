using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;
using MyFrame.RBAC.Model;
using MyFrame.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFrame.RBAC.Repository
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
    }
}
