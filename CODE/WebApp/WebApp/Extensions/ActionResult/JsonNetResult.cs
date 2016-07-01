using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Extensions.ActionResult
{
    /// <summary>
    /// 使用json.net序列化相应数据
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        /// <summary>
        /// yyyy-MM-dd、yyyy-MM-dd hh:mm:ss等
        /// </summary>
        public string DateTimeFormat { get; set; }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(base.ContentType) ? base.ContentType : "application/json";
            if (base.ContentEncoding != null)
            {
                response.ContentEncoding = base.ContentEncoding;
            }
            string serializedObject = string.Empty;
            if (!string.IsNullOrEmpty(this.DateTimeFormat))
            {
                //日期格式化转换器
                IsoDateTimeConverter timeFormatConverter = new IsoDateTimeConverter();
                timeFormatConverter.DateTimeFormat = this.DateTimeFormat;
                serializedObject = JsonConvert.SerializeObject(base.Data, Formatting.Indented, timeFormatConverter);
            }
            else
            {
                serializedObject = JsonConvert.SerializeObject(base.Data, Formatting.Indented);
            }

            response.Write(serializedObject);
        }
    }
}