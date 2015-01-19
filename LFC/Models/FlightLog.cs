using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LFC.Models
{
    public class FlightLog
    {
        public int FlightLogID { get; set; }
        [Required]
        [Display(Name = "Airplane")]
        public String AirplaneID { get; set; }
        [Required]
        [Display(Name = "Pilot")]
        public String ApplicationUserID { get; set; }
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public DateTime Date { get; set; }
        [Display(Name = "Start Tach")]
        public double StartTach { get; set; }
        [Display(Name = "End Tach")]
        public double EndTach { get; set; }
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:MM/dd/yyyy}")]
        public DateTime? Billed { get; set; }

        public virtual Airplane Airplane { get; set; }
        public virtual ApplicationUser Pilot { get; set; }
    }
    
}