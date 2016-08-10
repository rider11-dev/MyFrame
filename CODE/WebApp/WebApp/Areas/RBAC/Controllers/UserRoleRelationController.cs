using MyFrame.Infrastructure.OptResult;
using MyFrame.RBAC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Controllers;
using MyFrame.Infrastructure.Extension;
using MyFrame.RBAC.Service.Interface;
using WebApp.Extensions.Filters;

namespace WebApp.Areas.RBAC.Controllers
{
    public class UserRoleRelationController : BaseController
    {
        IUserRoleRelService _userRoleRelSrv;
        public UserRoleRelationController(IUserRoleRelService userRoleRelSrv, IOperationService optSrv)
            : base(optSrv)
        {
            _userRoleRelSrv = userRoleRelSrv;
        }

        [HttpPost]
        [AuthCheck]
        public JsonResult ClearRoles()
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
            OperationResult result = _userRoleRelSrv.Delete(r => usrIds.Contains(r.UserId));
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "清除成功" });
        }

        [HttpPost]
        [AuthCheck]
        public JsonResult Assign(int[] roleIds, int[] usrIds)
        {
            if (usrIds == null || usrIds.Length < 1)
            {
                return Json(new { code = OperationResultType.ParamError, message = "用户不能为空" });
            }
            if (roleIds == null || roleIds.Length < 1)
            {
                return Json(new { code = OperationResultType.ParamError, message = "角色不能为空" });
            }
            OperationResult result = _userRoleRelSrv.AssignToUsers(roleIds, usrIds);
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "角色分配成功" });
        }

        [HttpPost]
        [AuthCheck]
        public JsonResult SetRoles(int[] usrIds, int[] roleIds)
        {
            if (usrIds == null || usrIds.Length < 1)
            {
                return Json(new { code = OperationResultType.ParamError, message = "用户不能为空" });
            }
            if (roleIds == null || roleIds.Length < 1)
            {
                return Json(new { code = OperationResultType.ParamError, message = "角色不能为空" });
            }
            OperationResult result = _userRoleRelSrv.SetRoles(usrIds, roleIds);
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = result.ResultType, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "角色设置成功" });
        }
    }
}