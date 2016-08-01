using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.ViewModel
{
    /// <summary>
    /// 模块精简信息模型
    /// </summary>
    public class ModuleSimpleViewModel
    {
        public ModuleSimpleViewModel() { }

        public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string LinkUrl { get; set; }

        public string Icon { get; set; }

        public int SortOrder { get; set; }

        public bool IsMenu { get; set; }

        public int? ParentId { get; set; }

        public bool HasChild { get; set; }

        /// <summary>
        /// 子模块列表
        /// </summary>
        public IEnumerable<ModuleSimpleViewModel> SubModules { get; set; }
    }
}
