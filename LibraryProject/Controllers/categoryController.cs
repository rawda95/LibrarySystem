using LibraryProject.IRepository;
using LibraryProject.Models;
using System.Net;
using System.Web.Mvc;

namespace LibraryProject.Controllers
{
    public class categoryController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        readonly ICategoryRepository categoryRepository;
        public categoryController(ICategoryRepository _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }
        // GET: category
        public ActionResult Index()
        {

            //return View(db.Categories.ToList());
            return View(categoryRepository.GetAll());

        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryRepository.GetById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] Category category)
        {
            if (ModelState.IsValid)
            {
                //db.Categories.Add(category);
                //db.SaveChanges();
                Category _category = categoryRepository.Add(category);
                if (_category == null)
                {
                    return View(_category);
                }
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category Category = categoryRepository.GetById(id);
            if (Category == null)
            {
                return HttpNotFound();
            }
            return View(Category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] Category category)
        {
            if (ModelState.IsValid)
            {
                Category _category = categoryRepository.Update(category);
                if (_category == null)
                {
                    return View(_category);
                }
                //db.Entry(category).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryRepository.GetById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryRepository.GetById(id);
            categoryRepository.Delete(category);
            //db.Categories.Remove(category);
            //db.SaveChanges();
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
