using MyFrame.RBAC.Model;
using MyFrame.RBAC.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebApp.Controllers;
using WebApp.Extensions.Filters;
using MyFrame.Infrastructure.Extension;
using MyFrame.Infrastructure.Pagination;
using MyFrame.Infrastructure.OptResult;
using WebApp.Extensions.ActionResult;
using MyFrame.RBAC.ViewModel;
using AutoMapper;

namespace WebApp.Areas.RBAC.Controllers
{
    public class OperationController : BaseController
    {
        public OperationController(IOperationService optSrv)
            : base(optSrv)
        {
        }

        /// <summary>
        /// 列表界面
        /// </summary>
        /// <returns></returns>
        [LoginCheck]
        public ActionResult Index()
        {
            base.SetOptPermissions();
            return View();
        }

        public ActionResult Add()
        {
            return PartialView(new OperationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthCheck]
        public JsonResult Add(OperationViewModel optVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }
            Operation opt = Mapper.Map<OperationViewModel, Operation>(optVM);
            OperationResult result = OptSrv.Add(opt);
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "添加成功" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthCheck]
        public JsonResult Edit(OperationViewModel optVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }
            Operation opt = Mapper.Map<OperationViewModel, Operation>(optVM);
            OperationResult result = OptSrv.UpdateDetail(opt);
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "修改成功" });
        }

        [HttpPost]
        [AuthCheck]
        public JsonResult Delete()
        {
            int[] optIds = null;
            using (var reader = new System.IO.StreamReader(HttpContext.Request.InputStream))
            {
                string data = reader.ReadToEnd();
                try
                {
                    optIds = data.DeSerializeFromJson<int[]>();
                }
                catch (Exception ex)
                {
                    return Json(new { code = OperationResultType.ParamError, message = ex.GetDeepestException().Message });
                }
            }
            if (optIds == null || optIds.Length < 1)
            {
                return Json(new { code = OperationResultType.ParamError, message = "操作id列表不能为空" });
            }
            OperationResult result = OptSrv.Delete(m => optIds.Contains(m.Id));
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "删除成功" });
        }

        /// <summary>
        /// 分页获取指定模块下的操作完整信息
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetOperationFullInfoByPage(int pageNumber, int pageSize)
        {
            Expression<Func<Operation, bool>> where = o => true;
            var moduleId = HttpContext.Request["moduleId"];
            if (!string.IsNullOrEmpty(moduleId))
            {
                var mId = moduleId.ConvertTo<int>(-1);
                where = where.And(o => o.ModuleId == mId);
            }
            var optName = HttpContext.Request["OptName"];
            if (!string.IsNullOrEmpty(optName))
            {
                where = where.And(o => o.OptName.Contains(optName));
            }
            var pageArgs = new PageArgs { PageSize = pageSize, PageIndex = pageNumber };

            var result = OptSrv.FindByPageWithFullInfo(where, query => query.OrderBy(o => o.SortOrder), pageArgs);

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

        public JsonResult GetOptsSimpleInfoByPage(int pageNumber, int pageSize)
        {
            Expression<Func<Operation, bool>> where = r => true;//
            var moduleId = HttpContext.Request["moduleId"];
            if (!string.IsNullOrEmpty(moduleId))
            {
                var mId = moduleId.ConvertTo<int>(-1);
                where = where.And(o => o.ModuleId == mId);
            }

            var optName = HttpContext.Request["OptName"];
            if (!string.IsNullOrEmpty(optName))
            {
                where = where.And(o => o.OptName.Contains(optName));
            }
            var pageArgs = new PageArgs { PageSize = pageSize, PageIndex = pageNumber };
            var result = OptSrv.FindBySelectorByPage(where,
                o => new
                {
                    Id = o.Id,
                    OptCode = o.OptCode,
                    OptName = o.OptName,
                    SortOrder = o.SortOrder,
                    ModuleId = o.ModuleId
                },
                query => query.OrderBy(o => o.SortOrder),
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
    }
}