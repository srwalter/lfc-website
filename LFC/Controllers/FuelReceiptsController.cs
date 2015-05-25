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
    public class FuelReceiptsController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: FuelReceipts
        public ActionResult Index()
        {
            var fuelReceipts = db.FuelReceipts.Include(f => f.Airplane).Include(f => f.Pilot);
            return View(fuelReceipts.ToList());
        }

        // GET: FuelReceipts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelReceipt fuelReceipt = db.FuelReceipts.Find(id);
            if (fuelReceipt == null)
            {
                return HttpNotFound();
            }
            return View(fuelReceipt);
        }

        // GET: FuelReceipts/Create
        public ActionResult Create()
        {
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "Type");
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName");
            return View();
        }

        // POST: FuelReceipts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FuelReceiptID,ApplicationUserID,AirplaneID,Date,Gallons,Dollars")] FuelReceipt fuelReceipt)
        {
            if (ModelState.IsValid)
            {
                db.FuelReceipts.Add(fuelReceipt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "Type", fuelReceipt.AirplaneID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", fuelReceipt.ApplicationUserID);
            return View(fuelReceipt);
        }

        // GET: FuelReceipts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelReceipt fuelReceipt = db.FuelReceipts.Find(id);
            if (fuelReceipt == null)
            {
                return HttpNotFound();
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "Type", fuelReceipt.AirplaneID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", fuelReceipt.ApplicationUserID);
            return View(fuelReceipt);
        }

        // POST: FuelReceipts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FuelReceiptID,ApplicationUserID,AirplaneID,Date,Gallons,Dollars")] FuelReceipt fuelReceipt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fuelReceipt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "Type", fuelReceipt.AirplaneID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", fuelReceipt.ApplicationUserID);
            return View(fuelReceipt);
        }

        // GET: FuelReceipts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuelReceipt fuelReceipt = db.FuelReceipts.Find(id);
            if (fuelReceipt == null)
            {
                return HttpNotFound();
            }
            return View(fuelReceipt);
        }

        // POST: FuelReceipts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FuelReceipt fuelReceipt = db.FuelReceipts.Find(id);
            db.FuelReceipts.Remove(fuelReceipt);
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
