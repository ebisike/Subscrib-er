using Subscrib_er.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscrib_er.Web.ViewModel.Package
{
    public class PackageModel
    {
        public Subscrib_er.Entities.Package package { get; set; }
        public Subscrib_er.Entities.Payments payments { get; set; }
    }
}
