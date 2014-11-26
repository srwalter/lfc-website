using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using LFC.Models;

namespace LFC.DAL
{
    public class LFCContext : DbContext
    {
        public LFCContext() : base ("AirplaneContext")
        {
        }

        public DbSet<Airplane> Airplanes { get; set; }
    }
}