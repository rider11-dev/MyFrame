using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MyFrame.RBAC.ViewModel
{
    public class RoleViewModel
    {
        [Display(Name = "角色id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "角色名不能为空")]
        [Display(Name = "角色名")]
        [MaxLength(20, ErrorMessage = "角色名不能超过20个字符")]
        public string RoleName { get; set; }
        [Display(Name = "排序")]
        [RegularExpression(@"\d+", ErrorMessage = "排序必须是数字")]
        [Range(1, 99999)]
        public int SortOrder { get; set; }
        [Display(Name = "备注")]
        [MaxLength(255, ErrorMessage = "备注不能超过255个字符")]
        public string Remark { get; set; }
        [Display(Name = "是否激活")]
        public bool Enabled { get; set; }
        [Display(Name = "是否系统")]
        public bool IsSystem { get; set; }

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
