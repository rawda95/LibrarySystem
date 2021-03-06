﻿using LibraryProject.Identity;
using System.Linq;
using System.Web.Mvc;

namespace LibraryProject.Controllers
{
    public class DefaultController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Default
        public ActionResult Index()
        {
            var books = db.Books.OrderByDescending(b => b.dateArived);
            return View(books);
        }
    }
}
