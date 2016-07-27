using MyFrame.IRepository;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Repository.RBAC
{
    public interface IUserRoleRelRepository : IBaseRepository<UserRoleRelation>, IBaseRepositoryExtend<UserRoleRelation>
    {
    }
}
