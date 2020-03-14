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
    public class AirplanesController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: Airplanes
        public ActionResult Index()
        {
            IEnumerable<Airplane> planes;
            if (User.IsInRole("Admin"))
            {
                planes = db.Airplanes.ToList();
            }
            else
            {
                planes = db.Airplanes.Where(x => x.Active == true).ToList();
            }
            return View(planes);
        }

        public ActionResult HoursPerMonth()
        {
            var planes = db.Airplanes.Where(x => x.Active == true).ToList();
            return View(planes);
        }


        // GET: Airplanes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airplane airplane = db.Airplanes.Include("MaintenanceOfficer").First(x => x.AirplaneID == id);
            if (airplane == null)
            {
                return HttpNotFound();
            }
            return View(airplane);
        }

        // GET: Airplanes/Create
        [Authorize(Roles="Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Airplanes/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude="Updated,UpdatedBy")] Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.First(u => u.UserName == User.Identity.Name);
                airplane.Active = true;
                airplane.AnnualDue = DateTime.Now;
                airplane.EltDue = DateTime.Now;
                airplane.EltBatteryDue = DateTime.Now;
                airplane.TransponderDue = DateTime.Now;
                airplane.StaticDue = DateTime.Now;
                airplane.GPSExpires = DateTime.Now;
                airplane.RegistrationExpires = DateTime.Now;
                airplane.UpdatedNow(user);
                db.Airplanes.Add(airplane);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Failed to send: " + e.ToString();
                    return View();
                }
                return RedirectToAction("Index");
            }

            return View(airplane);
        }

        // GET: Airplanes/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airplane airplane = db.Airplanes.Find(id);
            if (airplane == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaintenanceOfficerID = new SelectList(db.Users.OrderBy(x => x.LastName), "Id", "FullName", airplane.MaintenanceOfficerID);
            return View(airplane);
        }

        private void CheckHourlyMaintenance(Airplane plane, double prev_time, double new_time, ActiveAlert.AlertType type)
        {
            var tach = plane.getCurrentTach();

            var old_delta = prev_time - tach;
            var new_delta = new_time - tach;
            if (old_delta < 10.0 && new_delta > 10.0)
            {
                var alert = db.ActiveAlerts.Find(type, plane.AirplaneID);
                if (alert != null)
                {
                    db.ActiveAlerts.Remove(alert);
                }
            }
        }

        // POST: Airplanes/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude="Updated,UpdatedBy")] Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                var old = db.Airplanes.Find(airplane.AirplaneID);
                CheckHourlyMaintenance(old, old.HundredHour, airplane.HundredHour, ActiveAlert.AlertType.HundredHour);
                CheckHourlyMaintenance(old, old.OilChange, airplane.OilChange, ActiveAlert.AlertType.OilChange);
                db.Entry(old).State = EntityState.Detached;

                var user = db.Users.First(u => u.UserName == User.Identity.Name);
                airplane.UpdatedNow(user);
                db.Entry(airplane).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Failed to send: " + e.ToString();
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View(airplane);
        }

        // GET: Airplanes/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airplane airplane = db.Airplanes.Find(id);
            if (airplane == null)
            {
                return HttpNotFound();
            }
            return View(airplane);
        }

        // POST: Airplanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Airplane airplane = db.Airplanes.Find(id);
            db.Airplanes.Remove(airplane);
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
