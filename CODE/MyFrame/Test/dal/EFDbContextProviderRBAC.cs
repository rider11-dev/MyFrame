using MyFrame.Repository.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.dal
{
    public class EFDbContextProviderRBAC : IEFDbContextProvider
    {
        public string NameOrConnectionString { get; set; }
        public EFDbContext Generate()
        {
            return new EFDbContextRBAC();
        }
    }
}
