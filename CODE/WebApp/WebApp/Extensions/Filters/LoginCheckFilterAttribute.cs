using MyFrame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Extensions.Session;

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
                bool check = false;
                Boolean.TryParse(AppSettingHelper.Get(KEY_LOGIN_CHECK), out check);
                return check;
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (EnableCheck)
            {
                if (filterContext.HttpContext.Session[SessionConfig.KEY_USER_ID] == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/RBAC/Account/Index");
                }
            }
        }
    }
}