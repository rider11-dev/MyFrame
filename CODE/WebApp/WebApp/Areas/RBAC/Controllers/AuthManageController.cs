using MyFrame.Infrastructure.OptResult;
using MyFrame.RBAC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Controllers;
using MyFrame.Infrastructure.Extension;
using MyFrame.RBAC.Model;
using WebApp.Extensions.ActionResult;
using WebApp.Extensions.Filters;

namespace WebApp.Areas.RBAC.Controllers
{
    /// <summary>
    /// 角色权限分配控制器
    /// </summary>
    public class AuthManageController : BaseController
    {
        IRolePermissionService _rolePermissionSrv;
        public AuthManageController(IRolePermissionService rolePermissionSrv)
        {
            _rolePermissionSrv = rolePermissionSrv;
        }

        [LoginCheckFilter]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SavePermission(int roleId, int[] perIds, int perType)
        {
            if (perIds == null || perIds.Length < 1)
            {
                return Json(new { code = OperationResultType.ParamError, message = "权限不能为空" });
            }
            OperationResult rst = _rolePermissionSrv.AssignPermissions(roleId, perIds, perType);
            if (rst.ResultType != OperationResultType.Success)
            {
                return Json(new { code = rst.ResultType, message = rst.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "权限分配成功" });
        }

        public JsonResult GetPermission(int roleId)
        {
            OperationResult rst = _rolePermissionSrv.Find(r => r.RoleId == roleId);
            if (rst.ResultType != OperationResultType.Success)
            {
                return Json(new { code = rst.ResultType, message = rst.Message });
            }
            return new JsonNetResult
                {
                    Data = new { code = rst.ResultType, message = "角色权限获取成功", rows = rst.AppendData },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
        }
    }
}