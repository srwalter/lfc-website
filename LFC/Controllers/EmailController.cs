﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

using LFC.Models;
using LFC.ViewModels;
using LFC.DAL;

namespace LFC.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        private LFCContext db = new LFCContext();

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

            message.To.Add("LFC_members@lexingtonflyingclub.org");
            IQueryable<ApplicationUser> users = db.Users;

            switch (model.Recipients)
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
                    ViewBag.Message = "Error: unsupported recipient type: " + model.Recipients;
                    return View(model);
            }

            foreach (var user in users)
            {
                message.Bcc.Add(user.Email);
            }
            
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