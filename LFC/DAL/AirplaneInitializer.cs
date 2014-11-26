using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using LFC.Models;

namespace LFC.DAL
{
    public class AirplaneInitializer : System.Data.Entity. DropCreateDatabaseIfModelChanges<AirplaneContext>
    {
        protected override void Seed(AirplaneContext context)
        {
            var planes = new List<Airplane>
            {
                new Airplane{AirplaneID="89652", Type="Cessna 152", Rate=84.25F},
                new Airplane{AirplaneID="75903", Type="Cessna 172", Rate=114F}
            };
            planes.ForEach(s => context.Airplanes.Add(s));
            context.SaveChanges();
            base.Seed(context);
        }
    }
}