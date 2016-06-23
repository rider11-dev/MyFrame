using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MyFrame.Model
{
    /// <summary>
    /// 数据跟踪接口
    /// </summary>
    public interface IDataTrack
    {
        int? Creator { get; set; }

        DateTime? CreateTime { get; set; }

        int? LastModifier { get; set; }

        DateTime? LastModifyTime { get; set; }
    }
}
