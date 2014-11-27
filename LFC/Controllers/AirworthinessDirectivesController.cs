﻿using System;
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
    public class AirworthinessDirectivesController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: AirworthinessDirectives
        public ActionResult Index()
        {
            var airworthinessDirectives = db.AirworthinessDirectives.Include(a => a.Airplane);
            return View(airworthinessDirectives.ToList());
        }

        // GET: AirworthinessDirectives/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirworthinessDirective airworthinessDirective = db.AirworthinessDirectives.Find(id);
            if (airworthinessDirective == null)
            {
                return HttpNotFound();
            }
            return View(airworthinessDirective);
        }

        // GET: AirworthinessDirectives/Create
        public ActionResult Create()
        {
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID");
            return View();
        }

        // POST: AirworthinessDirectives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AirworthinessDirectiveID,AirplaneID,Description,FrequencyHours,FrequencyMonths,FrequencyMisc,LastDoneHours,LastDoneDate")] AirworthinessDirective airworthinessDirective)
        {
            if (ModelState.IsValid)
            {
                db.AirworthinessDirectives.Add(airworthinessDirective);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", airworthinessDirective.AirplaneID);
            return View(airworthinessDirective);
        }

        // GET: AirworthinessDirectives/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirworthinessDirective airworthinessDirective = db.AirworthinessDirectives.Find(id);
            if (airworthinessDirective == null)
            {
                return HttpNotFound();
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", airworthinessDirective.AirplaneID);
            return View(airworthinessDirective);
        }

        // POST: AirworthinessDirectives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AirworthinessDirectiveID,AirplaneID,Description,FrequencyHours,FrequencyMonths,FrequencyMisc,LastDoneHours,LastDoneDate")] AirworthinessDirective airworthinessDirective)
        {
            if (ModelState.IsValid)
            {
                db.Entry(airworthinessDirective).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", airworthinessDirective.AirplaneID);
            return View(airworthinessDirective);
        }

        // GET: AirworthinessDirectives/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirworthinessDirective airworthinessDirective = db.AirworthinessDirectives.Find(id);
            if (airworthinessDirective == null)
            {
                return HttpNotFound();
            }
            return View(airworthinessDirective);
        }

        // POST: AirworthinessDirectives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AirworthinessDirective airworthinessDirective = db.AirworthinessDirectives.Find(id);
            db.AirworthinessDirectives.Remove(airworthinessDirective);
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
