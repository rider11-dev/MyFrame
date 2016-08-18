using MyFrame.Infrastructure.OptResult;
using MyFrame.RBAC.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core.ViewModel;
using WebApp.Controllers;
using WebApp.Core.Service.Interface;
using WebApp.Extensions.Filters;
using WebApp.Extensions.Session;
using MyFrame.Infrastructure.Common;
using WebApp.Core.Models;

namespace WebApp.Areas.Public.Controllers
{
    public class UserDetailsController : BaseController
    {
        IUserDetailsService _usrDetailSrv;
        public UserDetailsController(IUserDetailsService usrDetailSrv, IOperationService optSrv)
            : base(optSrv)
        {
            _usrDetailSrv = usrDetailSrv;
        }

        //
        // GET: /Public/UserDetails/
        [LoginCheck]
        public ActionResult Index()
        {
            var usrId = Session.GetUserId();
            if (usrId == null)
            {
                throw new Exception("未找到登录用户信息");
            }
            var rst = _usrDetailSrv.GetDetailsById(usrId);
            if (rst.ResultType != OperationResultType.Success)
            {
                throw new Exception("未找到登录用户信息" + rst.Message);
            }

            return View(rst.AppendData as UserDetailsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginCheck]
        public JsonResult Save(UserDetailsViewModel usrDetailsVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }
            var usrDetails = OOMapper.Map<UserDetailsViewModel, UserDetails>(usrDetailsVM);
            var rst = _usrDetailSrv.Save(usrDetails);
            if (rst.ResultType != OperationResultType.Success)
            {
                return Json(new { code = rst.ResultType, message = rst.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "保存成功" });
        }
    }
}