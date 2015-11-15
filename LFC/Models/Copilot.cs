using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LFC.Models
{
    public class Copilot
    {
        public int CopilotID { get; set; }
        [Display(Name ="Pilot")]
        public String ApplicationUserID { get; set; }
        public String AirplaneID { get; set; }
        [Display(Name = "Date and Time")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Display(Name = "Estimated Duration (Hours)")]
        public double Duration { get; set; }

        public virtual ApplicationUser Pilot { get; set; }
        public virtual Airplane Airplane { get; set; }
    }
}