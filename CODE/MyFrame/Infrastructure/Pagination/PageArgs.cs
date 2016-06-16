using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Infrastructure.Pagination
{
    /// <summary>
    /// 分页参数类
    /// </summary>
    public class PageArgs
    {
        public int PageSize { get; set; }
        /// <summary>
        /// 从1开始
        /// </summary>
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int RecordsCount { get; set; }
    }
}
