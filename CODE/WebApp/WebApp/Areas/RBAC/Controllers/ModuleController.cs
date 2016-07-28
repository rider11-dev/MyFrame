﻿using MyFrame.Infrastructure.Extension;
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
using MyFrame.Infrastructure.OrderBy;

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

        public ActionResult Add()
        {
            var vmModule = new ModuleViewModel();
            return View(vmModule);
        }

        [HttpPost]
        public JsonResult Add(ModuleViewModel vmModule)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }
            OperationResult result = _moduleSrv.Add(new Module
            {
                Code = vmModule.Code,
                Name = vmModule.Name,
                LinkUrl = vmModule.LinkUrl,
                Icon = vmModule.Icon,
                IsMenu = vmModule.IsMenu,
                ParentId = vmModule.ParentId,
                Enabled = vmModule.Enabled,
                IsSystem = vmModule.IsSystem,
                SortOrder = vmModule.SortOrder,
                Remark = vmModule.Remark,
                Creator = HttpContext.Session.GetUserId(),
                CreateTime = DateTime.Now
            });
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "添加成功" });
        }

        [HttpPost]
        public JsonResult Edit(ModuleViewModel vmModule)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }
            OperationResult result = _moduleSrv.UpdateDetail(new Module
            {
                Id = vmModule.Id,
                Name = vmModule.Name,
                LinkUrl = vmModule.LinkUrl,
                Icon = vmModule.Icon,
                IsMenu = vmModule.IsMenu,
                ParentId = vmModule.ParentId,
                Enabled = vmModule.Enabled,
                IsSystem = vmModule.IsSystem,
                SortOrder = vmModule.SortOrder,
                Remark = vmModule.Remark,
                LastModifier = HttpContext.Session.GetUserId(),
                LastModifyTime = DateTime.Now
            });
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "修改成功" });
        }
        [HttpPost]
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
    }
}