using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LFC.Models
{
    public class FuelReceipt
    {
        public int FuelReceiptID { get; set; }
        public String ApplicationUserID { get; set; }
        public String AirplaneID { get; set; }
        public DateTime Date { get; set; }
        public double Gallons { get; set; }
        public double Dollars { get; set; }

        public virtual Airplane Airplane { get; set; }
        public virtual ApplicationUser Pilot { get; set; }
    }
}