using MyFrame.Core.Repository;
using MyFrame.Core.UnitOfWork;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Repository.Impl
{
    public class OperationRepository : BaseRepository<Operation>, IOperationRepository
    {
        public OperationRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
