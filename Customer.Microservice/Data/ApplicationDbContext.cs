using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Microservice.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.OpenConnection();
            Database.EnsureCreated();

            if (Customers.Any() is false)
            {
                Customers.Add(new Entities.Customer
                {
                    City = "Moscow",
                    Name = "Guest"
                });

                SaveChanges();
            }
        }

        public DbSet<Entities.Customer> Customers{ get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
