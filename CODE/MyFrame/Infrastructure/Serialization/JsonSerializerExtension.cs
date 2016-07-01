using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.Infrastructure.Extension
{
    public static class JsonSerializerExtension
    {
        /// <summary>
        /// 序列化obj为json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToJson(this object obj)
        {
            IsoDateTimeConverter timeFormatConverter = new IsoDateTimeConverter();
            timeFormatConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented, timeFormatConverter);
            return json;
        }
        /// <summary>
        /// 将json字符串反序列化为指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T DeSerializeFromJson<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
