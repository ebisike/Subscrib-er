using Subscrib_er.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Subscrib_er.Web.ViewModel.Package
{
    public class PackageViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Package Name")]
        public string PackageName { get; set; }

        [Required]
        [Display(Name = "Dealer Name")]
        public string DealerName { get; set; }

        //[Required]
        //public int Cost { get; set; }
        public bool payNow { get; set; }
        public packagestate packagestate { get; set; }
        public string Description { get; set; }
    }
}
