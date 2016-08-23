using MyFrame.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Core.Models
{
    [Description("用户详细信息")]
    [Table("UserDetails")]
    public class UserDetails : IKey<int>
    {
        public UserDetails() { }

        [Description("主键")]
        [Key]
        public int Id { get; set; }

        [Description("昵称")]
        public string NickName { get; set; }

        [Description("出生日期")]
        public DateTime? BirthDate { get; set; }

        [Description("年龄")]
        public int? Age { get; set; }

        [Description("联系电话")]
        public string Telephone { get; set; }

        [Description("地址")]
        public string Address { get; set; }

        [Description("兴趣爱好")]
        public string Interests { get; set; }

        [Description("头像源文件相对路径")]
        public string SrcImage { get; set; }

        [Description("头像文件相对路径")]
        public string AvatarImage { get; set; }

        [Description("个人说明")]
        public string PersonalNote { get; set; }
    }
}