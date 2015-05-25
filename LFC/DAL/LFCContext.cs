using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using LFC.Models;

namespace LFC.DAL
{
    public class LFCContext : IdentityDbContext<ApplicationUser>
    {
        public LFCContext() : base ("LFCContext")
        {
        }

        public static LFCContext Create ()
        {
            return new LFCContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static string RenderViewToString(ControllerContext context,
                string viewPath, object model)
        {
            var viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;
            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }
            return result;
        }
        
        public DbSet<Airplane> Airplanes { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.Equipment> Equipments { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.AirworthinessDirective> AirworthinessDirectives { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.FlightLog> FlightLogs { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.HobbsTime> HobbsTimes { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.Instructor> Instructors { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.AirplaneCheckout> AirplaneCheckouts { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.ActiveAlert> ActiveAlerts { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.FuelReceipt> FuelReceipts { get; set; }
    }
}