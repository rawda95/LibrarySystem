using LibraryProject.Identity;
using LibraryProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LibraryProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        ApplicationDbContext db = new ApplicationDbContext();

        //public AccountController()
        //{
        //}

        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set
        //    {
        //        _signInManager = value;
        //    }
        //}

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}


        public ActionResult Details(string id)
        {

            //var data = db.Employees.Include(a=>a.User).SingleOrDefault(p => p.EmployeeId == id );
            //var data = db.Users.Include(a => a.EmployeeModel).SingleOrDefault(a => a.Id == id);
            //RegisterViewModel registerViewModel = new RegisterViewModel() {
            //    UserName = data.UserName,
            //    Email = data.Email,
            //    FirstName = data.FirstName,
            //    LastName = data.LastName,
            //    Address = data.Address,
            //    Phone = data.PhoneNumber
            //};
            if (User.Identity.GetUserId().ToString().Equals(id))// Edit Profile
            {
                var roleId = db.Users.Include(z => z.Roles).SingleOrDefault(z => z.Id == id).Roles.FirstOrDefault().RoleId;
                Session["roleName"] = db.Roles.SingleOrDefault(z => z.Id == roleId).Name;

            }
            dynamic data, hireDate = DateTime.MaxValue, salary = double.MaxValue;
            if (!User.IsInRole("Member"))
            {
                if (Session["roleName"].ToString() != "Member")
                {
                    data = db.Users.Include(a => a.EmployeeModel).SingleOrDefault(p => p.Id == id);
                    hireDate = data.EmployeeModel.HireDate;
                    salary = data.EmployeeModel.Salary;
                }
                else
                {
                    data = db.Users.SingleOrDefault(p => p.Id == id);

                }


            }
            else
            {
                data = db.Users.SingleOrDefault(p => p.Id == id);


            }

            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                UserName = data.UserName,
                Email = data.Email,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Address = data.Address,
                Phone = data.PhoneNumber,
                HireDate = hireDate,
                Salary = salary,


            };
            if (User.Identity.GetUserId().ToString().Equals(id))// Edit Profile
                return View(data);
            return PartialView(registerViewModel);

            //return View(registerViewModel);

        }

        public ActionResult Edit(string id)
        {
            ViewBag.ModelValid = TempData["ModelValid"];
            if (User.Identity.GetUserId().ToString().Equals(id))// Edit Profile
            {
                var roleId = db.Users.Include(z => z.Roles).SingleOrDefault(z => z.Id == id).Roles.FirstOrDefault().RoleId;
                Session["roleName"] = db.Roles.SingleOrDefault(z => z.Id == roleId).Name;

            }
            ViewBag.ModelValid = "";
            dynamic data, hireDate = DateTime.MaxValue, salary = double.MaxValue;
            if (!User.IsInRole("Member"))
            {


                if (Session["roleName"].ToString() != "Member")
                {
                    data = db.Users.Include(a => a.EmployeeModel).SingleOrDefault(p => p.Id == id);
                    hireDate = data.EmployeeModel.HireDate;
                    salary = data.EmployeeModel.Salary;
                }
                else
                {
                    data = db.Users.SingleOrDefault(p => p.Id == id);

                }


            }
            else
            {
                data = db.Users.SingleOrDefault(p => p.Id == id);


            }

            UpdateViewModel registerViewModel = new UpdateViewModel()
            {
                UserName = data.UserName,
                Email = data.Email,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Address = data.Address,
                Phone = data.PhoneNumber,
                HireDate = hireDate,
                Salary = salary,


            };
            string image = Convert.ToBase64String(data.Photo);
            ViewBag.imgSrc = string.Format("data:0; base64, {1}", MimeMapping.GetMimeMapping(image), image);


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            if (User.Identity.GetUserId().ToString().Equals(id))
            {// Edit Profile
                return View(registerViewModel);

            }
            return PartialView("_Edit", registerViewModel);

            //return View(registerViewModel);

        }
        [HttpPost]
        public ActionResult Update([Bind(Exclude = "Photo")]UpdateViewModel model, HttpPostedFileBase Photo)
        {
            bool flag = checkRegister(model);
            if (flag)
            {
                TempData["ModelValid"] = "Username Or Email Register Before";
                ViewBag.ModelValid = TempData["ModelValid"];

                if (User.Identity.GetUserId().ToString().Equals(model.Id))// Edit Profile
                {

                    return View("Edit", model);


                }
                else
                {
                    return PartialView("_Data", Index());
                }

            }
            //bool flag = false;
            if (ModelState.IsValid)
            {

                if (!flag)
                {
                    dynamic data;
                    if (!User.IsInRole("Member"))
                    {
                        if (User.IsInRole("BasicAdmin"))
                        {
                            string msg = "Basic Admin Update.........." + Session["roleName"];
                            //  Email.Send("BasicAdmin@gmail.com", "Update", msg);
                        }
                        if (Session["roleName"].ToString() != "Member")
                        {
                            data = db.Users.Include(a => a.EmployeeModel).SingleOrDefault(p => p.Id == model.Id);
                            if (!User.Identity.GetUserId().ToString().Equals(model.Id))// Edit Profile
                            {
                                data.EmployeeModel.Salary = model.Salary;
                                data.EmployeeModel.HireDate = (DateTime)model.HireDate;
                            }

                        }
                        else
                        {
                            data = db.Users.SingleOrDefault(p => p.Id == model.Id);

                        }


                    }
                    else
                    {
                        data = db.Users.SingleOrDefault(p => p.Id == model.Id);


                    }
                    data.PhoneNumber = model.Phone;
                    data.FirstName = model.FirstName;
                    data.LastName = model.LastName;
                    data.UserName = model.UserName;
                    data.Email = model.Email;
                    if (Photo != null)
                    {
                        data.Photo = new byte[Photo.ContentLength];
                        Photo.InputStream.Read(data.Photo, 0, Photo.ContentLength);
                    }

                    db.SaveChanges();


                }

            }
            else
            {
                //return PartialView("_Data", null);


            }

            if (User.Identity.GetUserId().ToString().Equals(model.Id))// Edit Profile
            {

                return RedirectToAction("Index", "Manage", new { id = User.Identity.GetUserId() });


            }
            if (Session["roleName"].ToString() != null)
            {
                //return RedirectToAction("Index", "Account", new { view = Request.QueryString["edit"] });
                return PartialView("_Data", Index());

            }
            else//Update My Profile
                return RedirectToAction("Index", "Manage");
        }
        public ActionResult Delete(string id)
        {
            Session["roleName"] = "Admin";
            var bm = db.BorrowMembers.Where(q => q.MemberId == id).ToList();
            db.BorrowMembers.RemoveRange(bm);

            var br = db.ReadMembers.Where(q => q.MemberId == id).ToList();
            db.ReadMembers.RemoveRange(br);
            db.SaveChanges();
            if (Session["roleName"].ToString() != "Member")
            {
                EmployeeModel employee = db.Employees.SingleOrDefault(p => p.EmployeeId == id);
                db.Employees.Remove(employee);

            }
            else
            {
                db.Members.Remove(db.Members.SingleOrDefault(p => p.MemeberId == id));
            }



            db.SaveChanges();

            var user = _userManager.FindById(id);
            var Role = _userManager.GetRoles(id);
            _userManager.RemoveFromRole(id, Role[0]);
            db.SaveChanges();

            _userManager.Delete(user);



            db.SaveChanges();
            return PartialView("_Data", Index());

            //return RedirectToAction("Index", "Account", new { view = Request.QueryString["delete"] });
        }


        public ActionResult AcceptMember(string id)
        {

            var member = db.Users.SingleOrDefault(p => p.Id == id);
            member.FlagId = 1;
            db.SaveChanges();
            return PartialView("_Data", Index());
            //return RedirectToAction("Index", "Account", new { view = Request.QueryString["accept"] });
        }

        //public ActionResult Index()
        //{

        //    string roleName;
        //    if (Request.QueryString["view"] != null)
        //        roleName = Request.QueryString["view"];
        //    else
        //    {
        //        roleName = ViewBag.roleName;
        //    }
        //    ViewBag.roleName = roleName;
        //    TempData["roleName"] = roleName;
        //    var roleId = db.Roles.SingleOrDefault(r => r.Name == roleName).Id;
        //    dynamic data;
        //    if (!User.IsInRole("Member"))
        //    {
        //        data = db.Users.Include(a => a.EmployeeModel).Where(a => a.Roles.Any(x => x.RoleId == roleId));
        //    }
        //    else
        //    {
        //        data = db.Users.Where(a => a.Roles.Any(x => x.RoleId == roleId));

        //    }

        //    return View(data);
        //}
        public List<ApplicationUser> Index()
        {

            string roleName;
            if (Session["roleName"].ToString() != null)
                roleName = Session["roleName"].ToString();
            else
            {
                roleName = ViewBag.roleName;
            }
            ViewBag.roleName = roleName;
            TempData["roleName"] = roleName;
            var roleId = db.Roles.SingleOrDefault(r => r.Name == roleName).Id;
            List<ApplicationUser> data = null;
            if (!User.IsInRole("Member"))
            {
                if (Session["roleName"].ToString() != "Member")
                {
                    data = db.Users.Include(a => a.EmployeeModel).Where(a => a.Roles.Any(x => x.RoleId == roleId)).ToList();
                }
                else
                {
                    data = db.Users.Where(a => a.Roles.Any(x => x.RoleId == roleId)).ToList();

                }

            }
            else
            {
                data = db.Users.Where(a => a.Roles.Any(x => x.RoleId == roleId)).ToList();

            }

            return data;
        }

        [HttpPost]
        public ActionResult Index(string txtSearch)
        {
            var roleName = TempData["roleName"].ToString();
            ViewBag.roleName = roleName;
            TempData["roleName"] = roleName;
            var roleId = db.Roles.SingleOrDefault(r => r.Name == roleName).Id;
            dynamic data, searched;
            if (Session["roleName"].ToString() != "Member")
            {
                data = db.Users.Include(a => a.EmployeeModel).Where(a => a.Roles.Any(x => x.RoleId == roleId));

                if (txtSearch == null)
                    searched = db.Users.Where(a => a.Roles.Any(x => x.RoleId == roleId));
                else
                {
                    searched = db.Users.Where(a => a.Roles.Any(x => x.RoleId == roleId)).Where((a => a.FirstName.Contains(txtSearch))).ToList();
                    if (searched.Count == 0)
                        searched = db.Users.Where(a => a.Roles.Any(x => x.RoleId == roleId)).Where((a => a.LastName.Contains(txtSearch)));

                }

            }
            else
            {
                data = db.Users.Where(a => a.Roles.Any(x => x.RoleId == roleId));
                if (txtSearch == null)
                    searched = db.Users.Where(a => a.Roles.Any(x => x.RoleId == roleId));

                else
                {
                    searched = db.Users.Where(a => a.Roles.Any(x => x.RoleId == roleId)).Where((a => a.FirstName.Contains(txtSearch))).ToList();
                    if (searched.Count == 0)
                        searched = db.Users.Where(a => a.Roles.Any(x => x.RoleId == roleId)).Where((a => a.Email.Contains(txtSearch)));

                }


            }

            return PartialView("_Data", searched);
        }

        public ActionResult GetUsers(string pattern)
        {
            pattern = pattern.ToLower();
            var roleName = TempData["roleName"].ToString();
            TempData["roleName"] = roleName;
            ViewBag.roleName = roleName;
            var roleId = db.Roles.SingleOrDefault(r => r.Name == roleName).Id;
            dynamic data, searched;
            List<string> vv = new List<string>();
            List<string> searchLower = new List<string>();
            if (Session["roleName"].ToString() != "Member")
            {

                if (pattern == null)
                {
                    searched = (from N in db.Users
                                where N.Roles.Any(x => x.RoleId == roleId)
                                select new { N.FirstName });
                    foreach (var item in searched)
                    {
                        vv.Add(item.FirstName);
                    }

                    searched = (from N in db.Users
                                where N.Roles.Any(x => x.RoleId == roleId)
                                select new { N.LastName });
                    foreach (var item in searched)
                    {
                        vv.Add(item.LastName);
                    }

                }

                else
                {
                    searched = (from N in db.Users
                                where N.FirstName.Contains(pattern) && N.Roles.Any(x => x.RoleId == roleId)
                                select new { N.FirstName });
                    foreach (var item in searched)
                    {
                        if (!vv.Contains(item.FirstName))
                            vv.Add(item.FirstName);
                    }

                    searched = (from N in db.Users
                                where N.LastName.Contains(pattern) && N.Roles.Any(x => x.RoleId == roleId)
                                select new { N.LastName });
                    foreach (var item in searched)
                    {

                        if (!vv.Contains(item.LastName))

                            vv.Add(item.LastName);
                    }
                }

            }
            else
            {
                if (pattern == null)
                {
                    searched = (from N in db.Users
                                where N.Roles.Any(x => x.RoleId == roleId)
                                select new { N.FirstName });
                    foreach (var item in searched)
                    {

                        if (!vv.Contains(item.FirstName))

                            vv.Add(item.FirstName);
                    }

                    searched = (from N in db.Users
                                where N.Roles.Any(x => x.RoleId == roleId)
                                select new { N.Email });
                    foreach (var item in searched)
                    {

                        vv.Add(item.Email);
                    }
                }

                else
                {
                    searched = (from N in db.Users
                                where N.FirstName.ToLower().Contains(pattern) && N.Roles.Any(x => x.RoleId == roleId)
                                select new { N.FirstName });
                    foreach (var item in searched)
                    {
                        if (!vv.Contains(item.FirstName))

                            vv.Add(item.FirstName);
                    }

                    searched = (from N in db.Users
                                where N.Email.ToLower().Contains(pattern) && N.Roles.Any(x => x.RoleId == roleId)
                                select new { N.Email });
                    foreach (var item in searched)
                    {
                        vv.Add(item.Email);
                    }
                }




            }


            return Json(vv, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// To Check if userName or Email Register Before
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool checkRegister(RegisterViewModel model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();

            bool flag = false;

            foreach (var item in db.Users)
            {
                if (actionName.Equals("Update") && item.Id.Equals(model.Id))
                {
                    continue;
                }
                if (item.UserName.ToLower().Equals(model.UserName.ToLower()) || item.Email.ToLower().Equals(model.Email.ToLower()))
                {
                    flag = true;
                    break;
                }

            }
            return flag;
        }

        private bool checkRegister(UpdateViewModel model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();

            bool flag = false;

            foreach (var item in db.Users)
            {
                if (actionName.Equals("Update") && item.Id.Equals(model.Id))
                {
                    continue;
                }
                if (item.UserName.ToLower().Equals(model.UserName.ToLower()) || item.Email.ToLower().Equals(model.Email.ToLower()))
                {
                    flag = true;
                    break;
                }

            }
            return flag;
        }



        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int flag = 1;
            string roleId = db.Roles.SingleOrDefault(role => role.Name == "Member").Id;
            var loginValid = db.Users.Where(user => user.UserName == model.UserName).SingleOrDefault(user => user.Roles.Any(b => b.RoleId == roleId));
            if (loginValid != null)
                flag = loginValid.FlagId;
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            if (flag != 0)
            {


                ////var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);          
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                var roId = db.Users.Include(z => z.Roles).SingleOrDefault(z => z.UserName == model.UserName).Roles.FirstOrDefault().RoleId;
                var roName = db.Roles.SingleOrDefault(z => z.Id == roleId).Name;

                switch (result)
                {
                    case SignInStatus.Success:

                        if (roName.Equals("Member"))
                        {
                            return RedirectToAction("Index", "member", null);
                        }
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await _signInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()

        {

            string roleName = Request.QueryString["add"];

            Session["roleName"] = roleName;



            if (User.Identity.IsAuthenticated)
            {
                UserAddTableModel _UserAddTableModel = new UserAddTableModel();
                _UserAddTableModel.ApplicationUsers = Index();

                _UserAddTableModel.RegisterViewModel = new RegisterViewModel();
                return View(_UserAddTableModel);
            }
            return View("RegisterAnony");
        }

        public ActionResult ViewUser()
        {

            string roleName = Request.QueryString["view"];
            Session["roleName"] = roleName;
            if (User.Identity.IsAuthenticated)
            {
                UserAddTableModel _UserAddTableModel = new UserAddTableModel();
                _UserAddTableModel.ApplicationUsers = Index();
                _UserAddTableModel.UpdateViewModel = new UpdateViewModel();
                return View(_UserAddTableModel);
            }
            return View("RegisterAnony");
        }

        public ActionResult CancelEdit()
        {


            return View("_Add", new RegisterViewModel());
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Exclude = "Photo")]RegisterViewModel model, HttpPostedFileBase Photo)
        {
            string roleName = Request.QueryString["add"];
            Session["roleName"] = roleName;
            bool flag = checkRegister(model);
            if (ModelState.IsValid)
            {

                if (Photo != null)
                {
                    model.Photo = new byte[Photo.ContentLength];
                    Photo.InputStream.Read(model.Photo, 0, Photo.ContentLength);
                }
                else
                {
                    // Get image path  
                    string imgPath = Server.MapPath("~/Images/default.png");
                    // Convert image to byte array  
                    model.Photo = System.IO.File.ReadAllBytes(imgPath);
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone,
                    Address = model.Address,
                    Photo = model.Photo
                };
                //var user = new ApplicationUser
                //{
                //    UserName = "BasicAdmin",
                //    Email = model.Email,
                //    FirstName = model.FirstName,
                //    LastName = model.LastName,
                //    PhoneNumber = model.Phone,
                //    Address = model.Address,
                //    Photo = model.Photo
                //};
                var result = await _userManager.CreateAsync(user, model.Password);
                //await UserManager.AddToRoleAsync(user.Id, "BasicAdmin");

                if (result.Succeeded && !flag)
                {
                    // Send Email Config
                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);

                    string msg = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to comlete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, Request.Url.Scheme));
                    // Email.Send("BasicAdmin@gmail.com", "Mail Configuration", msg);



                    //Add Users
                    if (Request.QueryString["add"] == "" || Request.QueryString["add"] == null || Request.QueryString["add"] == "Member")
                    {
                        roleName = "Member";
                        MemberModels member = new MemberModels() { status_id = 0, CanBorrow = false, DatedExceed = DateTime.Now, MemeberId = user.Id };

                        db.Members.Add(member);
                        db.SaveChanges();
                    }

                    await _userManager.AddToRoleAsync(user.Id, roleName);
                    if (!User.IsInRole("Member") && User.Identity.IsAuthenticated)
                    {
                        EmployeeModel employee = new EmployeeModel() { Salary = model.Salary, HireDate = (DateTime)model.HireDate, EmployeeId = user.Id };

                        db.Employees.Add(employee);
                        db.SaveChanges();
                    }


                    if (Request.QueryString["add"] == "" || Request.QueryString["add"] == null)// Anony
                    {
                        return RedirectToAction("Login", "Account", new { view = roleName });
                    }
                    return PartialView("_Data", Index());
                }
                AddErrors(result);
                ViewBag.ModelValid = result.Errors.First().ToString();

            }

            // If we got this far, something failed, redisplay form
            //return View(model);
            if (User.Identity.IsAuthenticated)
            {
                UserAddTableModel _UserAddTableModel = new UserAddTableModel();
                _UserAddTableModel.ApplicationUsers = Index();

                _UserAddTableModel.RegisterViewModel = new RegisterViewModel();
                // return new HttpStatusCodeResult(500);
                return PartialView("_Data", Index());
            }
            return View("RegisterAnony", model);
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);       
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await _signInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await _signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
