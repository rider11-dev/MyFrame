using MyFrame.RBAC.Service;
using MyFrame.RBAC.Model;
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
using WebApp.Extensions.Session;
using AutoMapper;

namespace WebApp.Areas.RBAC.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleSrv;
        public RoleController(IRoleService roleSrv)
        {
            _roleSrv = roleSrv;
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

        public JsonResult GetRoleFullInfoByPage(int pageNumber, int pageSize)
        {
            Expression<Func<Role, bool>> where = r => true;
            var roleName = HttpContext.Request["RoleName"];
            if (!string.IsNullOrEmpty(roleName))
            {
                where = where.And(r => r.RoleName.Contains(roleName));
            }
            var pageArgs = new PageArgs { PageSize = pageSize, PageIndex = pageNumber };
            var result = _roleSrv.FindByPageWithFullInfo(where, query => query.OrderBy(r => r.SortOrder), pageArgs);

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

        public JsonResult GetRolesSimpleInfoByPage(int pageNumber, int pageSize)
        {
            Expression<Func<Role, bool>> where = r => true;//
            var roleName = HttpContext.Request["RoleName"];
            if (!string.IsNullOrEmpty(roleName))
            {
                where = where.And(r => r.RoleName.Contains(roleName));
            }
            var pageArgs = new PageArgs { PageSize = pageSize, PageIndex = pageNumber };
            var result = _roleSrv.FindByPageWithSimpleInfo(where, query => query.OrderBy(r => r.SortOrder), pageArgs);

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
            RoleViewModel roleVM = new RoleViewModel();
            return PartialView(roleVM);
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
        public JsonResult Add(RoleViewModel roleVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }

            var role = Mapper.Map<Role>(roleVM);
            role.Creator = HttpContext.Session.GetUserId();
            role.CreateTime = DateTime.Now;

            OperationResult result = _roleSrv.Add(role);

            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "添加成功" });
        }

        [HttpPost]
        public JsonResult Edit(RoleViewModel roleVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, messgae = base.ParseModelStateErrorMessage(ModelState) });
            }

            var role = Mapper.Map<Role>(roleVM);
            role.LastModifier = HttpContext.Session.GetUserId();
            role.LastModifyTime = DateTime.Now;

            var rst = _roleSrv.UpdateDetail(role);

            if (rst.ResultType != OperationResultType.Success)
            {
                return Json(new { code = rst.ResultType, message = rst.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "修改成功" });
        }
        [HttpPost]
        public JsonResult Delete()
        {
            int[] roleIds = null;
            using (var reader = new System.IO.StreamReader(HttpContext.Request.InputStream))
            {
                string data = reader.ReadToEnd();
                try
                {
                    roleIds = data.DeSerializeFromJson<int[]>();
                }
                catch (Exception ex)
                {
                    return Json(new { code = OperationResultType.ParamError, message = ex.GetDeepestException().Message });
                }
            }
            if (roleIds == null || roleIds.Length < 1)
            {
                return Json(new { code = OperationResultType.ParamError, message = "角色id列表不能为空" });
            }
            OperationResult result = _roleSrv.DeleteWithRelations(roleIds);
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "删除成功" });
        }
    }
}