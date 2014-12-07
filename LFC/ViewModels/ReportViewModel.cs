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
        public String BillingName { get; set; }
        public DateTime Date { get; set; }
        [DisplayFormat(DataFormatString="{0:F2}")]
        public double Hours { get; set; }
    }

    public class FlyingReport
    {
        public String Plane { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double StartHobbs;
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double EndHobbs;
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double StartTach;
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double EndTach;
    }

    public class ReportViewModel
    {
        public IEnumerable<FlyingReport> Flying { get; set; }
        public IEnumerable<BillingReport> Billing { get; set; }
    }
}