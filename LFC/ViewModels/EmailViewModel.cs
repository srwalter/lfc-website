using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LFC.ViewModels
{
    public enum Recipients
    {
        All,
        Full,
        Restricted,
        Special,
        Inactive,
        Associate,
        Officers,
        [Display(Name = "Line Officers")]
        LineOfficers,
        [Display(Name = "Maintenance Officers")]
        MaintenanceOfficers,
        [Display(Name = "Safety Officer")]
        SafetyOfficer,
        [Display(Name = "Diamond Flyers")]
        DiamondFlyers,
    }

    public class EmailViewModel
    {
        public String Subject { get; set; }
        public List<Recipients> Recipients { get; set; }
        [DataType(DataType.MultilineText)]
        public String Body { get; set; }
    }
}