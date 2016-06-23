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

        public static string Get(string key)
        {
            if (string.IsNullOrEmpty(key) || !ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return string.Empty;
            }
            return ConfigurationManager.AppSettings[key].ToString();
        }
    }
}
