
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
    public class UserRoleRelRepository : BaseRepository<UserRoleRelation>, IUserRoleRelRepository
    {
        public UserRoleRelRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public void Add(IEnumerable<UserRoleRelation> entities)
        {
            if (entities != null && entities.Count() > 0)
            {
                entities.ToList().ForEach(ur => base.Add(ur));
            }
        }
    }
}
