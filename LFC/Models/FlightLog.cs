using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LFC.Models
{
    public class FlightLog
    {
        public int FlightLogID { get; set; }
        [Required]
        public String AirplaneID { get; set; }
        [Required]
        public String ApplicationUserID { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime Date {get; set;}
        public double StartTach {get; set;}
        public double EndTach {get; set;}

        public virtual Airplane Airplane {get; set; }
        public virtual ApplicationUser Pilot { get; set; }
    }
}