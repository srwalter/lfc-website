using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LFC.Models
{
    public class BillingEmail
    {
        [Key]
        [DataType(DataType.MultilineText)]
        public String Body { get; set; }
    }
}