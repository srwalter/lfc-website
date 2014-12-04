using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace LFC.Models
{
    public class HobbsTime
    {
        [Key]
        public DateTime Date { get; set; }
        [Key]
        public String AirplaneID { get; set; }
        public virtual Airplane Airplane { get; set; }
        [Display(Name="Hobbs Hours")]
        public double HobbsHours { get; set; }
    }
}