using MyFrame.Infrastructure.Common;
using MyFrame.Infrastructure.Logger;
using MyFrame.Infrastructure.OptResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Infrastructure.Extension
{
    public static class OperationResultExtension
    {

        /// <summary>
        /// 解析操作结果:Success时，获取AppendData并转化为指定类型；其它返回null
        /// </summary>
        /// <typeparam name="T1">解析数据类型</typeparam>
        /// <typeparam name="T2">日志对象使用的类型</typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static T1 Parse<T1, T2>(this OperationResult result)
        {
            if (result == null)
            {
                return default(T1);
            }
            if (result.ResultType == OperationResultType.Success)
            {
                return (T1)result.AppendData;
            }

            ILogHelper<T2> logger = new Log4NetHelper<T2>();
            //日志处理
            if (logger != null && AppSettingHelper.Log)
            {
                if (result.Exception != null)
                {
                    logger.LogError(result.Exception);
                }
                else
                {
                    logger.LogInfo(result.Message);
                }
            }
            return default(T1);
        }
    }
}
