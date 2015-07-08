using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

using LFC.Models;

namespace LFC.ViewModels
{
    public class EmailIndividualViewModel
    {
            [Required]
            public String Subject { get; set; }
            [DataType(DataType.MultilineText)]
            [Required]
            public String Body { get; set; }
    }
}