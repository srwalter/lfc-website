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
    public class EquipmentController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: Equipment
        public ActionResult Index()
        {
            var equipments = db.Equipments.Include(e => e.Airplane);
            return View(equipments.ToList());
        }

        // GET: Equipment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipments.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // GET: Equipment/Create
        public ActionResult Create(String id)
        {
            var equipment = new Equipment();
            equipment.AirplaneID = id;
            return View(equipment);
        }

        // POST: Equipment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EquipmentID,AirplaneID,Type,Description")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                db.Equipments.Add(equipment);
                db.SaveChanges();
                return RedirectToAction("Edit", "Airplanes", new { id = equipment.AirplaneID });
            }

            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "Type", equipment.AirplaneID);
            return RedirectToAction("Edit", "Airplanes", new { id = equipment.AirplaneID });
        }

        // GET: Equipment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipments.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", equipment.AirplaneID);
            return View(equipment);
        }

        // POST: Equipment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EquipmentID,AirplaneID,Type,Description")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Airplanes", new { id = equipment.AirplaneID });
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", equipment.AirplaneID);
            return View(equipment);
        }

        // GET: Equipment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipments.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // POST: Equipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipment equipment = db.Equipments.Find(id);
            var airplane = equipment.AirplaneID;
            db.Equipments.Remove(equipment);
            db.SaveChanges();
            return RedirectToAction("Edit", "Airplanes", new { id = airplane });
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
