using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Models;

namespace Restaurant_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<FoodItem> FoodItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<TableBooking> TableBookings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.FoodItemId });

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.FoodItem)
                .WithMany(fi => fi.OrderItems)
                .HasForeignKey(oi => oi.FoodItemId);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
            .WithMany(u => u.Customers)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        }

        internal object FindById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
