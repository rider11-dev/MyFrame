using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MyFrame.ViewModel.RBAC
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

        [Display(Name = "电话")]
        [MaxLength(50, ErrorMessage = "电话长度不能超过50个字符")]
        public string Phone { get; set; }

        [Display(Name = "地址")]
        [MaxLength(300, ErrorMessage = "地址长度不能超过300个字符")]
        public string Address { get; set; }

        [Display(Name = "是否激活")]
        public bool Enabled { get; set; }

        [Display(Name = "备注")]
        [MaxLength(255, ErrorMessage = "备注不能超过255个字符")]
        public string Remark { get; set; }

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
