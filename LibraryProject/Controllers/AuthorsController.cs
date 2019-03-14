﻿using LibraryProject.IRepository;
using LibraryProject.Models;
using System.Net;
using System.Web.Mvc;

namespace LibraryProject.Controllers
{
    public class AuthorsController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        readonly IAuthorRepository authorRepository;
        public AuthorsController(IAuthorRepository _authorRepository)
        {
            authorRepository = _authorRepository;
        }
        // GET: Authors
        public ActionResult Index()
        {
            return View(authorRepository.GetAll());
        }

        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Author author = db.Authors.Find(id);
            Author author = authorRepository.GetById(id);

            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] Author author)
        {
            if (ModelState.IsValid)
            {
                Author _author = authorRepository.Add(author);
                if (_author == null)
                {
                    return View(_author);
                }
                //db.Authors.Add(author);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Author author = db.Authors.Find(id);
            Author author = authorRepository.GetById(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] Author author)
        {
            if (ModelState.IsValid)
            {
                Author _author = authorRepository.Update(author);
                if (_author == null)
                {
                    return View(_author);
                }
                //db.Entry(author).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Author author = db.Authors.Find(id);
            Author author = authorRepository.GetById(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Author author = db.Authors.Find(id);
            //db.Authors.Remove(author);
            //db.SaveChanges();
            Author author = authorRepository.GetById(id);
            authorRepository.Delete(author);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
