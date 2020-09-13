using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net.Mime;

using LFC.ViewModels;
using LFC.DAL;

namespace LFC.Controllers
{
    [Authorize]
    public class EmailIndividualController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: EmailIndividual
        [Authorize(Roles="Admin")]
        public ActionResult Index(bool? retired=false) {
            var users = db.Users.OrderBy(x => x.LastName);
            if (retired == false)
            {
                ViewBag.AllUsers = users.Where(x => x.MemberType != Models.ApplicationUser.MembershipType.Retired).ToList();
            } else
            {
                ViewBag.AllUsers = users.ToList();
            }
            ViewBag.Retired = retired;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Index(EmailIndividualViewModel model, List<String> recipients)
        {
            ViewBag.AllUsers = db.Users.OrderBy(x => x.LastName).ToList();
            if (!ModelState.IsValid)
                return View();

            if (recipients == null)
            {
                ViewBag.Message = "null";
                return View();
            }

            var message = new MailMessage();
            message.From = new MailAddress("info@lexingtonflyingclub.org", "Lexington Flying Club");
            message.Subject = model.Subject;
            var view = AlternateView.CreateAlternateViewFromString(model.Body, null, MediaTypeNames.Text.Plain);
            message.AlternateViews.Add(view);

            message.To.Add("LFC_members@lexingtonflyingclub.org");

            foreach (var recipient in recipients)
            {
                    message.Bcc.Add(recipient);
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