using LibraryProject.Identity;
using LibraryProject.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LibraryProject.Controllers
{
    public class booksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: books
        public ActionResult Index(string txtSearch)
        {


            if (acsessCheck())
            {
                return View("Error");
            }

            dynamic books;
            if (txtSearch != null)
            {
                books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher)
                    .Where(b => b.name.Contains(txtSearch) || b.Publisher.name.Contains(txtSearch) ||
                    b.Author.name.Contains(txtSearch));

            }
            else
            {
                books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher);
            }
            return View(books);
        }












        [HttpPost]
        public JsonResult NameString(string name)
        {

            db.Configuration.ProxyCreationEnabled = false;

            var books = db.Books.Where(b => b.name.Contains(name)).Select(b => b.name);




            var list = JsonConvert.SerializeObject(books,
                 Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                        });

            return Json(books.ToList(), JsonRequestBehavior.AllowGet);

        }







        // GET: books/Details/5
        public ActionResult Details(int? id)
        {

            if (acsessCheck())
            {
                return View("Error");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: books/Create
        public ActionResult Create()
        {

            if (acsessCheck() && !User.IsInRole("Admin"))
            {
                return View("Error");
            }

            ViewBag.author_id = new SelectList(db.Authors, "id", "name");
            ViewBag.category_id = new SelectList(db.Categories, "id", "name");
            ViewBag.publisher_id = new SelectList(db.Publishers, "id", "name");
            return View();
        }

        // POST: books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,PublisherDate,Edition,Pages,Copies,Aviable,ShielfNumber,dateArived,category_id,author_id,publisher_id")] book book)
        {

            if (acsessCheck() && !User.IsInRole("Admin"))
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.author_id = new SelectList(db.Authors, "id", "name", book.author_id);
            ViewBag.category_id = new SelectList(db.Categories, "id", "name", book.category_id);
            ViewBag.publisher_id = new SelectList(db.Publishers, "id", "name", book.publisher_id);
            return View(book);
        }

        // GET: books/Edit/5
        public ActionResult Edit(int? id)
        {

            if (acsessCheck() && !User.IsInRole("Admin"))
            {
                return View("Error");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.author_id = new SelectList(db.Authors, "id", "name", book.author_id);
            ViewBag.category_id = new SelectList(db.Categories, "id", "name", book.category_id);
            ViewBag.publisher_id = new SelectList(db.Publishers, "id", "name", book.publisher_id);
            return View(book);
        }

        // POST: books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,PublisherDate,Edition,Pages,Copies,Aviable,ShielfNumber,dateArived,category_id,author_id,publisher_id")] book book)
        {

            if (acsessCheck() && !User.IsInRole("Admin"))
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.author_id = new SelectList(db.Authors, "id", "name", book.author_id);
            ViewBag.category_id = new SelectList(db.Categories, "id", "name", book.category_id);
            ViewBag.publisher_id = new SelectList(db.Publishers, "id", "name", book.publisher_id);
            return View(book);
        }

        // GET: books/Delete/5
        public ActionResult Delete(int? id)
        {

            if (acsessCheck() && !User.IsInRole("Admin"))
            {
                return View("Error");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {


            if (acsessCheck() && !User.IsInRole("Admin"))
            {
                return View("Error");
            }

            book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




        public bool acsessCheck()
        {
            return (!(User.Identity.IsAuthenticated || (User.IsInRole("Employee")))
                || !(User.Identity.IsAuthenticated || (User.IsInRole("Admin"))));

        }


    }
}
