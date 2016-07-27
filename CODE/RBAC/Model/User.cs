
using MyFrame.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyFrame.Model.RBAC
{
    [Description("用户信息")]
    [Table("Users")]
    public class User : IKey<int>, IDataTrack
    {
        public User()
        {

        }

        [Key]
        [Description("主键")]
        public int Id { get; set; }

        [Display(Name = "用户名")]
        public string UserName { get; set; }


        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Display(Name = "电话")]
        public string Phone { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "是否激活")]
        public bool Enabled { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "创建人")]
        public int? Creator { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "最后修改人")]
        public int? LastModifier { get; set; }

        [Display(Name = "最后修改时间")]
        public DateTime? LastModifyTime { get; set; }
    }
}
