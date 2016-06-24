using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MyFrame.Model
{
    /// <summary>
    /// 逻辑删除接口
    /// </summary>
    public interface ILogicalDelete
    {
        bool IsDeleted { get; set; }
    }
}
