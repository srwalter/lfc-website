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
    public class ActiveAlertsController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: ActiveAlerts
        public ActionResult Index(String plane)
        {
            var activeAlerts = db.ActiveAlerts.Include(a => a.Airplane);
            if (plane != null)
                activeAlerts = activeAlerts.Where(x => x.AirplaneID == plane);
            return View(activeAlerts.ToList());
        }
        
        // GET: ActiveAlerts/Create
        public ActionResult Create()
        {
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID");
            return View();
        }

        // POST: ActiveAlerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Type,AirplaneID")] ActiveAlert activeAlert)
        {
            if (ModelState.IsValid)
            {
                db.ActiveAlerts.Add(activeAlert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", activeAlert.AirplaneID);
            return View(activeAlert);
        }

        // GET: ActiveAlerts/Delete/5
        public ActionResult Delete(ActiveAlert.AlertType type, String airplane)
        {
            ActiveAlert activeAlert = db.ActiveAlerts.Find(type, airplane);
            if (activeAlert == null)
            {
                return HttpNotFound();
            }
            return View(activeAlert);
        }

        // POST: ActiveAlerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ActiveAlert.AlertType type, String airplane)
        {
            ActiveAlert activeAlert = db.ActiveAlerts.Find(type, airplane);
            db.ActiveAlerts.Remove(activeAlert);
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
