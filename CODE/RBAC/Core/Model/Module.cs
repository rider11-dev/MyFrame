
using MyFrame.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Model
{
    /// <summary>
    /// 模块---实体
    /// </summary>
    [Description("模块")]
    [Table("Modules")]
    public class Module : BaseEntity<int>, IKey<int>
    {
        public Module()
        {
        }

        [Description("模块编号")]
        public string Code { get; set; }

        [Description("模块名称")]
        public string Name { get; set; }
        [Description("链接地址")]
        public string LinkUrl { get; set; }
        [Description("图标")]
        public string Icon { get; set; }

        [Description("父模块Id")]
        public int? ParentId { get; set; }

        [Description("备注")]
        public string Remark { get; set; }

        [Description("是否激活")]
        public bool Enabled { get; set; }

        [Description("是否系统模块")]
        public bool IsSystem { get; set; }

        [Description("排序号")]
        public int SortOrder { get; set; }

        [Description("创建时间")]
        public DateTime? CreateTime { get; set; }

        [Description("创建人")]
        public int? Creator { get; set; }

        [Description("最后修改人")]
        public int? LastModifier { get; set; }

        [Description("最后修改时间")]
        public DateTime? LastModifyTime { get; set; }

    }
}
