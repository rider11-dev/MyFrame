﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Extensions.Validation;

namespace WebApp.ViewModels.RBAC
{
    /// <summary>
    /// 密码修改模型
    /// </summary>
    public class ChangePwdViewModel
    {
        [Required(ErrorMessage = "旧密码不能为空")]
        [Display(Name = "旧密码")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "新密码不能为空")]
        [Display(Name = "新密码")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "新密码长度必须在{2}和{1}之间")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "请确认新密码")]
        [Display(Name = "密码确认")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配")]
        public string NewPassword2 { get; set; }
    }
}