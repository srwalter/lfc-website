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
    public class BillingEmailsController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: BillingEmails/Edit/5
        public ActionResult Edit()
        {
            BillingEmail billingEmail = new BillingEmail();
            if (db.BillingEmails.Count() > 0)
            {
                billingEmail = db.BillingEmails.First();
            }
            return View(billingEmail);
        }

        // POST: BillingEmails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Body")] BillingEmail billingEmail)
        {
            if (ModelState.IsValid)
            {
                db.BillingEmails.ToList().ForEach(p => db.BillingEmails.Remove(p));
                db.BillingEmails.Add(billingEmail);
                db.SaveChanges();
                ViewBag.StatusMessage = "Message updated.";
                return View(billingEmail);
            }

            return View(billingEmail);
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
