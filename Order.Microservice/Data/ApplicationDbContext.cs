using Microsoft.EntityFrameworkCore;
using Order.Microservice.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Microservice.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.OpenConnection();
            Database.EnsureCreated();

            if (Orders.Any() is false)
            {
                Orders.Add(new Entities.Order
                {
                    CustomerId = 1,
                    ProductId = 1,
                    Date = DateTime.Now,
                    OrderStatus = (int)OrderStatus.InProgress
                });

                Orders.Add(new Entities.Order
                {
                    CustomerId = 1,
                    ProductId = 2,
                    Date = DateTime.Now,
                    OrderStatus = (int)OrderStatus.InProgress
                });

                Orders.Add(new Entities.Order
                {
                    CustomerId = 1,
                    ProductId = 3,
                    Date = DateTime.Now,
                    OrderStatus = (int)OrderStatus.InProgress
                });

                SaveChanges();
            }
        }

        public DbSet<Entities.Order> Orders { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
