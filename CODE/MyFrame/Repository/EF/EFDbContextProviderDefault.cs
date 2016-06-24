using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Repository.EF
{
    public class EFDbContextProviderDefault : IEFDbContextProvider
    {
        public EFDbContext Generate(string nameOrConnectionString = null)
        {
            EFDbContext dbContext = null;
            dbContext = string.IsNullOrEmpty(nameOrConnectionString) ? new EFDbContext() : new EFDbContext(nameOrConnectionString);
            return dbContext;
        }

        public string NameOrConnectionString { get; set; }

        public EFDbContext Generate()
        {
            EFDbContext dbContext = null;
            dbContext = string.IsNullOrEmpty(NameOrConnectionString) ? new EFDbContext() : new EFDbContext(NameOrConnectionString);
            return dbContext;
        }
    }
}
