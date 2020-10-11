using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using LFC.DAL;
using LFC.Models;

using Microsoft.AspNet.Identity;

namespace LFC.Controllers
{
    [Authorize]
    public class FuelReceiptsController : Controller
    {
        private LFCContext db = new LFCContext();

        // GET: FuelReceipts
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var fuelReceipts = db.FuelReceipts.Include(f => f.Airplane).Include(f => f.Pilot);
            return View(fuelReceipts.ToList());
        }

        [Authorize(Roles = "Admin")]
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
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID");
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName");
            return View();
        }

        private void EmailFuelReceipt(FuelReceipt fr, HttpPostedFileBase image)
        {
            var message = new MailMessage();
            message.From = new MailAddress("info@lexingtonflyingclub.org", "Lexington Flying Club");
            message.Subject = "LFC Fuel Receipt for " + fr.AirplaneID;

            var body = "Pilot: {0}\n" +
                        "Plane: {1}\n" +
                        "Date: {2}\n" +
                        "Dollars: {3}\n" +
                        "Gallons: {4}\n";

            String tres = (from u in db.Users
                           where u.Officer == ApplicationUser.OfficerTitle.Treasurer
                           select u.Email).First();
            //message.To.Add(tres);
            message.To.Add("stevenrwalter@gmail.com");

            String pilot = (from u in db.Users
                            where u.Id == fr.ApplicationUserID
                            select u.ShortName).First();
            message.Body = String.Format(body, pilot, fr.AirplaneID, fr.Date, fr.Dollars, fr.Gallons);
            Attachment data = new Attachment(image.InputStream, image.FileName, MediaTypeNames.Application.Octet);
            message.Attachments.Add(data);

            var smtp = new SmtpClient();
            smtp.Send(message);
        }

        // POST: FuelReceipts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FuelReceiptID,ApplicationUserID,AirplaneID,Date,Gallons,Dollars")] FuelReceipt fuelReceipt, HttpPostedFileBase image)
        {
            if (ModelState.IsValid && image != null)
            {
                if (!User.IsInRole("Admin"))
                {
                    fuelReceipt.ApplicationUserID = User.Identity.GetUserId();
                }
                EmailFuelReceipt(fuelReceipt, image);
                db.FuelReceipts.Add(fuelReceipt);
                db.SaveChanges();

                ViewBag.Message = "Fuel receipt submitted successfully";
                ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", fuelReceipt.AirplaneID);
                ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", fuelReceipt.ApplicationUserID);
                return View();
            }

            if (image == null)
            {
                ViewBag.Message = "Must Attach Receipt Photo";
            }

            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", fuelReceipt.AirplaneID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", fuelReceipt.ApplicationUserID);
            return View(fuelReceipt);
        }

        // GET: FuelReceipts/Edit/5
        [Authorize(Roles = "Admin")]
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
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", fuelReceipt.AirplaneID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", fuelReceipt.ApplicationUserID);
            return View(fuelReceipt);
        }

        // POST: FuelReceipts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "FuelReceiptID,ApplicationUserID,AirplaneID,Date,Gallons,Dollars")] FuelReceipt fuelReceipt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fuelReceipt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AirplaneID = new SelectList(db.Airplanes, "AirplaneID", "AirplaneID", fuelReceipt.AirplaneID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "ShortName", fuelReceipt.ApplicationUserID);
            return View(fuelReceipt);
        }

        // GET: FuelReceipts/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
