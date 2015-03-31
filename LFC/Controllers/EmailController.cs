using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;

using LFC.Models;
using LFC.ViewModels;
using LFC.DAL;

namespace LFC.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        private LFCContext db = new LFCContext();

        private void SendBadgeReminders()
        {
            var body = "Your Airport Operation Area badge for Bluegrass Airport is scheduled to expire in the next 30 days.  Please ensure you renew it before it expires to avoid paying a penalty.";
            var smtp = new SmtpClient();

            var deadline = DateTime.Now.AddDays(30);
            var users = db.Users.Where(x => (x.BadgeExpires ?? DateTime.MaxValue) == deadline).ToList();
            foreach (var x in users)
            {
                var message = new MailMessage();
                message.From = new MailAddress("info@lexingtonflyingclub.org", "Lexington Flying Club");
                message.Subject = "LFC Airport Badge Expiring";

                var view = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Plain);
                message.AlternateViews.Add(view);

                String pres = (from u in db.Users
                               where u.Officer == ApplicationUser.OfficerTitle.President
                               select u.Email).First();
                String treasurer = (from u in db.Users
                                    where u.Officer == ApplicationUser.OfficerTitle.Treasurer
                                    select u.Email).First();
                message.CC.Add(pres);
                message.CC.Add(treasurer);
                message.To.Add(x.Email);

                ViewBag.Message += x.Email;
                smtp.Send(message);
            }
        }

        [AllowAnonymous]
        public ActionResult SendTimedEmails()
        {
            try
            {
                SendBadgeReminders();
            }
            catch (Exception e)
            {
                ViewBag.Message = "Failed to send: " + e.ToString();
            }
            return View("Index", "MembersArea");
        }

        // GET: Email

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            EmailViewModel model = new EmailViewModel();
            model.Recipients = new List<Recipients> { Recipients.All };
            return View(model);
        }

        private IQueryable<ApplicationUser> getEmailGroup (Recipients group)
        {
            IQueryable<ApplicationUser> users = db.Users;

            switch (group)
            {
                case Recipients.All:
                    // nothing to do
                    break;

                case Recipients.Full:
                    users = users.Where(x => x.MemberType == ApplicationUser.MembershipType.Full);
                    break;

                case Recipients.Restricted:
                    users = users.Where(x => x.MemberType == ApplicationUser.MembershipType.Restricted);
                    break;

                case Recipients.Special:
                    users = users.Where(x => x.MemberType == ApplicationUser.MembershipType.Special);
                    break;

                case Recipients.Inactive:
                    users = users.Where(x => x.MemberType == ApplicationUser.MembershipType.Inactive);
                    break;

                case Recipients.Officers:
                    users = users.Where(x => x.Officer != null);
                    users = users.Union(db.Airplanes.Where(x => x.Active == true).Select(x => x.MaintenanceOfficer));
                    break;

                case Recipients.Associate:
                    users = users.Where(x => x.MemberType == ApplicationUser.MembershipType.Associate);
                    break;

                case Recipients.MaintenanceOfficers:
                    users = db.Airplanes.Where(x => x.Active == true).Select(x => x.MaintenanceOfficer);
                    break;

                case Recipients.SafetyOfficer:
                    users = users.Where(x => x.Officer == ApplicationUser.OfficerTitle.SafetyOfficer);
                    break;

                case Recipients.LineOfficers:
                    users = users.Where(x => x.Officer != null);
                    break;

                case Recipients.DiamondFlyers:
                    users = db.AirplaneCheckouts.Where(x => x.AirplaneID == "N213DS").Select(x => x.Pilot);
                    break;

                default:
                    ViewBag.Message = "Error: unsupported recipient type: " + group;
                    return null;
            }

            return users;
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        public class MultipleButtonAttribute : ActionNameSelectorAttribute
        {
            public string Name { get; set; }
            public string Argument { get; set; }

            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                var isValidName = false;
                var keyValue = string.Format("{0}:{1}", Name, Argument);
                var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

                if (value != null)
                {
                    controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                    isValidName = true;
                }

                return isValidName;
            }
        }

        [Authorize(Roles = "Admin")]
        [MultipleButton(Name = "action", Argument = "AddMore")]
        public ActionResult AddMore(EmailViewModel model)
        {
            model.Recipients.Add(Recipients.All);
            return View("Index", model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [MultipleButton(Name = "action", Argument = "Send")]
        public ActionResult Send(EmailViewModel model)
        {
            if (model.Body == null)
            {
                ViewBag.Message = "Can't send empty message";
                return View("Index", model);
            }
            var message = new MailMessage();
            message.From = new MailAddress("info@lexingtonflyingclub.org", "Lexington Flying Club");
            message.Subject = model.Subject;
            var view = AlternateView.CreateAlternateViewFromString(model.Body, null, MediaTypeNames.Text.Plain);
            message.AlternateViews.Add(view);

            message.To.Add("LFC_members@lexingtonflyingclub.org");

			foreach (var recipient in model.Recipients) {
			    var users = getEmailGroup(recipient);
			    if (users == null)
			    {
				    return View(model);
			    }
			    foreach (var user in users)
			    {
				    message.Bcc.Add(user.Email);
			    }
			}
            
            var smtp = new SmtpClient();
            try
            {
                smtp.Send(message);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Failed to send: " + e.ToString();
                return View("Index", model);
            }

            return RedirectToAction("Index", "MembersArea");
        }
    }
}