using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Infrastructure.Logger
{
    public class LogHelperProvider : ILogHelperProvider
    {

        public ILogHelper<T> GetLogHelper<T>()
        {
            return new Log4NetHelper<T>();
        }
    }
}
