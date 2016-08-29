using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class ProfileViewModel
    {
        public Profile Profile { get; set; }

        public User User { get; set; }

        public Dictionary<string, int> Statistic { get; set; }

    }
}