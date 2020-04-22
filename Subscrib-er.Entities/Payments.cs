using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subscrib_er.Entities
{
    public class Payments
    {
        [Key]
        public Guid Id { get; set; }
        public string userId { get; set; }
        public Guid packageId { get; set; }
        public Package package { get; set; }
        public DateTime startDate { get; set; }
        //public int packageDuration { get; set; }
        public DateTime endDate { get; set; }
        public double amount { get; set; }
        public style PackageStyle { get; set; }
    }

    public enum style
    {
        Daily,
        Weekly,
        Monthly,
        Yearly
    }
}
