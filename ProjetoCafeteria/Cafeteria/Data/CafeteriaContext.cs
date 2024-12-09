using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cafeteria.Models;

namespace Cafeteria.Data
{
    public class CafeteriaContext : DbContext
    {
        public CafeteriaContext (DbContextOptions<CafeteriaContext> options)
            : base(options)
        {
        }

        public DbSet<Cafeteria.Models.Product> Product { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
        public DbSet<OrderItem> OrderItem { get; set; } = default!;
    }
}
