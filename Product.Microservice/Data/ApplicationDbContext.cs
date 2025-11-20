using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Microservice.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.OpenConnection();
            Database.EnsureCreated();

            if (Products.Any() is false)
            {
                Products.Add(new Entities.Product
                {
                    Name = "Computer 1",
                    Rate = 1000
                });

                Products.Add(new Entities.Product
                {
                    Name = "Computer 2",
                    Rate = 1500
                });

                Products.Add(new Entities.Product
                {
                    Name = "Computer 3",
                    Rate = 500
                });

                SaveChanges();
            }
        }

        public DbSet<Entities.Product> Products { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
