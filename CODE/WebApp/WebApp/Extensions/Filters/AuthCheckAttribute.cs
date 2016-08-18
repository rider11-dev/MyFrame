using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFrame.Infrastructure.Extension;
using MyFrame.RBAC.Service.Interface;
using MyFrame.Infrastructure.OptResult;
using WebApp.Extensions.Session;

namespace WebApp.Extensions.Filters
{
    /// <summary>
    /// 授权检查
    /// </summary>
    public class AuthCheckAttribute : ActionFilterAttribute
    {
        public IRolePermissionService RolePermissionSrv { get; set; }//使用autofac属性注入
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //登录校验
            if (!new LoginCheckAttribute().LoginCheck(filterContext.RequestContext))
            {
                return;
            }
            //权限校验
            var httpContext = filterContext.HttpContext;
            var moduleId = httpContext.Request["module"].ConvertTo<int>();
            var optId = httpContext.Request["opt"].ConvertTo<int>();
            var rst = RolePermissionSrv.CheckPermission(moduleId, optId, AppContext.EnableRBAC);

            if (rst.ResultType != OperationResultType.Success)
            {
                filterContext.Result = new JsonResult { Data = new { code = rst.ResultType, message = rst.Message } };
            }
        }
    }
}