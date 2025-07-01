﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Microservice.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Entities.Order> Orders { get; set; }
        Task<int> SaveChangesAsync();
    }
}
