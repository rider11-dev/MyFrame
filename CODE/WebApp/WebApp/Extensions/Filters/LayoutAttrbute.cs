using MyFrame.Infrastructure.OptResult;
using MyFrame.RBAC.Service;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.RBAC.ViewModel;
using WebApp.Extensions.Session;
using MyFrame.Infrastructure.Common;
using MyFrame.Infrastructure.Pagination;
using WebApp.Areas.RBAC.Controllers;
using MyFrame.RBAC.Service.Interface;
using MyFrame.Infrastructure.Extension;

namespace WebApp.Extensions.Filters
{
    public class LayoutAttrbute : ActionFilterAttribute
    {
        /// <summary>
        /// Autofac属性注入,Filter的注入不同于Controller, Controller的注入是通过构造函数注入的，而Filter是通过属性注入的
        /// </summary>
        public IModuleService ModuleSrv { get; set; }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            //设置菜单树数据源
            (filterContext.Result as ViewResult).ViewData["ModuleTreeData"] = GetModulesForTree(filterContext.HttpContext.Session);
        }

        /// <summary>
        /// 获取所有功能模块，树结构
        /// </summary>
        /// <returns></returns>
        private List<ModuleSimpleViewModel> GetModulesForTree(HttpSessionStateBase session)
        {
            List<ModuleSimpleViewModel> listVM = new List<ModuleSimpleViewModel>();
            var roles = session.GetRoleIds();
            var result = AppContext.EnableRBAC ?
                ModuleSrv.FindByRolesWithSimpleInfo(roles) :
                ModuleSrv.FindByPageWithSimpleInfo(m => m.Enabled,
                                                    query => query.OrderBy(m => m.Code),
                                                    new PageArgs { PageSize = 1000, PageIndex = 1 });
            if (result.ResultType == OperationResultType.Success)
            {
                //获取所有模块数据
                var modules = result.AppendData as List<ModuleSimpleViewModel>;
                listVM = ModuleController.BuildModulesTree(modules);
            }
            return listVM;
        }
    }
}