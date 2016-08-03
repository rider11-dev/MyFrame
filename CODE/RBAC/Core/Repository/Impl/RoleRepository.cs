
using MyFrame.Core.Model;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository;
using MyFrame.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFrame.Core.UnitOfWork;
using MyFrame.RBAC.Repository.Interface;
using System.Linq.Expressions;

namespace MyFrame.RBAC.Repository.Impl
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
