using Microsoft.AspNetCore.Identity;
using Subscrib_er.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscrib_er.Data
{
    public class ApplicationUser : IdentityUser
    {
        public gender Gender { get; set; }
        public state State { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoPath { get; set; }

    }
}
