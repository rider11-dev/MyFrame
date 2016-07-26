﻿using MyFrame.IService.RBAC;
using MyFrame.Model.RBAC;
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
using MyFrame.ViewModel.RBAC;
using WebApp.Extensions.Session;

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
            Expression<Func<Role, bool>> where = r => r.IsDeleted == false;
            var roleName = HttpContext.Request["RoleName"];
            if (!string.IsNullOrEmpty(roleName))
            {
                where = where.And(r => r.RoleName == roleName);
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

        public ActionResult Add()
        {
            RoleViewModel roleVM = new RoleViewModel();
            return PartialView(roleVM);
        }

        [HttpPost]
        public JsonResult Add(RoleViewModel roleVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }
            OperationResult result = _roleSrv.Add(new Role
            {
                RoleName = roleVM.RoleName,
                Remark = roleVM.Remark,
                Enabled = roleVM.Enabled,
                SortOrder = roleVM.SortOrder,
                Creator = HttpContext.Session.GetUserId(),
                CreateTime = DateTime.Now,
            });
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
            var role = new Role
            {
                Id = roleVM.Id,
                RoleName = roleVM.RoleName,
                Remark = roleVM.Remark,
                Enabled = roleVM.Enabled,
                SortOrder = roleVM.SortOrder,
                LastModifier = HttpContext.Session.GetUserId(),
                LastModifyTime = DateTime.Now
            };
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
            OperationResult result = _roleSrv.Update(u => roleIds.Contains(u.Id), u => new Role { IsDeleted = true });
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "删除成功" });
        }
    }
}