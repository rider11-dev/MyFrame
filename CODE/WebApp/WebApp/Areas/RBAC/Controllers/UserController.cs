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

        //
        // GET: /RBAC/User/
        public ActionResult Index()
        {
            return View();
        }

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
                return Json(new { code = result.ResultType, total = pageArgs.RecordsCount, rows = (result.AppendData as List<User>) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = result.ResultType, error = result.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}