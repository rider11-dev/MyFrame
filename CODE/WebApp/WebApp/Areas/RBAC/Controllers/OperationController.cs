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

namespace WebApp.Areas.RBAC.Controllers
{
    public class OperationController : BaseController
    {
        IOperationService _optSrv;
        public OperationController(IOperationService optSrv)
        {
            _optSrv = optSrv;
        }

        /// <summary>
        /// 列表界面
        /// </summary>
        /// <returns></returns>
        [LoginCheckFilter]
        public ActionResult Index()
        {
            return View();
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

            var result = _optSrv.FindByPageWithFullInfo(where, query => query.OrderBy(o => o.SortOrder), pageArgs);

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