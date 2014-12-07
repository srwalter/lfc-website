using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LFC.ViewModels
{
    public class EmailViewModel
    {
        public String Subject { get; set; }
        [DataType(DataType.MultilineText)]
        public String Body { get; set; }
    }
}