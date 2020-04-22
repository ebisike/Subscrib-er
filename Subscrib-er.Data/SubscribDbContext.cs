using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Subscrib_er.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscrib_er.Data
{
    public class SubscribDbContext : IdentityDbContext<ApplicationUser>
    {
        public SubscribDbContext(DbContextOptions<SubscribDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
       
        public DbSet<Package> packages { get; set; }
        public DbSet<Payments> payments { get; set; }
    }
}
