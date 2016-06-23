using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Repository.EF
{
    public interface IEFDbContextProvider
    {
        EFDbContext Generate();
    }
}
