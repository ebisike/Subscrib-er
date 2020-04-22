using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Subscrib_er.Web.Models
{
    public class CreateRoleViewModel
    { 
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
