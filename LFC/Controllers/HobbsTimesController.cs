using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LFC.DAL;
using LFC.Models;

namespace LFC.Controllers
{
    [Authorize]
    public class HobbsTimesController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: HobbsTimes
        public ActionResult Index(String plane="")
        {
            IQueryable<HobbsTime> hobbsTimes = db.HobbsTimes.Include(h => h.Airplane).OrderBy(x => x.Date);
            if (plane != "")
                hobbsTimes = hobbsTimes.Where(x => x.AirplaneID == plane);
            return View(hobbsTimes.ToList());
        }

        // GET: HobbsTimes/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HobbsTime hobbsTime = db.HobbsTimes.Find(id);
            if (hobbsTime == null)
            {
                return HttpNotFound();
            }
            return View(hobbsTime);
        }

        // GET: HobbsTimes/Create
        [Authorize(Roles="Admin")]
        public ActionResult Create(String AirplaneID)
        {
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", AirplaneID);
            return View();
        }

        private void CheckHourlyMaintenance(HobbsTime times, double due, ActiveAlert.AlertType type)
        {
            var plane = db.Airplanes.Find(times.AirplaneID);
            var old_tach = plane.getCurrentTach();
            var new_tach = times.TachHours;

            var old_delta = due - old_tach;
            var new_delta = due - new_tach;

            if (old_delta > 10.0 && new_delta < 10.0)
            {
                var alert = new ActiveAlert
                {
                    Airplane = plane,
                    Type = type
                };
                db.ActiveAlerts.Add(alert);
            }
        }

        // POST: HobbsTimes/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Date,AirplaneID,HobbsHours,TachHours")] HobbsTime hobbsTime)
        {
            if (ModelState.IsValid)
            {
                var plane = db.Airplanes.Find(hobbsTime.AirplaneID);
                CheckHourlyMaintenance(hobbsTime, plane.OilChange, ActiveAlert.AlertType.OilChange);
                CheckHourlyMaintenance(hobbsTime, plane.HundredHour, ActiveAlert.AlertType.HundredHour);
                foreach (var ad in plane.ADs)
                {
                    if (ad.FrequencyHours == null)
                        continue;
                    CheckHourlyMaintenance(hobbsTime, (ad.LastDoneHours + ad.FrequencyHours).GetValueOrDefault(), ActiveAlert.AlertType.AD);
                }
                db.HobbsTimes.Add(hobbsTime);
                db.SaveChanges();
                return RedirectToAction("Details", "Airplanes", new { id = hobbsTime.AirplaneID });
            }

            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", hobbsTime.AirplaneID);
            return View(hobbsTime);
        }

        // GET: HobbsTimes/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(DateTime date, String airplane)
        {
            if (date == null || airplane == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hobbsTime = db.HobbsTimes.Where(x => (x.AirplaneID == airplane && x.Date == date));
            if (hobbsTime.Count() == 0)
            {
                return HttpNotFound();
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "Type", hobbsTime.First().AirplaneID);
            return View(hobbsTime.First());
        }

        // POST: HobbsTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Date,AirplaneID,HobbsHours,TachHours,Billed")] HobbsTime hobbsTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hobbsTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "Type", hobbsTime.AirplaneID);
            return View(hobbsTime);
        }

        // GET: HobbsTimes/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(DateTime date, String airplane)
        {
            if (date == null || airplane == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hobbsTime = db.HobbsTimes.Where(x => x.Date == date && x.AirplaneID == airplane);
            if (hobbsTime.Count() == 0)
            {
                return HttpNotFound();
            }
            return View(hobbsTime.First());
        }

        // POST: HobbsTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DateTime date, String airplane)
        {
            var hobbsTime = db.HobbsTimes.Where(x => x.Date == date && x.AirplaneID == airplane);
            db.HobbsTimes.Remove(hobbsTime.First());
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
