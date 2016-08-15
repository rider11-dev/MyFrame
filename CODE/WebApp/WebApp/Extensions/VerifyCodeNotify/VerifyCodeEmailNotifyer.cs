using MyFrame.Infrastructure.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebApp.Extensions.VerifyCodeNotify
{
    /// <summary>
    /// 验证码邮件通知器
    /// </summary>
    public class VerifyCodeEmailNotifyer : IVerifyCodeNotifyer<string>
    {
        public void Notify(string verifyCode, string args)
        {
            if (string.IsNullOrEmpty(verifyCode))
            {
                throw new ArgumentNullException("verifyCode", "验证码不能为空");
            }
            var email = args;
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("args", "验证方式参数不能为空");
            }
            //下面发送邮件通知验证码
            EmailInfo mail = new EmailInfo
            {
                Subject = "MyFrame——密码修改验证码",
                Body = string.Format("您好，本次密码修改验证码：{0}", verifyCode),
                IsBodyHtml = true,
                Priority = MailPriority.High
            };
            mail.Receivers.Add(email);

            EmailHelper.Send(mail);
        }
    }
}