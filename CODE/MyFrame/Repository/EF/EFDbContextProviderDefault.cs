using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Repository.EF
{
    public class EFDbContextProviderDefault : IEFDbContextProvider
    {
        public EFDbContext Generate()
        {
            return new EFDbContext();
        }
    }
}
