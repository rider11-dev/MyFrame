using MyFrame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyFrame.Infrastructure.Extension;

namespace WebApp.Extensions
{
    public class AppContext
    {
        const string KEY_EFProfiler = "EFProfiler";

        /// <summary>
        /// 是否开启MiniProfiler
        /// </summary>
        public static bool EFProfilerEnable
        {
            get
            {
                var val = AppSettingHelper.Get(KEY_EFProfiler);
                return val.ConvertTo<Boolean>(false);
            }
        }
    }
}