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
using MyFrame.RBAC.Service.Interface;

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
        [LoginCheckFilter]
        public JsonResult SavePermission(int roleId, int[] perIds, int perType, int? moduleId = null)
        {
            var permissionType = perType.ConvertTo<PermissionType>();
            if (permissionType == PermissionType.Module)
            {
                //模块权限
                OperationResult rst = _rolePermissionSrv.AssignModulePermissions(roleId, perIds);
                if (rst.ResultType != OperationResultType.Success)
                {
                    return Json(new { code = rst.ResultType, message = rst.Message });
                }
                return Json(new { code = OperationResultType.Success, message = "模块权限分配成功" });
            }
            else if (permissionType == PermissionType.Operation)
            {
                if (moduleId == null)
                {
                    return Json(new { code = OperationResultType.ParamError, message = "模块id不能为空" });
                }
                OperationResult rst = _rolePermissionSrv.AssignOptPermissions(roleId, (int)moduleId, perIds);
                if (rst.ResultType != OperationResultType.Success)
                {
                    return Json(new { code = rst.ResultType, message = rst.Message });
                }
                return Json(new { code = OperationResultType.Success, message = "操作权限分配成功" });
            }
            return Json(new { code = OperationResultType.ParamError, message = "未识别的权限类型" });
        }

        [HttpPost]
        [LoginCheckFilter]
        public JsonResult SaveAllPermission(int roleId, int perType)
        {
            OperationResult rst = new OperationResult();
            var permissionType = perType.ConvertTo<PermissionType>();
            if (permissionType == PermissionType.Module)
            {
                rst = _rolePermissionSrv.AssignAllModulePermissions(roleId);
                if (rst.ResultType != OperationResultType.Success)
                {
                    return Json(new { code = rst.ResultType, message = rst.Message });
                }
            }
            else if (permissionType == PermissionType.Operation)
            {
                rst = _rolePermissionSrv.AssignAllOptPermissions(roleId);
                if (rst.ResultType != OperationResultType.Success)
                {
                    return Json(new { code = rst.ResultType, message = rst.Message });
                }
            }
            else
            {
                return Json(new { code = OperationResultType.ParamError, message = "未识别的权限类型" });
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