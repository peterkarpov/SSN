using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class SearchProfileListViewModel
    {

        [Display(Name = "First Name")]
        [RegularExpression(@"[A-Za-z0-9]+")]
        [StringLength(50)]
        public string fName { get; set; }
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9]+")]
        [Display(Name = "Last Name")]
        public string lName { get; set; }
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9]+")]
        [Display(Name = "Middle Name")]
        public string mName { get; set; }
        [Display(Name = "Gender")]
        [Remote("CheckGender", "ProfilesList")]
        public Gender? Gender { get; set; }
        [Display(Name = "Born")]
        public DateTime? dob { get; set; }
    }
}
