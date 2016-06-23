using MyFrame.Infrastructure.OptResult;
using MyFrame.IService.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Controllers;
using WebApp.ViewModels.RBAC;

namespace WebApp.Areas.RBAC.Controllers
{
    public class UserController : BaseController
    {
        readonly IUserServiceWrapper _usrSrvWrapper;
        public UserController(IUserServiceWrapper usrSrvWrapper)
        {
            _usrSrvWrapper = usrSrvWrapper;
        }
        //
        // GET: /RBAC/User/
        public ActionResult Index()
        {
            return View(new LoginVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//防止xss攻击
        public ActionResult Index(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            try
            {
                var result = _usrSrvWrapper.Find(u => u.UserName == loginVM.UserName);
                if (result.ResultType == OperationResultType.Success)
                {
                    var usr = (UserVM)result.AppendData;

                    //这里需要判断用户角色
                    RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}