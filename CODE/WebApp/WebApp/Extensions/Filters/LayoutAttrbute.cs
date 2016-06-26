using MyFrame.IService.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Extensions.Filters
{
    public class LayoutAttrbute : ActionFilterAttribute
    {
        #region Autofac属性注入,Filter的注入不同于Controller, Controller的注入是通过构造函数注入的，而Filter是通过属性注入的
        public IModuleServiceWrapper _moduleSrvWrapper;

        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

        }

        
    }
}