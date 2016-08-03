using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyFrame.Core.Repository
{
    /// <summary>
    /// EF数据上下文
    /// </summary>
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("DefaultConnection")
        { }

        public EFDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<EFDbContext>(null);
        }
    }
}
