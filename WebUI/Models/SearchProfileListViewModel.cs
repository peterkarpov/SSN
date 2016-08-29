using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class SearchProfileListViewModel
    {
        public string fName { get; set; }
        public string lName { get; set; }
        public string mName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? dob { get; set; }
    }
}
