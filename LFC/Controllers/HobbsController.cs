using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Text;

using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

using LFC.DAL;
using LFC.Models;
using LFC.ViewModels;

namespace LFC.Controllers
{
    [Authorize]
    public class HobbsController : Controller
    {
        private LFCContext db = new LFCContext();

        private void SendFlyingReport(IEnumerable<FlyingReport> flying)
        {
            var html = LFCContext.RenderViewToString(ControllerContext, "~/Views/Hobbs/_FlyingReports.cshtml", flying);

            var message = new MailMessage();
            message.From = new MailAddress("info@lexingtonflyingclub.org", "Lexington Flying Club");
            message.Subject = "LFC Flying Report";
            var view = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
            message.AlternateViews.Add(view);

            foreach (var user in db.Users.Where(x => x.Officer != null))
            {
                message.To.Add(user.Email);
            }
            message.To.Add("stevenrwalter@gmail.com");

            var smtp = new SmtpClient();
            smtp.Send(message);
        }

        private void SendBillingReport(IEnumerable<BillingReport> billing)
        {
            var html = LFCContext.RenderViewToString(ControllerContext, "~/Views/Hobbs/_BillingReports.cshtml", billing);

            var message = new MailMessage();
            message.From = new MailAddress("info@lexingtonflyingclub.org", "Lexington Flying Club");
            message.Subject = "LFC Billing Report";
            var view = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
            message.AlternateViews.Add(view);

            var plain = "plane\tm_id\tdate\thours\n";
            foreach (var entry in billing)
            {
                var plane = entry.Plane.Substring(1);
                plain = plain + String.Format("{0}\t{1}\t{2:MM/dd/yyyy}\t{3:F2}\n", plane, entry.BillingName, entry.Date, entry.Hours);
            }


            byte[] bytes = Encoding.ASCII.GetBytes(plain);
            var stream = new MemoryStream(bytes);
            var attach = new Attachment(stream, "billing.txt", "text/plain");
            message.Attachments.Add(attach);

            String pres = (from u in db.Users
                           where u.Officer == ApplicationUser.OfficerTitle.President
                           select u.Email).First();
            String treasurer = (from u in db.Users
                                where u.Officer == ApplicationUser.OfficerTitle.Treasurer
                                select u.Email).First();
            String asst = (from u in db.Users
                           where u.Officer == ApplicationUser.OfficerTitle.VPAdministration
                           select u.Email).First();
            message.To.Add(pres);
            message.To.Add(treasurer);
            message.To.Add(asst);
            message.To.Add("stevenrwalter@gmail.com");

            var smtp = new SmtpClient();
            smtp.Send(message);
        }

        [Authorize(Roles="Admin")]
        public ActionResult Reports(bool? send)
        {
            var flying = new List<FlyingReport>();
            foreach (var plane in db.Airplanes.Where(x => x.Active == true).ToList()) {
                var logs_for_plane = db.FlightLogs.Where(x => x.AirplaneID == plane.AirplaneID && x.Billed == null);
                var billed = 0.0;
                if (logs_for_plane.Count() > 0)
                {
                    billed = logs_for_plane.Sum(x => x.EndTach - x.StartTach);
                }

                var current_hobbs = plane.HobbsTimes.OrderByDescending(x => x.TachHours).First();
                var prev_hobbs = current_hobbs;
                try
                {
                    prev_hobbs = plane.HobbsTimes.Where(x => x.Billed.HasValue == true).OrderByDescending(x => x.TachHours).First();
                }
                catch
                {
                }
                var report = new FlyingReport();
                report.Plane = plane.AirplaneID;
                report.EndHobbs = current_hobbs.HobbsHours;
                report.EndTach = current_hobbs.TachHours;
                report.StartHobbs = prev_hobbs.HobbsHours;
                report.StartTach = prev_hobbs.TachHours;
                report.BilledTach = billed;
                flying.Add(report);
            }

            var billing = new List<BillingReport>();
            var flightlogs = db.FlightLogs.Include("Airplane").Include("Pilot").Where(x => x.Billed == null);
            foreach (var entry in flightlogs.OrderBy(x => x.Airplane.AirplaneID).ThenBy(x => x.Date))
            {
                var report = new BillingReport();
                if (entry.Pilot != null)
                    report.BillingName = entry.Pilot.ShortName;
                else
                    report.BillingName = entry.Airplane.AirplaneID.Substring(1);
                report.Date = entry.Date;
                report.Plane = entry.Airplane.AirplaneID;
                report.Hours = entry.EndTach - entry.StartTach;
                billing.Add(report);
            }

            var model = new ReportViewModel();
            model.Flying = flying;
            model.Billing = billing;

            if (send.GetValueOrDefault())
            {
                foreach (var entry in db.FlightLogs.Where(x => x.Billed == null))
                {
                    entry.Billed = DateTime.Now;
                    db.Entry(entry).State = EntityState.Modified;
                }

                foreach (var entry in db.HobbsTimes.Where(x => x.Billed == null)) {
                    entry.Billed = DateTime.Now;
                    db.Entry(entry).State = EntityState.Modified;
                }

                try
                {
                    SendFlyingReport(flying);
                    SendBillingReport(billing);
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Failed to send: " + e.ToString();
                    return View(model);
                }
                db.SaveChanges();
                ViewBag.Message = "Reports emailed successfully";
            }
            return View(model);
        }

        public ActionResult GetHobbs(String id)
        {
            var hobbs = db.HobbsTimes.Where(x => x.AirplaneID == id && x.Billed != null).Select(x => x.HobbsHours).Max();
            return Json(hobbs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTach(String id)
        {
            var tach = db.HobbsTimes.Where(x => x.AirplaneID == id && x.Billed != null).Select(x => x.TachHours).Max();
            return Json(tach, JsonRequestBehavior.AllowGet);
        }

        // GET: Hobbs
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(HobbsViewModel hobbs)
        {
            if (hobbs.TachEntries != null)
            {
                if (!ModelState.IsValid)
                {
                    hobbs.AllUsers = db.Users.Select(x => x.ShortName + " - " + x.FirstName + " " + x.LastName).ToList();
                    hobbs.AllUsers.Add("PLANE");
                    return View("Tach", hobbs);
                }

                var i = -1;
                foreach (var entry in hobbs.TachEntries) {
                    i++;
                    if (entry.PilotName == null && entry.EndTach == null)
                    {
                        continue;
                    }
                    var name_parts = entry.PilotName.Split(null);
                    var short_name = name_parts[0];
                    var pilot = db.Users.Where(x => x.ShortName == short_name);
                    if (pilot.Count() == 0 && entry.PilotName.ToUpper() != "PLANE")
                    {
                        ViewBag.Message = "Cannot find a pilot named '" + name_parts[0] + "'";
                        hobbs.AllUsers = db.Users.Select(x => x.ShortName + " - " + x.FirstName + " " + x.LastName).ToList();
                        hobbs.AllUsers.Add("PLANE");
                        return View("Tach", hobbs);
                    }
                    if (entry.StartTach > entry.EndTach.GetValueOrDefault())
                    {
                        var key = String.Format("TachEntries[{0}].EndTach", i);
                        ModelState.AddModelError(key, "End tach must be greater than start tach");
                        hobbs.AllUsers = db.Users.Select(x => x.ShortName + " - " + x.FirstName + " " + x.LastName).ToList();
                        hobbs.AllUsers.Add("PLANE");
                        return View("Tach", hobbs);
                    }
                    var flightlog = new FlightLog();
                    flightlog.Date = entry.Date;
                    flightlog.AirplaneID = hobbs.AirplaneID;
                    if (entry.PilotName == "PLANE")
                        flightlog.Pilot = null;
                    else
                        flightlog.Pilot = pilot.First();
                    flightlog.StartTach = entry.StartTach;
                    flightlog.EndTach = entry.EndTach.GetValueOrDefault();
                    db.FlightLogs.Add(flightlog);
                }

                var hobbsTime = new HobbsTime();
                hobbsTime.Date = hobbs.Date;
                hobbsTime.AirplaneID = hobbs.AirplaneID;
                hobbsTime.HobbsHours = hobbs.EndHobbs;
                hobbsTime.TachHours = hobbs.EndTach;
                db.HobbsTimes.Add(hobbsTime);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Failed to save changes: " + e.ToString();
                    hobbs.AllUsers = db.Users.Select(x => x.ShortName + " - " + x.FirstName + " " + x.LastName).ToList();
                    hobbs.AllUsers.Add("PLANE");
                    return View("Tach", hobbs);
                }
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID");
                return View(hobbs);
            }

            var entries = new List<TachEntry>(10);
            for (var i = 0; i < 10; i++)
            {
                entries.Add(new TachEntry());
            }
            hobbs.TachEntries = entries;
            hobbs.AllUsers = db.Users.Select(x => x.ShortName + " - " + x.FirstName + " " + x.LastName).ToList();
            hobbs.AllUsers.Add("PLANE");
            return View("Tach", hobbs);
        }
    }
}