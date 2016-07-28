
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
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        public ModuleRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    }
}
