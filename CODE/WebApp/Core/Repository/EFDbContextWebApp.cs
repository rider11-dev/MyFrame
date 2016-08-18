using MyFrame.RBAC.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Core.Models;

namespace WebApp.Core.Repository
{
    public class EFDbContextWebApp : EFDbContextRBAC
    {
        public EFDbContextWebApp()
            : base()
        {

        }

        public EFDbContextWebApp(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public DbSet<UserDetails> UserDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //设置id非自增
            modelBuilder.Entity<UserDetails>().Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
