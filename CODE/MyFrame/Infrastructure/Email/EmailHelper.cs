using MyFrame.Infrastructure.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyFrame.Infrastructure.Email
{
    public class EmailHelper
    {
        static ILogHelper<EmailHelper> _logHelper = LogHelperFactory.GetLogHelper<EmailHelper>();
        public static void Send(EmailInfo entity)
        {
            string msg = string.Empty;
            if (!EmailSender.Instance.Check(out msg))
            {
                throw new Exception("邮件发送者验证失败:" + msg);
            }

            Check(entity);

            var mail = BuildMessage(entity);

            SmtpClient client = new SmtpClient();
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            client.Host = "smtp." + mail.From.Host;
            client.Credentials = new NetworkCredential(mail.From.Address.Substring(0, mail.From.Address.LastIndexOf('@')), EmailSender.Instance.Password);

            try
            {
                //如果异步发送完成后需要后续操作，则注册client.SendCompleted事件
                client.SendAsync(mail, mail.Subject);//异步发送
            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
            }
        }



        static MailMessage BuildMessage(EmailInfo entity)
        {
            var mail = new MailMessage();

            mail.From = new MailAddress(EmailSender.Instance.Email);
            entity.Receivers.ForEach(r => { mail.To.Add(new MailAddress(r)); });

            mail.Subject = entity.Subject;
            mail.SubjectEncoding = entity.SubjectEncoding == null ? Encoding.UTF8 : entity.SubjectEncoding;
            mail.Body = entity.Body;
            mail.BodyEncoding = entity.BodyEncoding == null ? Encoding.UTF8 : entity.BodyEncoding;
            mail.IsBodyHtml = entity.IsBodyHtml;
            mail.Priority = entity.Priority;

            return mail;
        }

        static void Check(EmailInfo entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "邮件实体对象不能为空");
            }

            if (entity.Receivers == null || entity.Receivers.Count < 1)
            {
                throw new ArgumentNullException("entity.Receivers", "接收邮箱不能为空");
            }

            Regex regex = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            bool illegalEmail = entity.Receivers.Any(email =>
               {
                   return !regex.IsMatch(email);
               });
            if (illegalEmail)
            {
                throw new ArgumentNullException("entity.Receivers", "接收邮箱格式不正确");
            }

            if (string.IsNullOrEmpty(entity.Subject))
            {
                throw new ArgumentNullException("entity.Subject", "邮件主题不能为空");
            }
        }

    }
}
