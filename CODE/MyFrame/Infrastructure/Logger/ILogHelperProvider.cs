using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Infrastructure.Logger
{
    public interface ILogHelperProvider
    {
        ILogHelper<T> GetLogHelper<T>();
    }
}
