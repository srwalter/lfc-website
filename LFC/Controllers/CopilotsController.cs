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

using System.Net.Mail;
using System.Net.Mime;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LFC.Controllers
{
    public class CopilotsController : Controller
    {
        private LFCContext db = new LFCContext();

        private void SendEmailsFor(Copilot copilot)
        {
            string format = @"An LFC member is looking for a copilot for an upcoming flight:

    Pilot: {0}
    Plane: {1}
    Date: {2}
    Duration: {3} hours;

If you're interested in being the copilot, reply to this email to contact the pilot.

{4}

You're recieving this message because you are listed as being willing to serve as a safety pilot in the LFC database.
";
            var link = "http://www.lexingtonflyingclub.org/Copilots/Details/" + copilot.CopilotID.ToString();
            var body = String.Format(format, copilot.Pilot.FullName, copilot.AirplaneID, copilot.Date, copilot.Duration, link);
            var smtp = new SmtpClient();

            var users = db.Users.Where(x => x.Safety == true);
            if (copilot.InstrumentRequired)
            {
                users = users.Where(x => x.Instrument == true);
            }

            foreach (var x in users)
            {
                var message = new MailMessage();
                message.From = new MailAddress("info@lexingtonflyingclub.org", "Lexington Flying Club");
                message.ReplyToList.Add(new MailAddress(copilot.Pilot.Email, copilot.Pilot.FullName));
                message.Subject = "LFC Copilot Request";

                var view = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Plain);
                message.AlternateViews.Add(view);

                message.CC.Add("stevenrwalter@gmail.com");
                message.To.Add(x.Email);
                
                smtp.Send(message);
            }
        }

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
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID");
            return View();
        }

        // POST: Copilots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CopilotID,Date,Duration,AirplaneID")] Copilot copilot)
        {
            if (copilot.Date.Hour == 0 && copilot.Date.Minute == 0)
            {
                ViewBag.Message = "Specify a time and a date";
            }
            else if (ModelState.IsValid)
            {
                copilot.ApplicationUserID = User.Identity.GetUserId();
                db.Copilots.Add(copilot);
                db.SaveChanges();

                copilot = db.Copilots.Include("Pilot").First(x => x.CopilotID == copilot.CopilotID);
                SendEmailsFor(copilot);

                return RedirectToAction("Index");
            }
            
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", copilot.AirplaneID);
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
            Copilot copilot = db.Copilots.Include("Airplane").First(x => x.CopilotID == id);
            if (copilot == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", copilot.ApplicationUserID);
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", copilot.AirplaneID);
            return View(copilot);
        }

        // POST: Copilots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CopilotID,ApplicationUserID,Date,Duration,AirplaneID,InstrumentRequired")] Copilot copilot)
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
