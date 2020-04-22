using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subscrib_er.Entities
{
    public class Package
    {
        [Key]
        public Guid Id { get; set; }
        public string  userId { get; set; }
        public string PackageName { get; set; }
        public string DealerName { get; set; }
        //public int Cost { get; set; }
        public packagestate Status { get; set; }
        public string Description { get; set; }
    }

    public enum packagestate
    {
        Active,
        Expired,
        Inactive
    }
}
