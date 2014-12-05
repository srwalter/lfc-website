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
        public ActionResult Index()
        {
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID");
            return View();
        }

        [HttpPost]
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