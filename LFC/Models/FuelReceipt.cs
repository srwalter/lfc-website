using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LFC.Models
{
    public class FuelReceipt
    {
        public int FuelReceiptID { get; set; }
        [Display(Name="Pilot")]
        public String ApplicationUserID { get; set; }
        public String AirplaneID { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        public double Gallons { get; set; }
        [DisplayFormat(DataFormatString="{0:c}")]
        public double Dollars { get; set; }

        public virtual Airplane Airplane { get; set; }
        public virtual ApplicationUser Pilot { get; set; }
    }
}