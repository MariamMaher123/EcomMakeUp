using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcomMakeUp.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        // Add DbSet properties for your models/entities
        public DbSet<Products> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderProduct> ProductsProducts { get; set; }
        public DbSet<Payment> Payments { get; set; }

        // Override OnModelCreating if needed
      
    }
}
