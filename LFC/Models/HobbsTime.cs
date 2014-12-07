using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace LFC.Models
{
    public class HobbsTime
    {
        [Key]
        [Column(Order=1)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        [Key]
        [Column(Order=2)]
        public String AirplaneID { get; set; }
        public virtual Airplane Airplane { get; set; }
        [Display(Name="Hobbs Hours")]
        public double HobbsHours { get; set; }
        [Display(Name = "Tach Hours")]
        public double TachHours { get; set; }
    }
}