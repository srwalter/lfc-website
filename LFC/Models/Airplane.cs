using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace LFC.Models
{
    public class Airplane
    {
        [Display(Name="N-number")]
        public String AirplaneID { get; set; }
        [Required]
        public String Type { get; set; }
        public String Description { get; set; }
        public float Rate { get; set; }
        public String Serial { get; set; }
        [Display(Name="Engine Mfg.")]
        public String EngineMake { get; set; }
        [Display(Name="Engine Type")]
        public String EngineModel { get; set; }
        public int HP { get; set; }
        [Display(Name="Cruise Speed")]
        public int CruiseSpeed { get; set; }
        [Display(Name="Cruise Altitude")]
        public int CruiseAlt { get; set; }
        public int Range { get; set; }
        [Display(Name="Best Range Altitude")]
        public int RangeAlt { get; set; }
        [Display(Name="Empty Weight")]
        public float EmptyWt { get; set; }
        [Display(Name="Gross Weight")]
        public float GrossWt {get; set;}
        [Display(Name="Total Fuel")]
        public float TotalFuel { get; set; }
        [Display(Name="Usable Fuel")]
        public float UsableFuel { get; set; }
        public float Moment { get; set; }
        public float Arm { get; set; }
        public int Voltage { get; set; }
        [Display(Name="Oil Sump Capacity (Qt.)")]
        public int OilSump { get; set; }
    }
}