using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LFC.Models
{
    public class Airplane
    {
        [Display(Name="N-number")]
        [Required]
        [Key]
        public String AirplaneID { get; set; }
        [Required]
        public String Type { get; set; }
        public String Description { get; set; }
        public int ModelYear { get; set; }
        [DataType(DataType.Currency)]
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
        public double Moment { get; set; }
        public double Arm { get; set; }
        public int Voltage { get; set; }
        [Display(Name="Oil Sump Capacity (Qt.)")]
        public int OilSump { get; set; }

        [Display(Name="Tach Add")]
        public double TachAdd { get; set; }
        [Display(Name="Last Overhaul")]
        public double EngineOverhaul { get; set; }
        [Display(Name="Next Hundred Hour")]
        public double HundredHour { get; set; }
        [Display(Name="Next Oil Change")]
        public double OilChange { get; set; }
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name="Annual Due")]
        public DateTime AnnualDue { get; set; }
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name="ELT Due")]
        public DateTime EltDue { get; set; }
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name="ELT Battery Due")]
        public DateTime EltBatteryDue { get; set; }
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name="Transponder Due")]
        public DateTime TransponderDue { get; set; }
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name="Static Due")]
        public DateTime StaticDue { get; set; }
        public DateTime Updated { get; set; }
        [Display(Name="Updated By")]
        public String UpdatedBy { get; set; }
        [DataType(DataType.MultilineText)]
        public String Comments { get; set; }
        public String MaintenanceOfficerID { get; set; }
        [Display(Name="Maintenance Officer")]
        public virtual ApplicationUser MaintenanceOfficer { get; set; }
        [Display(Name="GPS DB Expires")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:MM/dd/yyyy}", ApplyFormatInEditMode=true)]
        public DateTime GPSExpires { get; set; }
        [Display(Name="Engine Serial")]
        public String EngineSerial { get; set; }
        public bool Active { get; set; }

        public double getCurrentTach()
        {
                var logs = this.HobbsTimes.OrderByDescending(x => x.Date).ToList();
                if (logs.Count == 0)
                    return 0.0;
                return logs[0].TachHours;
        }

        public List<String> MaintenanceActions
        {
            get
            {
                var actions = new List<String>();
                if (HundredHour > 0 && getCurrentTach() > HundredHour)
                {
                    actions.Add("Hundred hour inspection is due");
                }
                if (getCurrentTach() > OilChange)
                {
                    actions.Add("Oil change is due");
                }
                if (DateTime.Now >= AnnualDue)
                {
                    actions.Add("Annual inspection is due");
                }
                else if (DateTime.Now.AddDays(14) >= AnnualDue)
                {
                    actions.Add("Annual inspection is due soon");
                }
                if (DateTime.Now >= EltDue)
                {
                    actions.Add("ELT inspection is due");
                }
                else if (DateTime.Now.AddDays(14) >= EltDue)
                {
                    actions.Add("ELT inspection is due soon");
                }
                if (DateTime.Now >= EltBatteryDue)
                {
                    actions.Add("ELT battery is due for replacement");
                }
                else if (DateTime.Now.AddDays(14) >= EltBatteryDue)
                {
                    actions.Add("ELT battery is due for replacement soon");
                }
                if (DateTime.Now >= TransponderDue)
                {
                    actions.Add("Transponder inspection is due");
                }
                else if (DateTime.Now.AddDays(14) >= TransponderDue)
                {
                    actions.Add("Transponder inspection is due soon");
                }
                if (DateTime.Now >= StaticDue)
                {
                    actions.Add("Pitot/static inspection is due");
                }
                else if (DateTime.Now.AddDays(14) >= StaticDue)
                {
                    actions.Add("Pitot/static inspection is due soon");
                }
                if (DateTime.Now >= GPSExpires)
                {
                    actions.Add("GPS Database is expired");
                }
                return actions;
            }
        }

        public void UpdatedNow (ApplicationUser updatedBy)
        {
            this.Updated = DateTime.Now;
            this.UpdatedBy = updatedBy.ShortName;
        }

        [Display(Name="Equipment")]
        public virtual ICollection<Equipment> InstalledEquipment { get; set; }
        public virtual ICollection<AirworthinessDirective> ADs { get; set; }
        public virtual ICollection<FlightLog> FlightLogs { get; set; }
        public virtual ICollection<HobbsTime> HobbsTimes { get; set; }

        public static List<AirworthinessDirective> PastDueADs (ICollection<AirworthinessDirective> all_ads)
        {
                var ads = new List<AirworthinessDirective>();
                foreach (var ad in all_ads)
                {
                    if (ad.IsOverdue())
                    {
                        ads.Add(ad);
                        continue;
                    }
                }
                return ads;
        }
    }
}