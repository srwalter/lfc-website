using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LFC.Controllers
{
    [Authorize]
    public class MembersAreaController : Controller
    {
        // GET: MembersArea
        public ActionResult Index()
        {
            return View();
        }
    }
}