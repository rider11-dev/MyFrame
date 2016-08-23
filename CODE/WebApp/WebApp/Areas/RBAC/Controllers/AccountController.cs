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
using WebApp.Areas.RBAC.ViewModel;
using WebApp.Extensions.ActionResult;
using MyFrame.RBAC.Service.Interface;
using MyFrame.Infrastructure.Common;
using MyFrame.Infrastructure.Logger;
using WebApp.Extensions.VerifyCodeNotify;
using WebApp.Core.Service.Interface;
using WebApp.Core.Models;

namespace WebApp.Areas.RBAC.Controllers
{
    public class AccountController : BaseController
    {
        IUserService _usrSrv;
        IUserRoleRelService _usrRoleSrv;
        IRoleService _roleSrv;
        IUserDetailsService _usrDetailSrv;
        const string KEY_Session_VerifyCode_Login = "VerifyCode_Login";
        const string KEY_Session_VerifyCode_ChangePwd = "VerifyCode_ChangePwd";
        const string KEY_Session_VerifyCode_ChangePwd_BeginTime = "VerifyCode_ChangePwd_BeginTime";
        const string KEY_Config_VerifyCodeLength = "verifyCodeLength";
        const string KEY_Config_VerifyCodeExpireTime = "verifyCodeExpireTime";
        const string Msg_ChangPwd = "修改密码";
        const string Msg_ChangPwd_Notify = "修改密码时验证码通知";
        int VerifyCodeLength
        {
            get
            {
                return AppSettingHelper.Get(KEY_Config_VerifyCodeLength).ConvertTo<int>(VerificationCodeHelper.DefaultLength);
            }
        }

        int VerifyCodeExpireTime
        {
            get
            {
                return AppSettingHelper.Get(KEY_Config_VerifyCodeExpireTime).ConvertTo<int>(10);//默认有效期10分钟
            }
        }

        ILogHelper<AccountController> _logHelper = LogHelperFactory.GetLogHelper<AccountController>();
        public AccountController(IUserService usrSrv, IUserRoleRelService usrRoleSrv, IRoleService roleSrv, IOperationService optSrv, IUserDetailsService usrDetailSrv)
            : base(optSrv)
        {
            _usrSrv = usrSrv;
            _usrRoleSrv = usrRoleSrv;
            _roleSrv = roleSrv;
            _usrDetailSrv = usrDetailSrv;
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
            //_logHelper.LogInfo(string.Format("session-mode:{0},session-timeout:{1}", Session.Mode.ToString(), Session.Timeout));
            try
            {
                if (string.IsNullOrEmpty(loginVM.VerifyCode) || !string.Equals(loginVM.VerifyCode, Session.Get<string>(KEY_Session_VerifyCode_Login)))
                {
                    ModelState.AddModelError("", "验证码不正确");
                    return View(loginVM);
                }

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
                //获取用户详细信息
                result = _usrDetailSrv.Find(u => u.Id == usr.Id);
                if (result.ResultType == OperationResultType.Success)
                {
                    var usrDetails = (List<UserDetails>)result.AppendData;
                    if (usrDetails != null && usrDetails.Count > 0)
                    {
                        Session.SetUserDetail(usrDetails[0]);

                    }
                }

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

        public ActionResult GetVerifycationCode()
        {
            var verifyCode = VerificationCodeHelper.Create(VerifyCodeLength);
            if (!verifyCode.Check())
            {
                return null;
            }
            Session.Set(KEY_Session_VerifyCode_Login, verifyCode.Code);
            return File(verifyCode.ImageBytes, @"image/jpeg");
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

        public JsonResult EmailNotifyForChangePwd(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { code = OperationResultType.Error, message = Msg_ChangPwd_Notify + "失败，邮箱不能为空" });
            }
            var verifyCode = VerificationCodeHelper.Create(VerifyCodeLength, false);
            if (!verifyCode.Check())
            {
                return Json(new { code = OperationResultType.Error, message = Msg_ChangPwd_Notify + "失败，生成验证码时错误" });
            }
            //验证码以及生成时间写入session
            Session.Set(KEY_Session_VerifyCode_ChangePwd, verifyCode.Code);
            Session.Set(KEY_Session_VerifyCode_ChangePwd_BeginTime, DateTime.Now);

            //调用验证码通知器
            VerifyCodeEmailNotifyer notifier = new VerifyCodeEmailNotifyer();
            notifier.Notify(verifyCode.Code, email);

            return Json(new
            {
                code = OperationResultType.Success,
                message = string.Format("验证码已发送到您的邮箱，有效期{0}分钟，请及时查收。", VerifyCodeExpireTime)
            });
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

            //验证码校验
            if (Session.Get<string>(KEY_Session_VerifyCode_ChangePwd) != changePwdVM.VerifyCode)
            {
                return Json(new { code = OperationResultType.ParamError, message = Msg_ChangPwd + "失败，验证码不正确" });
            }
            var beginTime = Session.Get<DateTime>(KEY_Session_VerifyCode_ChangePwd_BeginTime);
            if (beginTime.AddMinutes(VerifyCodeExpireTime) < DateTime.Now)
            {
                return Json(new { code = OperationResultType.ParamError, message = Msg_ChangPwd + "失败，验证码已失效，请重新获取" });
            }
            //重置验证码session设置
            Session.Set(KEY_Session_VerifyCode_ChangePwd, null);
            Session.Set(KEY_Session_VerifyCode_ChangePwd_BeginTime, null);

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
            string userName = Session.GetUserName();
            string roleText = Session.GetRoleText();
            var usrDetail = Session.GetUserDetail();
            string usrAvatarUrl = usrDetail == null ? "#" : usrDetail.AvatarImage;
            return new JsonNetResult
            {
                Data = new
                {
                    code = OperationResultType.Success,
                    message = "账户信息获取成功",
                    info = new
                    {
                        user = userName,
                        role = roleText,
                        avatar = usrAvatarUrl
                    }
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}