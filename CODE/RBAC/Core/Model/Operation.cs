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
    /// 操作---实体
    /// </summary>
    [Description("模块")]
    [Table("Operations")]
    public class Operation : BaseEntity<int>, IKey<int>
    {
        public Operation() { }

        [Description("操作编号")]
        public string OptCode { get; set; }

        [Description("操作名称")]
        public string OptName { get; set; }

        [Description("操作链接")]
        public string SubmitUrl { get; set; }

        [Description("图标")]
        public string Icon { get; set; }

        [Description("Css类")]
        public string CssClass { get; set; }

        [Description("Css样式")]
        public string CssStyle { get; set; }

        [Description("所属模块")]
        public int ModuleId { get; set; }
        
        [Description("排序号")]
        public int SortOrder { get; set; }

        [Description("是否激活")]
        public bool Enabled { get; set; }

        [Description("备注")]
        public string Remark { get; set; }
    }
}
