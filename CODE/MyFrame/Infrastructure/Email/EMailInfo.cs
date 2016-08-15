using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.Infrastructure.Email
{
    /// <summary>
    /// 邮件实体类
    /// </summary>
    public class EmailInfo
    {
        public List<string> Receivers { get; private set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
        public Encoding SubjectEncoding { get; set; }
        public string Body { get; set; }
        public Encoding BodyEncoding { get; set; }
        public bool IsBodyHtml { get; set; }
        public MailPriority Priority { get; set; }

        public EmailInfo()
        {
            Receivers = new List<string>();
        }
    }
}
