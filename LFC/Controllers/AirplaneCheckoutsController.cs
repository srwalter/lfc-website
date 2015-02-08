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
    [Authorize(Roles="Admin")]
    public class AirplaneCheckoutsController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: AirplaneCheckouts
        public ActionResult Index()
        {
            var airplaneCheckouts = db.AirplaneCheckouts.Include(a => a.Airplane).Include(a => a.Pilot);
            return View(airplaneCheckouts.ToList());
        }

        // GET: AirplaneCheckouts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirplaneCheckout airplaneCheckout = db.AirplaneCheckouts.Find(id);
            if (airplaneCheckout == null)
            {
                return HttpNotFound();
            }
            return View(airplaneCheckout);
        }

        // GET: AirplaneCheckouts/Create
        public ActionResult Create()
        {
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID");
            ViewBag.PilotID = new SelectList(db.Users, "Id", "Fullname");
            return View();
        }

        // POST: AirplaneCheckouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PilotID,AirplaneID")] AirplaneCheckout airplaneCheckout)
        {
            if (ModelState.IsValid)
            {
                db.AirplaneCheckouts.Add(airplaneCheckout);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", airplaneCheckout.AirplaneID);
            ViewBag.PilotID = new SelectList(db.Users, "Id", "FullName", airplaneCheckout.PilotID);
            return View(airplaneCheckout);
        }

        // GET: AirplaneCheckouts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirplaneCheckout airplaneCheckout = db.AirplaneCheckouts.Find(id);
            if (airplaneCheckout == null)
            {
                return HttpNotFound();
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "Type", airplaneCheckout.AirplaneID);
            ViewBag.PilotID = new SelectList(db.Users, "Id", "ShortName", airplaneCheckout.PilotID);
            return View(airplaneCheckout);
        }

        // POST: AirplaneCheckouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PilotID,AirplaneID")] AirplaneCheckout airplaneCheckout)
        {
            if (ModelState.IsValid)
            {
                db.Entry(airplaneCheckout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "Type", airplaneCheckout.AirplaneID);
            ViewBag.PilotID = new SelectList(db.Users, "Id", "ShortName", airplaneCheckout.PilotID);
            return View(airplaneCheckout);
        }

        // GET: AirplaneCheckouts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirplaneCheckout airplaneCheckout = db.AirplaneCheckouts.Find(id);
            if (airplaneCheckout == null)
            {
                return HttpNotFound();
            }
            return View(airplaneCheckout);
        }

        // POST: AirplaneCheckouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AirplaneCheckout airplaneCheckout = db.AirplaneCheckouts.Find(id);
            db.AirplaneCheckouts.Remove(airplaneCheckout);
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
