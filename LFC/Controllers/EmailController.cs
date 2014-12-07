using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

using LFC.ViewModels;

namespace LFC.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        // GET: Email

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(EmailViewModel model)
        {
            var message = new MailMessage();
            message.From = new MailAddress("info@lexingtonflyingclub.org", "Lexington Flying Club");
            message.Subject = model.Subject;
            var view = AlternateView.CreateAlternateViewFromString(model.Body, null, MediaTypeNames.Text.Plain);
            message.AlternateViews.Add(view);

            // XXX
            message.To.Add("stevenrwalter@gmail.com");

            var smtp = new SmtpClient();
            try
            {
                smtp.Send(message);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Failed to send: " + e.ToString();
                return View(model);
            }

            return RedirectToAction("Index", "MembersArea");
        }
    }
}