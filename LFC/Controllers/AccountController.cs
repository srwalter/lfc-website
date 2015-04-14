using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using PagedList;

using LFC.Models;
using LFC.DAL;
using LFC.ViewModels;

namespace LFC.Controllers
{
    public class LFCUserManager : UserManager<ApplicationUser>
    {
        public LFCUserManager() : base(new UserStore<ApplicationUser>(new LFCContext()))
        {
            var provider = new DpapiDataProtectionProvider("LFC");
            PasswordValidator = new MinimumLengthValidator(4);
            UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("Reset"));
        }
    }

    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AccountController() : this(new LFCUserManager())
        {
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new LFCContext()));
        }

        private AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        
        public ActionResult Index(int? page, bool? retired=false)
        {
            var db = new LFCContext();
            var users = db.Users;
            var model = new List<EditUserViewModel>();
            List<ApplicationUser> list;
            if (retired == true)
            {
                list = users.Where(n => n.MemberType == ApplicationUser.MembershipType.Retired).OrderBy(n => n.LastName).ToList();
            }
            else
            {
                list = users.Where(n => n.MemberType != ApplicationUser.MembershipType.Retired).OrderBy(n => n.LastName).ToList();
            }
            foreach (var user in list)
            {
                var u = new EditUserViewModel(user);
                model.Add(u);
            }
            int pagesize = 20;
            int pagenumber = (page ?? 1);
            if (page < 0)
            {
                pagenumber = 1;
                pagesize = 5000;
            }
            return View(model.ToPagedList(pagenumber, pagesize));
        }

        public ActionResult CSV()
        {
            String data = "Last Name,First Name,Middle Initial,Username,Membership Type,Billing Name,Home Phone,Office Phone,Officer,Badge Expires,City,States,ZIP,Address\r\n";
            var db = new LFCContext();

            foreach (var u in db.Users)
            {
                data += u.LastName + ",";
                data += u.FirstName + ",";
                data += u.MiddleInitial + ",";
                data += u.UserName + ",";
                data += u.MemberType + ",";
                data += u.ShortName + ",";
                data += u.HomeTel + ",";
                data += u.OfficeTel + ",";
                data += u.Officer + ",";
                data += u.BadgeExpires + ",";
                data += u.City + ",";
                data += u.State + ",";
                data += u.ZipCode + ",";
                data += u.Address;
                data += "\r\n";
            }

            var result = new FileContentResult(System.Text.Encoding.UTF8.GetBytes(data), "text/csv");
            result.FileDownloadName = "LFCMembers.csv";
            return result;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            var db = new LFCContext();
            var user = db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit (EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new LFCContext();
                var user = db.Users.First(u => u.UserName == model.UserName);
                user.Email = model.Email;
                user.ShortName = model.ShortName;
                user.LastName = model.LastName;
                user.MiddleInitial = model.MiddleInitial;
                user.FirstName = model.FirstName;
                user.HomeTel = model.HomeTel;
                user.OfficeTel = model.OfficeTel;
                user.Address = model.Address;
                user.City = model.City;
                user.State = model.State;
                user.ZipCode = model.ZipCode;
                user.Certificate = model.Certificate;
                user.MemberType = model.MemberType;
                user.Safety = model.SafetyPilot;
                user.Instrument = model.Instrument;
                user.BadgeExpires = model.BadgeExpires;

                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Details(string id)
        {
            var db = new LFCContext();
            var user = db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id = null)
        {
            var db = new LFCContext();
            var user = db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            var db = new LFCContext();
            var user = db.Users.First(u => u.UserName == id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UserRoles(string id)
        {
            var db = new LFCContext();
            //_roleManager.Create(new IdentityRole("Admin"));
            var user = db.Users.First(u => u.UserName == id);
            var model = new SelectUserRolesViewModel(user);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult UserRoles(SelectUserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new LFCContext();
                var user = db.Users.First(u => u.UserName == model.UserName);
                foreach (var r in UserManager.GetRoles(user.Id))
                {
                    UserManager.RemoveFromRole(user.Id, r);
                }
                foreach (var r in model.Roles)
                {
                    if (r.Selected)
                    {
                        UserManager.AddToRole(user.Id, r.RoleName);
                    }
                }
                return RedirectToAction("Index");
            }
            return View();
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

            var username = model.Email;
            if (username.Contains('@'))
            {
                var user = UserManager.FindByEmail(username);
                if (user != null)
                {
                    username = user.UserName;
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(username, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
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

        //
        // GET: /Account/Register
        //[AllowAnonymous]
        [Authorize(Roles="Admin")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        //[AllowAnonymous]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    ShortName = model.ShortName,
                    LastName = model.LastName,
                    MiddleInitial = model.MiddleInitial,
                    FirstName = model.FirstName,
                    HomeTel = model.HomeTel,
                    OfficeTel = model.OfficeTel,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    MemberType = model.MembershipType,
                    Certificate = model.CertificateType,
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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
            var result = await UserManager.ConfirmEmailAsync(userId, code);
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
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
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

        [Authorize(Roles="Admin")]
        public ActionResult ChangePassword (String username)
        {
            var model = new ChangePasswordAdminViewModel();
            model.UserID = username;
            return View(model);
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword (ChangePasswordAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var id = model.UserID;
                var user = await UserManager.FindByNameAsync(model.UserID);
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                await UserManager.ResetPasswordAsync(user.Id, code, model.NewPassword);
                return RedirectToAction("Index");
            }
            return View(model);
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
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
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
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
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