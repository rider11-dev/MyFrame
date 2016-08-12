using EmitMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.Infrastructure.Common
{
    /// <summary>
    /// 对象映射
    /// </summary>
    public class OOMapper
    {
        public static TTo Map<TFrom, TTo>(TFrom from)
        {
            //EmitMapper内部有缓存机制，故不需要再维护Mapper的缓存
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>();
            return mapper.Map(from);
        }
    }
}
