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
            return View(db.Airplanes.ToList());
        }

        // GET: Airplanes/Details/5
        public ActionResult Details(string id)
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
        public ActionResult Create(Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                db.Airplanes.Add(airplane);
                db.SaveChanges();
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
            return View(airplane);
        }

        // POST: Airplanes/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                db.Entry(airplane).State = EntityState.Modified;
                db.SaveChanges();
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
