using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModels.RBAC;
using MyFrame.Infrastructure.Extension;
using MyFrame.Infrastructure.Expression;
using MyFrame.Infrastructure.Pagination;
using MyFrame.Infrastructure.OptResult;

namespace WebApp.Areas.RBAC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServiceWrapper _userSrvWrapper;

        public UserController(IUserServiceWrapper userSrvWrapper)
        {
            _userSrvWrapper = userSrvWrapper;
        }

        /// <summary>
        /// 列表界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetUsersByPage(int pageNumber, int pageSize)
        {
            Expression<Func<User, bool>> where = u => u.IsDeleted == false;
            var userName = HttpContext.Request["UserName"];
            if (!string.IsNullOrEmpty(userName))
            {
                where = where.And(u => u.UserName.Contains(userName));
            }
            var pageArgs = new PageArgs { PageSize = pageSize, PageIndex = pageNumber };
            var result = _userSrvWrapper.FindByPage(where, new List<OrderByArgs<User>>()
            {
                new OrderByArgs<User>{
                    Expression = u => u.UserName,
                    OrderByType = OrderByType.Asc
                }
            }, pageArgs);

            if (result.ResultType == OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = "数据获取成功", total = pageArgs.RecordsCount, rows = (result.AppendData as List<User>) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = result.ResultType, message = result.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Add()
        {
            UserVM usrVM = new UserVM()
            {
                Enabled = true,
                IsDeleted = false
            };
            return PartialView(usrVM);
        }

        [HttpPost]
        public JsonResult Add(UserVM usrVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = "参数不合法" });
            }



            return Json(new { code = OperationResultType.Success, message = "添加成功" });
        }
    }
}