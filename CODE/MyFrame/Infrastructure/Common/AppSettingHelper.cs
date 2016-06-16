using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MyFrame.Infrastructure.Common
{
    public class AppSettingHelper
    {
        const string KEY_LOG = "log";
        /// <summary>
        /// 是否记录日志
        /// </summary>
        public static bool Log
        {
            get
            {
                return Convert.ToBoolean(Get(KEY_LOG));
            }
        }

        static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }
    }
}
