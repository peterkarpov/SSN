using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Enter Login")]
        [Display(Name = "Login, also your User Name")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "The User Name should be between 3 and 10 characters")]
        [RegularExpression(@"[A-Za-z0-9]+", ErrorMessage = "Wrong login")]
        [System.Web.Mvc.Remote("CheckUserName", "Authentication", "This login already using")]
        public string UserName { get; set; }
        


        [Required(ErrorMessage = "Enter password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "The password should be between 4 and 8 characters")]
        public string Password { get; set; }



        [Compare("Password", ErrorMessage = "Passwords not compare")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }



        [Required(ErrorMessage = "Enter email")]
        [Display(Name = "Email")]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Wrong email")]
        [RegularExpression(@"[A-Za-z0-9]+(\.[A-Za-z0-9]+)*@[A-Za-z0-9]+(\.[A-Za-z]{2,4})+", ErrorMessage = "Wrong email")]
        public string Email { get; set; }
    }
}