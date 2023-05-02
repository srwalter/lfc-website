using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LFC.ViewModels;
using LFC.Models;
using LFC.DAL;

namespace LFC.Controllers
{
    public class HomeController : Controller
    {
        private LFCContext db = new LFCContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Links()
        {
            return View();
        }
        public ActionResult N89652()
        {
            return View();
        }

        public ActionResult Contact()
        {
            var cvm = new ContactViewModel();
            cvm.Planes = db.Airplanes.Include("MaintenanceOfficer").Where(x => x.Active == true);
            cvm.Officers = db.Users.Where(x => x.Officer != null).OrderBy(x => x.Officer);
            return View(cvm);
        }

        public ActionResult Join()
        {
            return View();
        }

        public ActionResult Bylaws()
        {
            return View();
        }
    }
}