using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class AuthenticationViewModel
    {
        [Required(ErrorMessage = "Enter Login")]
        [Display(Name = "Login")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}