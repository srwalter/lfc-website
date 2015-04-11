﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LFC.Models
{
    public class ActiveAlert
    {
        public enum AlertType
        {
            OilChange,
            HundredHour,
            AD,
            Transponder,
            Static,
            Annual,
            ELT,
            ELTBattery,
        };

        [Key, Column(Order = 1)]
        public AlertType Type { get; set; }
        [Key, Column(Order = 2)]
        public String AirplaneID { get; set; }
        public virtual Airplane Airplane { get; set; }
    }
}