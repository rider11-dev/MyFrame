
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
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        public ModuleRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    }
}
