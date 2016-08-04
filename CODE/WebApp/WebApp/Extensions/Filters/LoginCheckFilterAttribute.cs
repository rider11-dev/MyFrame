using MyFrame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Extensions.Session;
using MyFrame.Infrastructure.Extension;

namespace WebApp.Extensions.Filters
{
    /// <summary>
    /// 登录校验过滤器
    /// 需要在FilterConfig中注册
    /// </summary>
    public class LoginCheckFilterAttribute : ActionFilterAttribute
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

            if (EnableCheck)
            {
                if (filterContext.HttpContext.Session.GetUserId() == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/RBAC/Account/Login");
                }
            }
        }
    }
}