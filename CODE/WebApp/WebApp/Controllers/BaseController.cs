using MyFrame.Infrastructure.Dynamic;
using MyFrame.Infrastructure.OptResult;
using MyFrame.RBAC.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApp.Extensions;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IOperationService OptSrv;
        public BaseController(IOperationService optSrv)
        {
            OptSrv = optSrv;
        }

        protected void SetOptPermissions()
        {
            var rst = OptSrv.GetOptInfoByController(RouteData.Values["controller"].ToString(), AppContext.EnableRBAC);
            if (rst.ResultType == OperationResultType.Success)
            {
                ViewData.Add("Opts", rst.AppendData);
            }
        }

        protected ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home", new { Area = "RBAC" });
        }

        protected dynamic BuildJsonResultObject(OperationResultType resultType, string msg, params KeyValuePair<string, object>[] extraProperties)
        {
            dynamic obj = new MyDynamicObject();
            obj.set("code", resultType);
            obj.set("message", msg);
            foreach (var item in extraProperties)
            {
                obj.set(item.Key, item.Value);
            }

            return obj;
        }

        protected string ParseModelStateErrorMessage(ModelStateDictionary stateDict)
        {
            if (stateDict.IsValid)
            {
                return string.Empty;
            }
            var errorStates = stateDict.Where(s => s.Value.Errors.Count > 0).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var state in errorStates)
            {
                sb.AppendFormat("{0}:", state.Key);
                foreach (var error in state.Value.Errors)
                {
                    sb.AppendFormat("{0},", error.ErrorMessage);
                }
                sb.Append(System.Environment.NewLine);
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}