using MyFrame.IRepository.RBAC;
using MyFrame.Model.RBAC;
using MyFrame.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Repository.RBAC
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
