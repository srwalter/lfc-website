using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
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
        
        public DbSet<Airplane> Airplanes { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.Equipment> Equipments { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.AirworthinessDirective> AirworthinessDirectives { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.FlightLog> FlightLogs { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.HobbsTime> HobbsTimes { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.Instructor> Instructors { get; set; }

        public System.Data.Entity.DbSet<LFC.Models.AirplaneCheckout> AirplaneCheckouts { get; set; }
    }
}