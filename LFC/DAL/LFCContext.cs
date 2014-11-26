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
        public LFCContext() : base ("AirplaneContext")
        {
        }

        public static LFCContext Create ()
        {
            return new LFCContext();
        }

        public DbSet<Airplane> Airplanes { get; set; }
    }
}