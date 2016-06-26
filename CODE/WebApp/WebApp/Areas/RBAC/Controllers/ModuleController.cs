using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.RBAC.Controllers
{
    public class ModuleController : Controller
    {
        //
        // GET: /RBAC/Module/
        public ActionResult Index()
        {
            return View();
        }
    }
}