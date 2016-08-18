using MyFrame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Extensions.Session;
using MyFrame.Infrastructure.Extension;
using System.Web.Routing;

namespace WebApp.Extensions.Filters
{
    /// <summary>
    /// 登录校验过滤器
    /// 需要在FilterConfig中注册
    /// </summary>
    public class LoginCheckAttribute : ActionFilterAttribute
    {
        const string KEY_LOGIN_CHECK = "logincheck";
        /// <summary>
        /// 是否开启登录校验
        /// </summary>
        bool EnableCheck
        {
            get
            {
                var val = AppSettingHelper.Get(KEY_LOGIN_CHECK);
                return val.ConvertTo<Boolean>(false);
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            LoginCheck(filterContext.RequestContext);
        }

        public bool LoginCheck(RequestContext requestContext)
        {
            bool pass = true;
            if (EnableCheck)
            {
                if (requestContext.HttpContext.Session.GetUserId() == null)
                {
                    pass = false;
                    var loginUrl = new UrlHelper(requestContext).Action("Login", "Account", new { Area = "RBAC" });
                    requestContext.HttpContext.Response.Write(string.Format("<script language='javascript'>parent.window.open('{0}', '_top')</script>", loginUrl));
                    //loginUrl + "?returnUrl=" + requestContext.HttpContext.Request.RawUrl));
                    requestContext.HttpContext.Response.End();
                }
            }
            return pass;
        }
    }
}