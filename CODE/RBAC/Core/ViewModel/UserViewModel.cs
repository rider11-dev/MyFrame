using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MyFrame.RBAC.ViewModel
{
    public class UserViewModel
    {
        [Display(Name = "用户id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        [MaxLength(20, ErrorMessage = "用户名不能超过20个字符")]
        public string UserName { get; set; }

        [Display(Name = "邮箱")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50, ErrorMessage = "Email地址不能超过50个字符")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9._]+\.[A-Za-z]{2,4}", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }

        [Display(Name = "是否激活")]
        public bool Enabled { get; set; }

        [Display(Name = "创建人")]
        public int? Creator { get; set; }
        [Display(Name = "创建人")]
        public string CreatorName { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "最后修改人")]
        public int? LastModifier { get; set; }
        [Display(Name = "最后修改人")]
        public string LastModifierName { get; set; }

        [Display(Name = "最后修改时间")]
        public DateTime? LastModifyTime { get; set; }
    }
}
