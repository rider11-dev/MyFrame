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

namespace WebApp.Areas.RBAC.Controllers
{
    public class AccountController : BaseController
    {
        readonly IUserService _usrSrv;
        public AccountController(IUserService usrSrv)
        {
            _usrSrv = usrSrv;
        }
        //
        // GET: /RBAC/User/
        public ActionResult Login()
        {
            return View(new LoginViewModel());
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
                if (result.ResultType == OperationResultType.Success)
                {
                    var usr = (User)result.AppendData;
                    if (usr == null)
                    {
                        ModelState.AddModelError("", "用户名不存在");
                        return View(loginVM);
                    }
                    else
                    {
                        if (EncryptionHelper.GetMd5Hash(loginVM.Password) != usr.Password)
                        {
                            ModelState.AddModelError("", "密码不正确");
                            return View(loginVM);
                        }
                        //登录成功
                        //登记session
                        HttpContext.Session.SetUser(usr);
                        //重定向
                        return RedirectToHome();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "登录失败，" + result.Message);
                    return View(loginVM);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "登录失败，请参考：" + ex.GetDeepestException().Message);
                return View(loginVM);
            }
        }
    }
}