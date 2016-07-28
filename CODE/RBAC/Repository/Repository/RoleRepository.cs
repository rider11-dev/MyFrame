
using MyFrame.Model.Unit;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository;
using MyFrame.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.RBAC.Repository
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
