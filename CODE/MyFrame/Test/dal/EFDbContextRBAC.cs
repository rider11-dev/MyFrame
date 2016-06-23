using MyFrame.Repository.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Test.entity;

namespace Test.dal
{
    public class EFDbContextRBAC : EFDbContext
    {
        public DbSet<User> Users { get; set; }
    }
}
