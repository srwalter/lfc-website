using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LFC.Models
{
    public class AirplaneCheckout
    {
        [Key, Column(Order = 0)]
        public String PilotID { get; set; }
        public virtual ApplicationUser Pilot { get; set; }
        [Key, Column(Order = 1)]
        public String AirplaneID { get; set; }
        public virtual Airplane Airplane { get; set; }
    }
}