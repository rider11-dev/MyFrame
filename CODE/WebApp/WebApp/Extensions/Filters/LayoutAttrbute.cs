using MyFrame.Infrastructure.OptResult;
using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModels.RBAC;

namespace WebApp.Extensions.Filters
{
    public class LayoutAttrbute : ActionFilterAttribute
    {
        #region Autofac属性注入,Filter的注入不同于Controller, Controller的注入是通过构造函数注入的，而Filter是通过属性注入的
        public IModuleService ModuleSrv { get; set; }

        #endregion

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            //设置菜单树数据源
            (filterContext.Result as ViewResult).ViewData["ModuleTreeData"] = GetModulesForTree();
        }

        /// <summary>
        /// 获取所有功能模块，树结构
        /// </summary>
        /// <returns></returns>
        private List<ModuleVM> GetModulesForTree()
        {
            List<ModuleVM> listVM = new List<ModuleVM>();
            var result = ModuleSrv.Find(m => m.Enabled == true && m.IsDeleted == false);
            if (result.ResultType == OperationResultType.Success)
            {
                //获取所有模块数据
                var modules = (from module in (result.AppendData as List<Module>)
                               select new ModuleVM
                               {
                                   Id = module.Id,
                                   Code = module.Code,
                                   Name = module.Name,
                                   LinkUrl = module.LinkUrl,
                                   Icon = module.Icon,
                                   SortOrder = module.SortOrder,
                                   IsMenu = module.IsMenu,
                                   ParentId = module.ParentId,
                                   HasChild = module.HasChild,
                                   Enabled = module.Enabled,
                                   IsSystem = module.IsSystem,
                                   Remark = module.Remark,
                                   Creator = module.Creator,
                                   CreateTime = module.CreateTime,
                                   LastModifier = module.LastModifier,
                                   LastModifyTime = module.LastModifyTime
                               }).ToList();
                //递归构建菜单树
                var firstList = modules.Where(m => m.ParentId == null).OrderBy(m => m.SortOrder);
                foreach (var vm in firstList)
                {
                    SetSubModules(vm, modules);
                }
                //只添加第一层即可
                listVM.AddRange(firstList);
            }
            return listVM;
        }

        private void SetSubModules(ModuleVM parent, List<ModuleVM> srcList)
        {
            if (parent == null || srcList == null || srcList.Count < 1 || !parent.HasChild)
            {
                return;
            }
            var subList = srcList.Where(m => m.ParentId == parent.Id).OrderBy(m => m.SortOrder);
            //设置parent
            parent.SubModules = subList.ToList();
            //递归处理subList
            foreach (var vm in subList)
            {
                SetSubModules(vm, srcList);
            }
        }
    }
}