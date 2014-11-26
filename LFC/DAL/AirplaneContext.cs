using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using LFC.Models;

namespace LFC.DAL
{
    public class AirplaneContext : DbContext
    {
        public AirplaneContext() : base ("AirplaneContext")
        {
        }

        public DbSet<Airplane> Airplanes { get; set; }
    }
}