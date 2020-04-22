using Subscrib_er.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Subscrib_er.Web.ViewModel.Payments
{
    public class PaymentViewModel
    {
        public Guid ID { get; set; }
        public Guid packageID { get; set; }
        public string userID { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        //[Required]
        //public int duration { get; set; }
        
        [Required]
        public double Cost { get; set; }
        //public double InitialCost { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Subscription style")]
        public style style { get; set; }
    }
}
