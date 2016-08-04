using MyFrame.Infrastructure.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Extensions.HttpModule
{
    public class ErrorHandlerModule : IHttpModule
    {
        ILogHelper<ErrorHandlerModule> _logHelper;
        public ErrorHandlerModule()
        {
            _logHelper = LogHelperFactory.GetLogHelper<ErrorHandlerModule>();
            _logHelper.LogInfo("ErrorHandlerModule is constructed");
        }

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.Error += context_Error;
        }

        private void context_Error(object sender, EventArgs e)
        {
            _logHelper.LogInfo("context_Error");
            HttpContext context = HttpContext.Current;
            HttpResponse response = context.Response;
            HttpRequest request = context.Request;

            //获取到HttpUnhandledException异常，这个异常包含一个实际出现的异常
            Exception ex = context.Server.GetLastError();
            _logHelper.LogError(ex);
            //实际发生的异常
            Exception innerEx = ex.InnerException;
            response.Write("来自ErrorModule的错误处理<br />");
            response.Write(innerEx.Message);
            context.Server.ClearError();
        }
    }
}