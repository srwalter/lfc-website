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

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LFC.Controllers
{
    public class CopilotsController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: Copilots
        public ActionResult Index()
        {
            var copilots = db.Copilots.Include(c => c.Pilot);
            return View(copilots.ToList());
        }

        // GET: Copilots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Copilot copilot = db.Copilots.Find(id);
            if (copilot == null)
            {
                return HttpNotFound();
            }
            return View(copilot);
        }

        // GET: Copilots/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Copilots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CopilotID,Date,Duration")] Copilot copilot)
        {
            if (ModelState.IsValid)
            {
                copilot.ApplicationUserID = User.Identity.GetUserId();
                db.Copilots.Add(copilot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(copilot);
        }

        // GET: Copilots/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Copilot copilot = db.Copilots.Find(id);
            if (copilot == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", copilot.ApplicationUserID);
            return View(copilot);
        }

        // POST: Copilots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CopilotID,ApplicationUserID,Date,Duration")] Copilot copilot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(copilot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", copilot.ApplicationUserID);
            return View(copilot);
        }

        // GET: Copilots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Copilot copilot = db.Copilots.Find(id);
            if (copilot == null)
            {
                return HttpNotFound();
            }
            return View(copilot);
        }

        // POST: Copilots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Copilot copilot = db.Copilots.Find(id);
            if (!User.IsInRole("Admin") && copilot.Pilot.Id != User.Identity.GetUserId())
            {
                ViewBag.Message = "You may only delete your own copilot requests";
                return View(copilot);
            }
            db.Copilots.Remove(copilot);
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
