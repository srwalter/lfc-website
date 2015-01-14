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
    public class FlightLogsController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: FlightLogs
        [Authorize(Roles="Admin")]
        public ActionResult Index(String AirplaneID)
        {
            var flightLogs = db.FlightLogs.Include(f => f.Airplane).Include(f => f.Pilot);
            if (AirplaneID != null && AirplaneID != "All Planes")
                flightLogs = flightLogs.Where(x => x.AirplaneID == AirplaneID);
            flightLogs = flightLogs.OrderBy(x => x.AirplaneID)
                .ThenBy(x => x.Date)
                .ThenBy(x => x.EndTach);
            var planes = db.Airplanes.Select(x => x.AirplaneID).ToList();
            planes.Insert(0, "All Planes");
            ViewBag.AirplaneID = new SelectList(planes);
            return View(flightLogs.ToList());
        }

        // GET: FlightLogs/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightLog flightLog = db.FlightLogs.Find(id);
            if (flightLog == null)
            {
                return HttpNotFound();
            }
            return View(flightLog);
        }

        // GET: FlightLogs/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID");
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName");
            return View();
        }

        // POST: FlightLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "FlightLogID,AirplaneID,ApplicationUserID,Date,StartTach,EndTach")] FlightLog flightLog)
        {
            if (ModelState.IsValid)
            {
                db.FlightLogs.Add(flightLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", flightLog.AirplaneID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", flightLog.ApplicationUserID);
            return View(flightLog);
        }

        // GET: FlightLogs/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightLog flightLog = db.FlightLogs.Find(id);
            if (flightLog == null)
            {
                return HttpNotFound();
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", flightLog.AirplaneID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", flightLog.ApplicationUserID);
            return View(flightLog);
        }

        // POST: FlightLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "FlightLogID,AirplaneID,ApplicationUserID,Date,StartTach,EndTach")] FlightLog flightLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flightLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", flightLog.AirplaneID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", flightLog.ApplicationUserID);
            return View(flightLog);
        }

        // GET: FlightLogs/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightLog flightLog = db.FlightLogs.Find(id);
            if (flightLog == null)
            {
                return HttpNotFound();
            }
            return View(flightLog);
        }

        // POST: FlightLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlightLog flightLog = db.FlightLogs.Find(id);
            db.FlightLogs.Remove(flightLog);
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
