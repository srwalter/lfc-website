using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LFC.Models
{
    public class Equipment
    {
        public enum EquipmentType {
            [Display(Name="NAV/Comm")]
            NAVComm,
            ADF,
            DME,
            GPS,
            Transponder,
            Autopilot,
            Intercom
        }
        public int EquipmentID { get; set; }
        public String AirplaneID {get; set; }
        public virtual Airplane Airplane { get; set; }
        public EquipmentType Type {get; set;}
        public String Description { get; set; }
    }
}