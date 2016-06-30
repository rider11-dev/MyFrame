using MyFrame.Model.RBAC;
using MyFrame.Repository.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyFrame.IRepository.RBAC
{
    public class EFDbContextRBAC : EFDbContext
    {
        public EFDbContextRBAC()
            : base()
        { }
        public EFDbContextRBAC(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Module> Modules { get; set; }
    }
}
