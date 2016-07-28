using MyFrame.Repository.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.RBAC.Repository
{
    public class EFDbContextProviderRBAC : IEFDbContextProvider
    {
        public EFDbContextProviderRBAC()
        { }
        public EFDbContextProviderRBAC(string nameOrConnectionString)
        {
            NameOrConnectionString = nameOrConnectionString;
        }

        public EFDbContext Generate()
        {
            EFDbContext dbContext = string.IsNullOrEmpty(NameOrConnectionString) ? new EFDbContextRBAC() : new EFDbContextRBAC(NameOrConnectionString);

            return dbContext;
        }

        public string NameOrConnectionString { get; set; }
    }
}
