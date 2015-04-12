using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LFC.Models;

namespace LFC.ViewModels
{
    public class AlertEmail
    {
        public String AirplaneID;
        public ICollection<ActiveAlert.AlertType> Alerts;
    }
}