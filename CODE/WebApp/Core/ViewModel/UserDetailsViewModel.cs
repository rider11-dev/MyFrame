using MyFrame.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApp.Core.Extensions.Validation;

namespace WebApp.Core.ViewModel
{
    public class UserDetailsViewModel
    {
        [Display(Name = "用户id")]
        public int Id { get; set; }

        [Display(Name = "昵称")]
        [MaxLength(10, ErrorMessage = "昵称不能超过10个字符")]
        public string NickName { get; set; }

        [Display(Name = "出生日期")]
        [ValidDateTime]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "年龄")]
        public int? Age { get; set; }

        [Display(Name = "联系电话")]
        [MaxLength(20, ErrorMessage = "联系电话不能超过20个字符")]
        public string Telephone { get; set; }

        [Display(Name = "地址")]
        [MaxLength(100, ErrorMessage = "地址不能超过100个字符")]
        public string Address { get; set; }

        [Display(Name = "兴趣爱好")]
        [MaxLength(100, ErrorMessage = "兴趣爱好不能超过100个字符")]
        public string Interests { get; set; }

        [Display(Name = "头像")]
        public string HeadImage { get; set; }

        [Display(Name = "个人说明")]
        [MaxLength(100, ErrorMessage = "个人说明不能超过100个字符")]
        public string PersonalNote { get; set; }

        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "邮箱")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50, ErrorMessage = "Email地址不能超过50个字符")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9._]+\.[A-Za-z]{2,4}", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }
    }
}