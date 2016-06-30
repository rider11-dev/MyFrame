using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Extensions.Filters;

namespace WebApp.Areas.RBAC.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /RBAC/Home/
        [LoginCheckFilter]
        [LayoutAttrbute]
        public ActionResult Index()
        {
            return View();
        }
    }
}