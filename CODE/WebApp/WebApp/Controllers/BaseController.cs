using MyFrame.Infrastructure.Dynamic;
using MyFrame.Infrastructure.OptResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home", new { Area = "RBAC" });
        }

        protected dynamic BuildJsonObject(OperationResultType resultType, string msg, params KeyValuePair<string, object>[] extraProperties)
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
    }
}