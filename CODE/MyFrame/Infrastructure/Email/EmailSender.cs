using MyFrame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.Infrastructure.Email
{
    public class EmailSender
    {
        const string Key_Config_Email = "email";
        const string Key_Config_Email_Pwd = "email_pwd";
        static EmailSender _instance;
        public static EmailSender Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                _instance = new EmailSender(AppSettingHelper.Get(Key_Config_Email), AppSettingHelper.Get(Key_Config_Email_Pwd));
                return _instance;
            }
        }
        private EmailSender(string email, string pwd)
        {
            Email = email;
            Password = pwd;
        }
        public string Email { get; set; }

        public string Password { get; set; }

        public bool Check(out string msg)
        {
            msg = string.Empty;
            bool success = true;
            if (string.IsNullOrEmpty(Email))
            {
                msg += "发送者邮箱不能为空";
                success = false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                msg += "发送者邮箱密码不能为空";
                success = false;
            }
            return success;
        }

    }
}
