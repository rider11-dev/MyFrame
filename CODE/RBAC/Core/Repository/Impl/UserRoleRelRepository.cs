
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

namespace MyFrame.RBAC.Repository.Impl
{
    public class UserRoleRelRepository : BaseRepository<UserRoleRelation>, IUserRoleRelRepository
    {
        public UserRoleRelRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public void AddBatch(IEnumerable<UserRoleRelation> entities)
        {
            if (entities != null && entities.Count() > 0)
            {
                entities.ToList().ForEach(ur => base.Add(ur));
            }
        }
    }
}
