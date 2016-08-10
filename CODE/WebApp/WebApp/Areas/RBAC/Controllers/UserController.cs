using MyFrame.RBAC.Service;
using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModels.RBAC;
using MyFrame.Infrastructure.Extension;
using MyFrame.Infrastructure.Pagination;
using MyFrame.Infrastructure.OptResult;
using WebApp.Extensions.Session;
using WebApp.Extensions.ActionResult;
using WebApp.Controllers;
using WebApp.Extensions.Filters;
using MyFrame.RBAC.ViewModel;

using AutoMapper;
using MyFrame.RBAC.Service.Interface;

namespace WebApp.Areas.RBAC.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userSrv;

        public UserController(IUserService userSrv, IOperationService optSrv)
            : base(optSrv)
        {
            _userSrv = userSrv;
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

        /// <summary>
        /// 分页获取用户完整信息
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetUserFullInfoByPage(int pageNumber, int pageSize)
        {
            Expression<Func<User, bool>> where = u => true;
            var userName = HttpContext.Request["UserName"];
            if (!string.IsNullOrEmpty(userName))
            {
                where = where.And(u => u.UserName.Contains(userName));
            }
            var pageArgs = new PageArgs { PageSize = pageSize, PageIndex = pageNumber };

            var result = _userSrv.FindByPageWithFullInfo(where, query => query.OrderBy(u => u.UserName), pageArgs);

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

        public JsonResult GetUsersSimpleInfoByPage(int pageNumber, int pageSize)
        {
            Expression<Func<User, bool>> where = u => true;//只能是激活的用户
            var usrName = HttpContext.Request["UserName"];
            if (!string.IsNullOrEmpty(usrName))
            {
                where = where.And(u => u.UserName.Contains(usrName));
            }
            var pageArgs = new PageArgs { PageSize = pageSize, PageIndex = pageNumber };
            var result = _userSrv.FindBySelectorByPage(where,
                u => new
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Remark = u.Remark
                },
                query => query.OrderBy(u => u.UserName),
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
        public ActionResult Add()
        {
            UserViewModel usrVM = new UserViewModel();
            return PartialView(usrVM);
        }

        /// <summary>
        /// 列表帮助
        /// </summary>
        /// <returns></returns>
        public ActionResult GridHelp()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthCheckAttribute]
        public JsonResult Add(UserViewModel usrVM)
        {
            if (!ModelState.IsValid)
            {

                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }

            var usr = Mapper.Map<User>(usrVM);
            usr.Creator = HttpContext.Session.GetUserId();
            usr.CreateTime = DateTime.Now;

            OperationResult result = _userSrv.Add(usr);

            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "添加成功" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthCheckAttribute]
        public JsonResult Edit(UserViewModel usrVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }

            var usr = Mapper.Map<User>(usrVM);
            usr.LastModifier = HttpContext.Session.GetUserId();
            usr.LastModifyTime = DateTime.Now;

            OperationResult result = _userSrv.UpdateDetail(usr);
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "修改成功" });
        }

        [HttpPost]
        [AuthCheckAttribute]
        public JsonResult Delete()
        {
            int[] usrIds = null;
            using (var reader = new System.IO.StreamReader(HttpContext.Request.InputStream))
            {
                string data = reader.ReadToEnd();
                try
                {
                    usrIds = data.DeSerializeFromJson<int[]>();
                }
                catch (Exception ex)
                {
                    return Json(new { code = OperationResultType.ParamError, message = ex.GetDeepestException().Message });
                }
            }
            if (usrIds == null || usrIds.Length < 1)
            {
                return Json(new { code = OperationResultType.ParamError, message = "用户id列表不能为空" });
            }
            OperationResult result = _userSrv.DeleteWithRelations(usrIds);
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "删除成功" });
        }
    }
}