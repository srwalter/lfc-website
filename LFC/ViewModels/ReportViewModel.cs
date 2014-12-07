using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LFC.ViewModels
{
    public class BillingReport
    {
        public String Plane { get; set; }
        [Display(Name="Billing Name")]
        public String BillingName { get; set; }
        public DateTime Date { get; set; }
        [DisplayFormat(DataFormatString="{0:F2}")]
        public double Hours { get; set; }
    }

    public class FlyingReport
    {
        public String Plane { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Display(Name="Start Hobbs")]
        public double StartHobbs;
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Display(Name="End Hobbs")]
        public double EndHobbs;
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Display(Name="Start Tach")]
        public double StartTach;
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Display(Name="End Tach")]
        public double EndTach;
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Display(Name="Billed Tach")]
        public double BilledTach;
    }

    public class ReportViewModel
    {
        public IEnumerable<FlyingReport> Flying { get; set; }
        public IEnumerable<BillingReport> Billing { get; set; }
    }
}