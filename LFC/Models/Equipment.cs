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
            [Display(Name ="WAAS GPS")]
            GPSWaas,
            Transponder,
            Autopilot,
            Intercom,
            HSI,
            [Display(Name = "Electronic HSI")]
            ElectronicHSI,
            [Display(Name = "Audio Panel")]
            AudioPanel,
            [Display(Name = "ADS-B Out")]
            ADSBOut,
            [Display(Name = "ADS-B In/Out")]
            ADSBInOut,
            EGT,
            CHT,
            [Display(Name = "NAV Indicator")]
            NAVIndicator,
            [Display(Name = "Glideslope Indicator")]
            GSIndicator,
            MFD,
            [Display(Name = "Engine Monitor")]
            EngineMonitor
        }
        public int EquipmentID { get; set; }
        public String AirplaneID {get; set; }
        public virtual Airplane Airplane { get; set; }
        public EquipmentType Type {get; set;}
        public String Description { get; set; }
    }
}