using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LFC.Models
{
    public class Instructor
    {
        public int InstructorID { get; set; }
        [Display(Name="First Name")]
        public String FirstName { get; set; }
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        public bool CFII { get; set; }
        public bool MEI { get; set; }
        public bool ASEL { get; set; }
        public bool ASES { get; set; }
        [Display(Name="Other Ratings")]
        public String OtherRatings { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        [Display(Name="Zip Code")]
        public int? ZipCode { get; set; }
        [Display(Name="Daytime Phone")]
        public String DayPhone { get; set; }
        [Display(Name="Evening Phone")]
        public String EveningPhone { get; set; }
        [Display(Name="Cell Phone")]
        public String CellPhone { get; set; }
        public String Email { get; set; }
        public String Available { get; set; }
        public bool Retired { get; set; }
        public bool DiamondApproved { get; set; }
    }
}