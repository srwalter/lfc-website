using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LFC.Models
{
    public class AirworthinessDirective
    {
        [Key]
        public int KeyNum { get; set; }
        [Display(Name = "AD name")]
        public String AirworthinessDirectiveID { get; set; }
        [Display(Name = "Airplane")]
        public String AirplaneID { get; set; }
        public String Description { get; set; }
        [Display(Name = "Frequency (in hours)")]
        public int? FrequencyHours { get; set; }
        [Display(Name = "Frequency (in months)")]
        public int? FrequencyMonths { get; set; }
        [Display(Name = "Frequency (other)")]
        public String FrequencyMisc { get; set; }
        [Display(Name = "Last done (hours)")]
        public double? LastDoneHours { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode=true)]
        [Display(Name = "Last done (date)")]
        public DateTime? LastDoneDate { get; set; }

        public virtual Airplane Airplane { get; set; }
    }
}