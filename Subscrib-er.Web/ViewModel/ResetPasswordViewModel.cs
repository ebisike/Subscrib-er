using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Subscrib_er.Web.ViewModel
{
    public class ResetPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required] 
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        public string token { get; set; }
    }
}
