using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using LFC.Models;

namespace LFC.ViewModels
{
    public class TachEntry
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public String PilotName { get; set; }
        public double StartTach { get; set; }
        public double? EndTach { get; set; }
    }
    public class HobbsViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:MM/dd/yyyy}")]
        [Required]
        public DateTime Date { get; set; }
        [Display(Name="N-number")]
        public String AirplaneID { get; set; }
        public virtual Airplane Airplane { get; set; }
        [Display(Name="Start Tach")]
        public double StartTach { get; set; }
        [Display(Name="End Tach")]
        public double EndTach { get; set; }
        [Display(Name="Start Hobbs")]
        public double StartHobbs { get; set; }
        [Display(Name="End Hobbs")]
        public double EndHobbs { get; set; }
        public List<TachEntry> TachEntries { get; set; }
        public IEnumerable<ApplicationUser> AllUsers { get; set; }
    }
}