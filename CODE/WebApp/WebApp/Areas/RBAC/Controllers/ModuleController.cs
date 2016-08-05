using MyFrame.Infrastructure.Extension;
using MyFrame.Infrastructure.OptResult;
using MyFrame.RBAC.Service;
using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebApp.Extensions.ActionResult;
using WebApp.Extensions.Filters;
using MyFrame.Infrastructure.Pagination;
using MyFrame.RBAC.ViewModel;
using WebApp.Controllers;
using WebApp.Extensions.Session;

using MyFrame.RBAC.Service.Interface;
using WebApp.ViewModels.RBAC;
using AutoMapper;

namespace WebApp.Areas.RBAC.Controllers
{
    public class ModuleController : BaseController
    {
        IModuleService _moduleSrv;

        public ModuleController(IModuleService moduleSrv)
        {
            _moduleSrv = moduleSrv;
        }

        [LoginCheckFilter]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetModulesFullInfoByPage(int pageNumber, int pageSize)
        {
            Expression<Func<Module, bool>> where = m => true;
            var code = HttpContext.Request["Code"];
            if (!string.IsNullOrEmpty(code))
            {
                where = where.And(m => m.Code.Contains(code));
            }
            var name = HttpContext.Request["Name"];
            if (!string.IsNullOrEmpty(name))
            {
                where = where.And(m => m.Name.Contains(name));
            }
            var pageArgs = new PageArgs { PageSize = pageSize, PageIndex = pageNumber };

            var result = _moduleSrv.FindByPageWithFullInfo(where,
                query => query.OrderBy(m => m.ParentId).ThenBy(m => m.SortOrder),
                pageArgs);

            if (result.ResultType == OperationResultType.Success)
            {
                return new JsonNetResult
                {
                    Data = new { code = result.ResultType, message = "数据获取成功", total = pageArgs.RecordsCount, rows = result.AppendData },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                };
            }
            else
            {
                return Json(new { code = result.ResultType, message = result.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetModulesSimpleInfoByPage(int pageNumber, int pageSize)
        {
            Expression<Func<Module, bool>> where = r => true;//
            var moduleName = HttpContext.Request["ModuleName"];
            if (!string.IsNullOrEmpty(moduleName))
            {
                where = where.And(m => m.Name.Contains(moduleName));
            }
            var pageArgs = new PageArgs { PageSize = pageSize, PageIndex = pageNumber };
            var result = _moduleSrv.FindByPageWithSimpleInfo(where, query => query.OrderBy(r => r.SortOrder), pageArgs);

            if (result.ResultType == OperationResultType.Success)
            {
                var modules = BuildModulesTree(result.AppendData as List<ModuleSimpleViewModel>);
                var formatObj = FormatDataForTreeView(modules);
                return new JsonNetResult
                {
                    Data = new { code = result.ResultType, message = "数据获取成功", total = pageArgs.RecordsCount, rows = formatObj },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                };
            }
            else
            {
                return Json(new { code = result.ResultType, message = result.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 树帮助
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeHelp()
        {
            return PartialView();
        }

        public ActionResult Add()
        {
            var vmModule = new ModuleViewModel();
            return View(vmModule);
        }

        [HttpPost]
        [LoginCheckFilter]
        [ValidateAntiForgeryToken]
        public JsonResult Add(ModuleViewModel vmModule)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }
            var module = Mapper.Map<ModuleViewModel, Module>(vmModule);
            module.Creator = HttpContext.Session.GetUserId();
            module.CreateTime = DateTime.Now;

            OperationResult result = _moduleSrv.Add(module);

            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "添加成功" });
        }

        [HttpPost]
        [LoginCheckFilter]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(ModuleViewModel vmModule)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }
            var module = Mapper.Map<ModuleViewModel, Module>(vmModule);
            module.LastModifier = HttpContext.Session.GetUserId();
            module.LastModifyTime = DateTime.Now;

            OperationResult result = _moduleSrv.UpdateDetail(module);

            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "修改成功" });
        }

        [HttpPost]
        [LoginCheckFilter]
        public JsonResult Delete()
        {
            int[] moduleIds = null;
            using (var reader = new System.IO.StreamReader(HttpContext.Request.InputStream))
            {
                string data = reader.ReadToEnd();
                try
                {
                    moduleIds = data.DeSerializeFromJson<int[]>();
                }
                catch (Exception ex)
                {
                    return Json(new { code = OperationResultType.ParamError, message = ex.GetDeepestException().Message });
                }
            }
            if (moduleIds == null || moduleIds.Length < 1)
            {
                return Json(new { code = OperationResultType.ParamError, message = "模块id列表不能为空" });
            }
            OperationResult result = _moduleSrv.Delete(m => moduleIds.Contains(m.Id));
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "删除成功" });
        }

        public static List<ModuleSimpleViewModel> BuildModulesTree(List<ModuleSimpleViewModel> modules)
        {
            List<ModuleSimpleViewModel> listVM = new List<ModuleSimpleViewModel>();
            if (modules == null || modules.Count < 1)
            {
                return listVM;
            }
            //递归构建菜单树
            var firstList = modules.Where(m => m.ParentId == null).OrderBy(m => m.SortOrder);
            foreach (var vm in firstList)
            {
                SetSubModules(vm, modules);
            }
            //只添加第一层即可
            listVM.AddRange(firstList);

            return listVM;
        }

        public List<ModuleTreeNode> FormatDataForTreeView(IEnumerable<ModuleSimpleViewModel> modules)
        {
            if (modules == null || modules.Count() < 1)
            {
                return new List<ModuleTreeNode>();
            }
            List<ModuleTreeNode> treeNodes = new List<ModuleTreeNode>();
            foreach (var module in modules)
            {
                var node = new ModuleTreeNode { id = module.Id.ToString(), text = module.Name, sort = module.SortOrder };
                if (module.SubModules != null && module.SubModules.Count() > 0)
                {
                    node.nodes = new List<ModuleTreeNode>();
                    node.nodes.AddRange(FormatDataForTreeView(module.SubModules));
                }
                treeNodes.Add(node);
            }
            return treeNodes;
        }

        private static void SetSubModules(ModuleSimpleViewModel parent, List<ModuleSimpleViewModel> srcList)
        {
            if (parent == null || srcList == null || srcList.Count < 1)
            {
                return;
            }
            var subList = srcList.Where(m => m.ParentId == parent.Id).OrderBy(m => m.SortOrder);
            //设置parent
            parent.SubModules = subList.ToList();
            //递归处理subList
            foreach (var vm in parent.SubModules)
            {
                SetSubModules(vm, srcList);
            }
        }
    }
}