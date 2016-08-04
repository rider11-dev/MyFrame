using MyFrame.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MyFrame.RBAC.Model
{
    [Description("角色信息")]
    [Table("Roles")]
    public class Role : BaseEntity<int>, IKey<int>
    {
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name = "是否激活")]
        public bool Enabled { get; set; }
        [Display(Name = "排序")]
        public int SortOrder { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "创建人")]
        public int? Creator { get; set; }

        [Display(Name = "最后修改人")]
        public int? LastModifier { get; set; }

        [Display(Name = "最后修改时间")]
        public DateTime? LastModifyTime { get; set; }
    }
}
