using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.ViewModel
{
    public class OperationViewModel
    {
        [Display(Name = "操作id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "操作编号不能为空")]
        [Description("操作编号")]
        [MaxLength(20, ErrorMessage = "操作编号不能超过{1}个字符")]
        public string OptCode { get; set; }

        [Required(ErrorMessage = "操作名称不能为空")]
        [Description("操作名称")]
        [MaxLength(20, ErrorMessage = "操作名称不能超过{1}个字符")]
        public string OptName { get; set; }

        [Required(ErrorMessage = "操作链接不能为空")]
        [Description("操作链接")]
        [MaxLength(100, ErrorMessage = "操作链接不能超过{1}个字符")]
        public string SubmitUrl { get; set; }

        [Description("图标")]
        [MaxLength(50, ErrorMessage = "图标不能超过{1}个字符")]
        public string Icon { get; set; }

        [Required(ErrorMessage = "所属模块不能为空")]
        [Description("所属模块id")]
        public string ModuleId { get; set; }

        [Description("所属模块")]
        public string ModuleName { get; set; }

        [Description("排序号")]
        public int SortOrder { get; set; }

        [Description("是否激活")]
        public bool Enabled { get; set; }

        [Description("备注")]
        [MaxLength(255, ErrorMessage = "备注不能超过{1}个字符")]
        public string Remark { get; set; }
    }
}
