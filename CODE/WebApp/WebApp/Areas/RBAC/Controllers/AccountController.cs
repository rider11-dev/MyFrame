using MyFrame.Infrastructure.Extension;
using MyFrame.Infrastructure.OptResult;
using MyFrame.RBAC.Service;
using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Controllers;
using WebApp.Extensions.Filters;
using WebApp.Extensions.Session;
using WebApp.ViewModels.RBAC;
using WebApp.Extensions.ActionResult;
using MyFrame.RBAC.Service.Interface;

namespace WebApp.Areas.RBAC.Controllers
{
    public class AccountController : BaseController
    {
        readonly IUserService _usrSrv;
        IUserRoleRelService _usrRoleSrv;
        IRoleService _roleSrv;
        public AccountController(IUserService usrSrv, IUserRoleRelService usrRoleSrv, IRoleService roleSrv, IOperationService optSrv)
            : base(optSrv)
        {
            _usrSrv = usrSrv;
            _usrRoleSrv = usrRoleSrv;
            _roleSrv = roleSrv;
        }
        //
        // GET: /RBAC/User/
        public ActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//防止xss攻击
        public ActionResult Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            try
            {
                var result = _usrSrv.FindByUserName(loginVM.UserName);
                if (result.ResultType != OperationResultType.Success)
                {
                    ModelState.AddModelError("", "登录失败，" + result.Message);
                    return View(loginVM);
                }

                var usr = (User)result.AppendData;
                if (usr == null)
                {
                    ModelState.AddModelError("", "用户名不存在");
                    return View(loginVM);
                }

                if (EncryptionHelper.GetMd5Hash(loginVM.Password) != usr.Password)
                {
                    ModelState.AddModelError("", "密码不正确");
                    return View(loginVM);
                }

                if (!usr.Enabled)
                {
                    ModelState.AddModelError("", "用户未激活");
                    return View(loginVM);
                }
                //设置当前用户
                RBACContext.CurrentUser = usr;
                //登录成功 登记session
                HttpContext.Session.SetUser(usr);
                //角色id
                result = _usrRoleSrv.Find(r => r.UserId == usr.Id);
                if (result.ResultType != OperationResultType.Success)
                {
                    ModelState.AddModelError("", "登录成功，但获取用户角色失败，" + result.Message);
                    return View(loginVM);
                }
                var roleIds = (result.AppendData as List<UserRoleRelation>).Select(r => r.RoleId).ToArray();
                HttpContext.Session.SetRoleIds(roleIds);
                //角色名称
                result = _roleSrv.Find(r => roleIds.Contains(r.Id));
                if (result.ResultType != OperationResultType.Success)
                {
                    ModelState.AddModelError("", "登录成功，但获取用户角色信息失败，" + result.Message);
                    return View(loginVM);
                }
                var roles = result.AppendData as List<Role>;
                HttpContext.Session.SetRoleText((roles != null && roles.Count > 0) ? string.Join(",", roles.Select(r => r.RoleName).ToArray()) : "");

                //重定向
                if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                {
                    return RedirectToHome();
                }
                else
                {
                    return base.Redirect(loginVM.ReturnUrl);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "登录失败，请参考：" + ex.GetDeepestException().Message);
                return View(loginVM);
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            RBACContext.CurrentUser = null;
            return base.RedirectToHome();
        }

        [LoginCheck]
        public ActionResult ChangePwd()
        {
            return PartialView(new ChangePwdViewModel());
        }

        [HttpPost]
        [LoginCheck]
        [ValidateAntiForgeryToken]
        public JsonResult ChangePwd(ChangePwdViewModel changePwdVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }

            var usrId = HttpContext.Session.GetUserId();
            if (usrId == null)
            {
                RedirectToAction("Login");
            }
            var result = _usrSrv.Find(u => u.Id == usrId);
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = OperationResultType.ParamError, message = result.Message });
            }
            var usrs = result.AppendData as List<User>;
            if (usrs == null || usrs.Count < 1)
            {
                return Json(new { code = OperationResultType.QueryNull, message = "用户不存在" });
            }
            if (EncryptionHelper.GetMd5Hash(changePwdVM.OldPassword) != usrs[0].Password)
            {
                return Json(new { code = OperationResultType.ParamError, message = "旧密码不正确" });
            }
            if (EncryptionHelper.GetMd5Hash(changePwdVM.NewPassword) == usrs[0].Password)
            {
                return Json(new { code = OperationResultType.ParamError, message = "新旧密码不能相同" });
            }
            result = _usrSrv.Update(u => u.Id == (int)usrId, u => new User { Password = EncryptionHelper.GetMd5Hash(changePwdVM.NewPassword) });
            if (result.ResultType != OperationResultType.Success)
            {
                return Json(new { code = OperationResultType.Error, message = result.Message });
            }
            return Json(new { code = OperationResultType.Success });
        }

        public JsonResult GetCurrentAccountInfo()
        {
            string userName = HttpContext.Session.GetUserName();
            string roleText = HttpContext.Session.GetRoleText();

            return new JsonNetResult
            {
                Data = new
                {
                    code = OperationResultType.Success,
                    message = "账户信息获取成功",
                    info = new
                    {
                        user = userName,
                        role = roleText
                    }
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}