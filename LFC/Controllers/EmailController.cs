﻿using System;
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
using System.Data.Entity;

namespace LFC.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        private LFCContext db = new LFCContext();

        private void SendBillingReminders()
        {
            if (db.BillingEmails.Count() == 0)
                return;

            var html = db.BillingEmails.First().Body;

            var message = new MailMessage();
            message.From = new MailAddress("info@lexingtonflyingclub.org", "Lexington Flying Club");
            message.Subject = "LFC Monthly Billing Reminder";
            var view = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
            message.AlternateViews.Add(view);

            var db2 = new LFCContext();
            var users = db2.Users.Where(x => x.MemberType != ApplicationUser.MembershipType.Retired);
            message.To.Add("info@lexingtonflyingclub.org");
            foreach (var user in users)
            {
                if (user.Email != null)
                    message.Bcc.Add(user.Email);
            }

            var smtp = new SmtpClient();
            smtp.Send(message);
        }

        private void SendPredictiveMaint()
        {
            var planes = db.Airplanes.Where(x => x.Active == true).ToList();
            foreach (var plane in planes)
            {
                var smtp = new SmtpClient();
                var lookahead = DateTime.Now.AddDays(30);
                bool hundred_hour = false;
                bool oil_change = false;

                if (Math.Abs((plane.EstimatedMaintenanceDate(plane.HundredHour) - lookahead).TotalHours) <= 24)
                {
                    hundred_hour = true;
                }
                if (Math.Abs((plane.EstimatedMaintenanceDate(plane.OilChange) - lookahead).TotalHours) <= 24)
                {
                    oil_change = true;
                }
                
                if (oil_change || hundred_hour)
                {
                    var body = "This is an automated message.  Based on current tach hours and average flying amounts, aircraft " + plane.AirplaneID + " is estimated to need maintenance on or about " + lookahead.GetDateTimeFormats('D')[1] + ".\n\n";
                    body += "Maintenance expected: \n\n";
                    if (oil_change)
                        body += "Oil change\n\n";
                    if (hundred_hour)
                        body += "100 hour inspection\n\n";
                    body += "\n";
                    
                    var message = new MailMessage();
                    message.From = new MailAddress("info@lexingtonflyingclub.org", "Lexington Flying Club");
                    message.Subject = "LFC Upcoming Aircraft Maintenance for " + plane.AirplaneID;

                    var view = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Plain);
                    message.AlternateViews.Add(view);

                    String pres = (from u in db.Users
                                   where u.Officer == ApplicationUser.OfficerTitle.President
                                   select u.Email).First();
                    var db2 = new LFCContext();
                    String maint = (from a in db2.Airplanes
                                    where a.AirplaneID == plane.AirplaneID
                                    select a.MaintenanceOfficer.Email).First();
                    message.CC.Add(pres);
                    message.CC.Add(maint);
                    message.CC.Add("stevenrwalter@gmail.com");
                    smtp.Send(message);
                }
            }
        }

        private void SendBadgeReminders()
        {
            var body = "Your Airport Operation Area badge for Bluegrass Airport is scheduled to expire in the next 30/60 days.  Please ensure you renew it before it expires to get the $15 renewal rate.  If a badge is not renewed within 30 days post expiration, a new badge ($50) and background check will be required. Additionally, you will be unable to access the ramp for a week or more. Once your badge is renewed, please Reply-All to this email with the updated expiration date and new badge number.\n\nSend Mike Proctor an email requesting that he email Security letting them know that you are coming apply for your badge renewal.  Then make an appointment with Security.  They have the necessary forms.  Be sure to take your drivers license and your passport; they require two government photo id's.  Renewal cost is $15.00.  If you are not planning to renew the badge, you must return the badge to the airport operations department as soon as practical.  If a badge expires and is not turned in within 90 days of the expiration date, the airport will fine the flying club $150 and you will lose your $150 deposit that the flying club holds…\n\nIf you have lost your badge, you MUST call Airport Operations and let them know that you have lost your badge.  If, after 30 days post expiration, you have not requested a replacement badge,  the Airport will fine the LFC $150 and you will lose your $150 deposit on file with the Club.\n\nPlease see these links for more info:\n http://www.lexingtonflyingclub.org/Content/badge-1.jpg \n http://www.lexingtonflyingclub.org/Content/badge-2.jpg";
            var smtp = new SmtpClient();

            var users = db.Users.Where(x => DbFunctions.DiffDays(DateTime.Now, x.BadgeExpires) == 30 || DbFunctions.DiffDays(DateTime.Now, x.BadgeExpires) == 60)
                .Where(x => x.MemberType != ApplicationUser.MembershipType.Retired).ToList();
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
                message.CC.Add("stevenrwalter@gmail.com");
                message.To.Add(x.Email);

                ViewBag.Message += x.Email;
                smtp.Send(message);
            }
        }

        private void CheckAirplaneMaintenance(Airplane plane, ActiveAlert.AlertType type, DateTime due, int days)
        {
            var deadline = DateTime.Now.AddDays(days);
            if (due.Date == deadline.Date)
            {
                var alert = new ActiveAlert
                {
                    Airplane = plane,
                    Type = type
                };
                db.ActiveAlerts.Add(alert);
            }
            else if (due > deadline)
            {
                var alert = db.ActiveAlerts.Find(type, plane.AirplaneID);
                if (alert != null)
                {
                    db.ActiveAlerts.Remove(alert);
                }
            }
        }

        private void QueueNewAlerts()
        {
            foreach (var plane in db.Airplanes.Where(x => x.Active == true).ToList())
            {
                CheckAirplaneMaintenance(plane, ActiveAlert.AlertType.Transponder, plane.TransponderDue, 30);
                CheckAirplaneMaintenance(plane, ActiveAlert.AlertType.Static, plane.StaticDue, 30);
                CheckAirplaneMaintenance(plane, ActiveAlert.AlertType.Annual, plane.AnnualDue, 30);
                CheckAirplaneMaintenance(plane, ActiveAlert.AlertType.ELTBattery, plane.EltBatteryDue, 30);
                CheckAirplaneMaintenance(plane, ActiveAlert.AlertType.ELT, plane.EltDue, 30);
                CheckAirplaneMaintenance(plane, ActiveAlert.AlertType.Registration, plane.RegistrationExpires, 60);
            }
            db.SaveChanges();
        }

        public void SendActiveAlerts()
        {
            var alertsByPlane = db.ActiveAlerts.GroupBy(x => x.Airplane);
            foreach (var alerts in alertsByPlane)
            {
                AlertEmail model = new AlertEmail();
                model.AirplaneID = alerts.Key.AirplaneID;
                model.Alerts = alerts.Select(x => x.Type).ToList();

                var html = LFCContext.RenderViewToString(ControllerContext, "~/Views/Email/_SendActiveAlerts.cshtml", model);

                var message = new MailMessage();
                message.From = new MailAddress("info@lexingtonflyingclub.org", "Lexington Flying Club");
                message.Subject = "Aircraft Maintenance Alert for " + alerts.Key.AirplaneID;
                var view = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
                message.AlternateViews.Add(view);

                var db2 = new LFCContext();
                String pres = (from u in db2.Users
                               where u.Officer == ApplicationUser.OfficerTitle.President
                               select u.Email).First();
                //String safety = (from u in db2.Users
                //               where u.Officer == ApplicationUser.OfficerTitle.SafetyOfficer
                //               select u.Email).First();
                String maint = (from a in db2.Airplanes
                                where a.AirplaneID == model.AirplaneID
                                select a.MaintenanceOfficer.Email).First();
                message.To.Add(maint);
                message.CC.Add(pres);
                //message.CC.Add(safety);
                message.To.Add("stevenrwalter@gmail.com");

                var smtp = new SmtpClient();
                smtp.Send(message);
            }
        }

        [AllowAnonymous]
        public ActionResult SendTimedEmails()
        {
            try
            {
                SendBadgeReminders();
                if (DateTime.Now.Day == 25)
                {
                    SendBillingReminders();
                }
            } catch (Exception e)
            {
                ViewBag.Message += "Failed to send: " + e.ToString();
            }

            try {
                SendPredictiveMaint();
                QueueNewAlerts();
                SendActiveAlerts();

            }
            catch (Exception e)
            {
                ViewBag.Message += "Failed to send: " + e.ToString();
            }
            return View();
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
                    users = users.Where(x => x.MemberType != ApplicationUser.MembershipType.Retired);
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

                case Recipients.BadgeHolders:
                    users = users.Where(n => n.BadgeExpires > new DateTime(2000, 1, 1));
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
            // XXX: add support for attachments?

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
                    if (user.Email != null)
                    {
                        message.Bcc.Add(user.Email);
                    }
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