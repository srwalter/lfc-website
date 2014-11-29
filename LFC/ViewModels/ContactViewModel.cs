using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LFC.Models;

namespace LFC.ViewModels
{
    public class ContactViewModel
    {
        public IEnumerable<ApplicationUser> Officers { get; set; }
        public IEnumerable<Airplane> Planes { get; set; }
    }
}