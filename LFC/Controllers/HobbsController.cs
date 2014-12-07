using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LFC.DAL;
using LFC.Models;
using LFC.ViewModels;

namespace LFC.Controllers
{
    [Authorize]
    public class HobbsController : Controller
    {
        private LFCContext db = new LFCContext();

        [Authorize(Roles="Admin")]
        public ActionResult Reports()
        {
            var flying = new List<FlyingReport>();
            foreach (var plane in db.Airplanes.Where(x => x.Active == true).ToList()) {
                var logs_for_plane = db.FlightLogs.Where(x => x.AirplaneID == plane.AirplaneID && x.Billed == false);
                var billed = 0.0;
                if (logs_for_plane.Count() > 0)
                {
                    billed = logs_for_plane.Sum(x => x.EndTach - x.StartTach);
                }

                var hobbs = db.HobbsTimes.Where(x => x.AirplaneID == plane.AirplaneID).OrderByDescending(x => x.Date).ToList();
                var report = new FlyingReport();
                report.Plane = plane.AirplaneID;
                report.EndHobbs = hobbs[0].HobbsHours;
                report.EndTach = hobbs[0].TachHours;
                report.StartHobbs = hobbs[1].HobbsHours;
                report.StartTach = hobbs[1].TachHours;
                report.BilledTach = billed;
                flying.Add(report);
            }

            var billing = new List<BillingReport>();
            foreach (var entry in db.FlightLogs.Include("Airplane").Include("Pilot").Where(x => x.Billed == false))
            {
                var report = new BillingReport();
                report.BillingName = entry.Pilot.ShortName;
                report.Date = entry.Date;
                report.Plane = entry.Airplane.AirplaneID;
                report.Hours = entry.EndTach - entry.StartTach;
                billing.Add(report);
            }

            var model = new ReportViewModel();
            model.Flying = flying;
            model.Billing = billing;
            return View(model);
        }

        public ActionResult GetHobbs(String id)
        {
            var hobbs = db.HobbsTimes.Where(x => x.AirplaneID == id).Select(x => x.HobbsHours).Max();
            return Json(hobbs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTach(String id)
        {
            var tach = db.HobbsTimes.Where(x => x.AirplaneID == id).Select(x => x.TachHours).Max();
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
                foreach (var entry in hobbs.TachEntries) {
                    if (entry.PilotName == null && entry.EndTach == 0.0)
                    {
                        continue;
                    }
                    var pilot = db.Users.Where(x => x.FirstName + " " + x.LastName == entry.PilotName);
                    if (pilot.Count() == 0)
                    {
                        ViewBag.Message = "There is no pilot named '" + entry.PilotName + "'";
                        hobbs.AllUsers = db.Users.AsEnumerable();
                        return View("Tach", hobbs);
                    }
                    var flightlog = new FlightLog();
                    flightlog.Date = hobbs.Date;
                    flightlog.AirplaneID = hobbs.AirplaneID;
                    flightlog.Pilot = pilot.First();
                    flightlog.StartTach = entry.StartTach;
                    flightlog.EndTach = entry.EndTach;
                    db.FlightLogs.Add(flightlog);
                }

                var hobbsTime = new HobbsTime();
                hobbsTime.Date = hobbs.Date;
                hobbsTime.AirplaneID = hobbs.AirplaneID;
                hobbsTime.HobbsHours = hobbs.EndHobbs;
                hobbsTime.TachHours = hobbs.EndTach;
                db.HobbsTimes.Add(hobbsTime);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var entries = new List<TachEntry>(10);
            for (var i = 0; i < 10; i++)
            {
                entries.Add(new TachEntry());
            }
            hobbs.TachEntries = entries;
            hobbs.AllUsers = db.Users.AsEnumerable();
            return View("Tach", hobbs);
        }
    }
}