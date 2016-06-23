
using MyFrame.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Test.entity
{
    [Description("用户信息")]
    public class User : IKey<int>, ILogicalDelete, IDataTrack
    {
        public User()
        {

        }

        [Key]
        [Description("主键")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "用户名")]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "密码")]
        [StringLength(32)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "邮箱")]
        [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "电话")]
        [StringLength(50)]
        public string Phone { get; set; }

        [Display(Name = "地址")]
        [StringLength(300)]
        public string Address { get; set; }

        [Display(Name = "是否激活")]
        public bool Enabled { get; set; }

        [Display(Name = "备注")]
        [StringLength(255)]
        public string Remark { get; set; }

        [Display(Name = "是否已删除")]
        public bool IsDeleted { get; set; }


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
