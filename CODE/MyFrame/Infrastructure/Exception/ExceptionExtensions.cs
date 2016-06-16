using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.Infrastructure.Extension
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 扩展方法——得到最底层异常
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception GetDeepestException(this Exception ex)
        {
            if (ex == null || ex.InnerException == null)
            {
                return ex;
            }
            return ex.InnerException.GetDeepestException();
        }
    }
}
