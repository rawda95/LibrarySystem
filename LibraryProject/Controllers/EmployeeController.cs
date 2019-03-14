using LibraryProject.Identity;
using LibraryProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace LibraryProject.Controllers
{
    public class EmployeeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employee
        public ActionResult Index()
        {

            return View();
        }





        public ActionResult borrowdbooks()
        {

            if (EmployeeCheck())
            {
                return View("Error");
            }

            // Session["id"] = 1;


            var BorrowMembers = db.BorrowMembers.Include(bm => bm.MemberModels.User);
            // var books = db.BorrowMembers.Include(b => b.User).Include(b => b.Category).Include(b => b.Publisher);


            // var a = book.Where(b => b.DataReading.Year == year);
            return View(BorrowMembers.ToList());
        }

        public ActionResult returnBorrowBook(int book_id, string member_id)
        {

            var memberBrow = db.BorrowMembers.Where(b => b.MemberId == member_id && b.BookId == book_id).FirstOrDefault();
            memberBrow.ReturnBook = true;
            memberBrow.returnDate = DateTime.Now;
            var book = db.Books.Find(book_id);

            book.NumOfbrrow -= 1;
            int numOfAvalibelBook = book.Copies - (book.NumOfbrrow + book.NumOfread);
            if (numOfAvalibelBook > 0)
            {
                book.Aviable = true;
            }
            else
            {
                book.Aviable = false;
            }
            db.SaveChanges();



            return RedirectToAction("borrowdbooks");

        }



        public ActionResult readbooks()
        {

            if (EmployeeCheck())
            {
                return View("Error");
            }

            // Session["id"] = 1;


            var readMembers = db.ReadMembers.Include(rm => rm.Member.User);
            // var books = db.BorrowMembers.Include(b => b.User).Include(b => b.Category).Include(b => b.Publisher);


            // var a = book.Where(b => b.DataReading.Year == year);
            return View(readMembers.ToList());
        }


        public ActionResult returnReadBook(int book_id, string member_id)
        {
            if (EmployeeCheck())
            {
                return View("Error");
            }
            var memberBrow = db.ReadMembers.Where(b => b.MemberId == member_id && b.BookId == book_id).FirstOrDefault();
            memberBrow.ReturnBook = true;

            var book = db.Books.Find(book_id);

            book.NumOfread -= 1;

            int numOfAvalibelBook = book.Copies - (book.NumOfbrrow + book.NumOfread);
            if (numOfAvalibelBook > 0)
            {
                book.Aviable = true;
            }
            else
            {
                book.Aviable = false;
            }

            db.SaveChanges();



            return RedirectToAction("readbooks");

        }








        public ActionResult addBorrowBook()
        {
            if (EmployeeCheck())
            {
                return View("Error");
            }
            ViewBag.MemberId = new SelectList(db.Members.Include(m => m.User), "MemeberId", "User.UserName");
            ViewBag.BookId = new SelectList(db.Books, "id", "name");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult addBorrowBook([Bind(Include = "Duration,MemberId,BookId")]BorrowMember item)
        {
            if (EmployeeCheck())
            {
                return View("Error");
            }
            if (ModelState.IsValid)
            {
                var book = db.Books.Find(item.BookId);
                var member = db.Members.Find(item.MemberId);
                var check = db.BorrowMembers.Where(mb => mb.BookId == item.BookId && mb.MemberId == item.MemberId).FirstOrDefault();
                if (member.CanBorrow)
                {
                    int numOfAvalibelBook = book.Copies - (book.NumOfbrrow + book.NumOfread);

                    if (book.Aviable && numOfAvalibelBook > 1)
                    {

                        if (check == null || check.ReturnBook)
                        {
                            //memeber naver borrow the book before 
                            // or borrow but return
                            item.DateBorrow = DateTime.Now;
                            item.returnDate = DateTime.Now; //for ignoer error 
                            item.ReturnBook = false;
                            item.NumoFBorrow = 1;
                            book.NumOfbrrow += 1;
                            db.BorrowMembers.Add(item);

                            book.NumOfbrrow = book.NumOfbrrow + 1;
                            numOfAvalibelBook = book.Copies - (book.NumOfbrrow + book.NumOfread);

                            if (numOfAvalibelBook < 1)
                            {

                                book.Aviable = false;
                            }
                            else
                            {
                                book.Aviable = true;
                            }

                            db.SaveChanges();

                            return RedirectToAction("borrowdbooks");
                            // return View();


                        }
                        else
                        {

                            ViewBag.error = "the member stil borrow the book ";
                            // return View();

                            //  return RedirectToAction("addBorrowBook");


                        }// if book in borrow
                    }// book is avalive 
                    else
                    {
                        ViewBag.error = "book not avalible";
                    }
                }//if memeber can borrow
                else
                {
                    ViewBag.error = "the member cant borrow now ";
                    //        return View();


                }
            }









            /*   else
               {

                   ViewBag.MemberId = new SelectList(db.Members.Include(m => m.User), "MemeberId", "User.UserName", item.MemberId);
                   ViewBag.BookId = new SelectList(db.Books, "id", "name", item.BookId);
                   // return View();

               }
               */
            ViewBag.MemberId = new SelectList(db.Members.Include(m => m.User), "MemeberId", "User.UserName");
            ViewBag.BookId = new SelectList(db.Books, "id", "name");
            return View();



        }


        public ActionResult MemberBorrowLate()
        {
            if (EmployeeCheck())
            {
                return View("Error");
            }
            //var lateMember = (from bm in db.BorrowMembers
            //                  where bm.DateBorrow.AddDays(-bm.Duration) > DateTime.Now
            //                  select bm);

            var lateMember = db.BorrowMembers.Include(Bm => Bm.MemberModels).Include(Bm => Bm.MemberModels.User).ToList();
            List<BorrowMember> lateMemberList = new List<BorrowMember>();
            foreach (var item in lateMember)
            {
                if (item.DateBorrow.AddDays(+item.Duration) < DateTime.Now && !item.ReturnBook)
                {
                    lateMemberList.Add(item);
                }
            }
            //var lateMembfer = db.BorrowMembers.Where(bm => bm.DateBorrow.AddDays(-bm.Duration) < DateTime.Now);
            return View(lateMemberList);
        }



        public ActionResult preventMember(string member_id)
        {
            if (EmployeeCheck())
            {
                return View("Error");
            }
            var member = db.Members.Find(member_id);
            member.CanBorrow = false;
            db.SaveChanges();
            return RedirectToAction("MemberBorrowLate");
        }


        [HttpPost]
        public JsonResult NamesBorrowBooks(string name)
        {

            db.Configuration.ProxyCreationEnabled = false;

            var books = db.BorrowMembers.Where(b => b.book.name.Contains(name)).Select(b => b.book.name);




            var list = JsonConvert.SerializeObject(books,
                 Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                        });

            return Json(books.ToList(), JsonRequestBehavior.AllowGet);

        }






        //for read



        public ActionResult addReadBook()
        {
            if (EmployeeCheck())
            {
                return View("Error");
            }
            ViewBag.MemberId = new SelectList(db.Members.Include(m => m.User), "MemeberId", "User.UserName");
            ViewBag.BookId = new SelectList(db.Books, "id", "name");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult addReadBook([Bind(Include = "MemberId,BookId")]MemberRead item)
        {
            if (EmployeeCheck())
            {
                return View("Error");
            }
            if (ModelState.IsValid)
            {
                var book = db.Books.Find(item.BookId);
                var member = db.Members.Find(item.MemberId);
                var check = db.ReadMembers.Where(mb => mb.BookId == item.BookId && mb.MemberId == item.MemberId).FirstOrDefault();

                int numOfAvalibelBook = book.Copies - (book.NumOfbrrow + book.NumOfread);

                if (book.Aviable && numOfAvalibelBook > 0)
                {

                    if (check == null)
                    {
                        //memeber naver borrow the book before 
                        // or borrow but return
                        item.DataReading = DateTime.Now;
                        item.ReturnBook = false;
                        item.numOfread = 1;
                        book.NumOfread += 1;
                        db.ReadMembers.Add(item);

                        numOfAvalibelBook = book.Copies - (book.NumOfbrrow + book.NumOfread);

                        if (numOfAvalibelBook < 0)
                        {

                            book.Aviable = false;
                        }
                        else
                        {
                            book.Aviable = true;
                        }

                        db.SaveChanges();

                        return RedirectToAction("readbooks");

                        // return View();


                    }
                    else if (check.ReturnBook)
                    {
                        check.DataReading = DateTime.Now;
                        check.ReturnBook = false;
                        check.numOfread += 1;
                        book.NumOfread += 1;
                        if (numOfAvalibelBook < 0)
                        {

                            book.Aviable = false;
                        }
                        else
                        {
                            book.Aviable = true;
                        }

                        db.SaveChanges();

                        return RedirectToAction("readbooks");
                    }
                    else
                    {

                        ViewBag.error = "the member stil read the book ";
                        // return View();

                        //  return RedirectToAction("addBorrowBook");


                    }// if book in borrow
                }// book is avalive 
                else
                {
                    ViewBag.error = "book not avalible";
                }
            }





            ViewBag.MemberId = new SelectList(db.Members.Include(m => m.User), "MemeberId", "User.UserName");
            ViewBag.BookId = new SelectList(db.Books, "id", "name");



            return View();




        }


        public ActionResult MemberReadLate()
        {
            if (EmployeeCheck())
            {
                return View("Error");
            }
            // var lateMember =db.BorrowMembers.Where(bm=>bm.DateBorrow.Subtract(bm.Duration)==DateTime.Now)
            return View();
        }







        public bool EmployeeCheck()
        {
            return (!User.Identity.IsAuthenticated || !User.IsInRole("Employee"));


        }


    }
}
