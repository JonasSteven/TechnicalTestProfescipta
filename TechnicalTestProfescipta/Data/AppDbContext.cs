using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TechnicalTestProfescipta.Models;

namespace TechnicalTestProfescipta.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<SOOrder> SOOrders { get; set; }
        public DbSet<SOItem> SOItems { get; set; }
        public DbSet<COMCustomer> COMCustomers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SOOrder>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.COMCustomerId);

            modelBuilder.Entity<SOItem>()
                .HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.SOOrderId);
        }
    }
}
