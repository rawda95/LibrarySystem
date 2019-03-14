using LibraryProject.Identity;
using LibraryProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace LibraryProject.Controllers
{



    public class CategoryBook
    {
        public IEnumerable<book> Books { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<Author> authors { get; set; }

        public IEnumerable<Publisher> publishers { get; set; }

    }


    [Authorize]
    public class MemberController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        // GET: Member
        public ActionResult Index(int? category_id, int? author_id, int? publisher_id, bool? availability, int? year, string txtSearch)
        {
            // int id = int.Parse(Session["id"].ToString());


            // string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(8);

            if (MemeberCheck())
            {
                //return RedirectToAction("Login", "Account");
                return View("Error");

            }

            dynamic books;
            ////Category
            if (category_id != null)
            {
                books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).Where(b => b.category_id == category_id).OrderByDescending(b => b.dateArived);
            }


            //Author
            else if (author_id != null)
            {
                books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).Where(b => b.author_id == author_id).OrderByDescending(b => b.dateArived);
            }


            //Publisher

            else if (publisher_id != null)
            {
                books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).Where(b => b.publisher_id == publisher_id).OrderByDescending(b => b.dateArived);
            }



            //availability
            else if (availability != null)
            {
                books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).Where(b => b.Aviable == availability).OrderByDescending(b => b.dateArived);
            }






            //year 
            else if (year != null)
            {
                books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).Where(b => b.dateArived.Year == year).OrderByDescending(b => b.dateArived);
            }

            else if (txtSearch != null)
            {
                books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).Where(b => b.name.Contains(txtSearch) || b.Author.name.Contains(txtSearch) || b.Publisher.name.Contains(txtSearch) || b.Category.name.Contains(txtSearch)).OrderByDescending(b => b.dateArived);

            }
            else
            {
                books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).OrderByDescending(b => b.dateArived);
            }


            var Categories = db.Categories.ToList();
            var Authors = db.Authors.ToList();
            var Publishers = db.Publishers.ToList();
            CategoryBook categoryBook = new CategoryBook();
            categoryBook.Books = books;
            categoryBook.categories = Categories;
            categoryBook.authors = Authors;
            categoryBook.publishers = Publishers;
            return View(categoryBook);


        }


        [HttpPost]
        public ActionResult Index(string txtSearch)
        {
            return RedirectToAction("Index", new { txtSearch = txtSearch });
        }

        public ActionResult AddRead(int? book_id)
        {
            if (MemeberCheck())
            {
                //return RedirectToAction("Login", "Account");
                return View("Error");


            }
            if (book_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  Session["id"] = 1;
            string userId = User.Identity.GetUserId().ToString();
            //var member = db.Members.Where(m => m.User.Id == userId).FirstOrDefault();


            var book = db.Books.Find(book_id);
            var member = db.Members.Find(userId);
            //var mm = db.Members.ToList();
            db.ReadMembers.Add(new MemberRead { book = book, Member = member, DataReading = DateTime.Now });
            db.SaveChanges();
            //  return View(db.ReadMembers.Where(a => a.MemberId == member_id));
            return RedirectToAction("ReadBooks");
        }

        public ActionResult ReadBooks(int? year, int? month)
        {

            if (MemeberCheck())
            {
                //  return RedirectToAction("Login", "Account");
                return View("Error");


            }

            // Session["id"] = 1;
            string userId = User.Identity.GetUserId().ToString();

            var member = db.Members.Find(userId);

            var books = member.ReadBooks.ToList();



            if (year != null)
            {
                books = books.Where(b => b.DataReading.Year == year).ToList();
            }
            else if (month != null)
            {

                books = books.Where(b => b.DataReading.Month == month).ToList();
            }

            // var a = book.Where(b => b.DataReading.Year == year);
            return View("Readbooks", books.OrderByDescending(b => b.DataReading).ToList());

        }





        public ActionResult borrowdbooks(int? year, int? month, bool? borrowed)
        {

            if (MemeberCheck())
            {
                return View("Error");

                // return RedirectToAction("Login", "Account");

            }

            // Session["id"] = 1;
            string userId = User.Identity.GetUserId().ToString();

            var member = db.Members.Find(userId);

            var books = member.BorrowBooks.ToList();



            if (year != null)
            {
                books = books.Where(b => b.DateBorrow.Year == year).ToList();
            }
            else if (month != null)
            {

                books = books.Where(b => b.DateBorrow.Month == month).ToList();
            }
            else if (borrowed != null)
            {
                books = books.Where(b => b.ReturnBook == borrowed).ToList();

            }

            // var a = book.Where(b => b.DataReading.Year == year);
            return View("borrowedbooks", books.OrderByDescending(b => b.DateBorrow).ToList());

        }








        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AddBorrow(FormCollection date)
        {

            if (MemeberCheck())
            {
                // return RedirectToAction("Login", "Account");
                return View("Error");


            }

            string userId = User.Identity.GetUserId().ToString();
            var member = db.Members.Find(userId);
            if (member.CanBorrow)
            {
                var book = db.Books.Find(int.Parse(date["book_id"]));
                int numOfAvalibelBook = book.Copies - (book.NumOfbrrow + book.NumOfread);

                if (book.Aviable && numOfAvalibelBook > 1)
                {

                    var borrowBook = new BorrowMember()
                    {
                        BookId = int.Parse(date["book_id"]),
                        MemberId = userId,
                        Duration = int.Parse(date["Duration"]),
                        DateBorrow = DateTime.Now,
                        ReturnBook = false,
                        returnDate = DateTime.Today
                    };
                    var BorrowMembersCheck = db.BorrowMembers.Where(mb => mb.BookId == borrowBook.BookId && mb.MemberId == borrowBook.MemberId).FirstOrDefault();

                    //  Session["id"] = 1;
                    //var member = db.Members.Where(m => m.User.Id == userId).FirstOrDefault();
                    //  var book = db.Books.Find(book_id);
                    // var member = db.Members.Find(userId);
                    //var mm = db.Members.ToList();
                    if (BorrowMembersCheck == null)
                    {
                        db.BorrowMembers.Add(borrowBook);

                    }
                    else if (BorrowMembersCheck.ReturnBook)
                    {
                        BorrowMembersCheck.Duration = int.Parse(date["Duration"]);
                        BorrowMembersCheck.DateBorrow = DateTime.Now;
                        BorrowMembersCheck.ReturnBook = false;
                        BorrowMembersCheck.returnDate = DateTime.Today;
                        BorrowMembersCheck.NumoFBorrow += 1;
                    }
                    else
                    {
                        return View("Error");

                        // the book is still with member

                    }
                    //var book = db.Books.Find(int.Parse(date["book_id"]));
                    book.NumOfbrrow = book.NumOfbrrow + 1;
                    numOfAvalibelBook = book.Copies - (book.NumOfbrrow + book.NumOfread);

                    if (numOfAvalibelBook < 1)
                    {

                        book.Aviable = false;
                    }

                    db.SaveChanges();
                    //  return View(db.ReadMembers.Where(a => a.MemberId == member_id));

                    return RedirectToAction("borrowdbooks");
                }
                //book is not  Aviable
                else
                {
                    return View("Error");

                }

            }

            else
            {

                return View("Error");

            }

        }



        public bool MemeberCheck()
        {
            return (!User.Identity.IsAuthenticated || !User.IsInRole("Member"));


        }
    }
}
