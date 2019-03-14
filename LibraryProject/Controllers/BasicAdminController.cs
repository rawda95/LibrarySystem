using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryProject.Controllers
{
    public class BasicAdminController : Controller
    {
        // GET: BasicAdmin
        public ActionResult Index()
        {
            return View();
        }
    }
}