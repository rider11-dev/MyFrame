using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.RBAC
{
    public class LoginVM
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [Display(Name = "密码")]
        [StringLength(32)]
        public string Password { get; set; }


        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
    }
}