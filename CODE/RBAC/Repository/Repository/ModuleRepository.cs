using MyFrame.IRepository.RBAC;
using MyFrame.Model.RBAC;
using MyFrame.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Repository.RBAC
{
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        public ModuleRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    }
}
