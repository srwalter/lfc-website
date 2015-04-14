using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace LFC.ViewModels
{
    public class ChangePasswordAdminViewModel
    {
        [Required]
        public String UserID { get; set; }
        [Display(Name = "New Password")]
        [Required]
        public String NewPassword { get; set; }
    }
}