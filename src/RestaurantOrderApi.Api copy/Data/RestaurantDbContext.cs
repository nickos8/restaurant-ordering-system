using Microsoft.EntityFrameworkCore;
using RestaurantOrderApi.Api.Models;

namespace RestaurantOrderApi.Api.Data;

public class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
    {
    }

    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItem>()
            .Property(m => m.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.UnitPrice)
            .HasPrecision(10, 2);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.MenuItem)
            .WithMany()
            .HasForeignKey(oi => oi.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, Name = "Adobo Rice Bowl", Category = "Main", Price = 150.00m, IsAvailable = true },
            new MenuItem { Id = 2, Name = "Sinigang na Baboy", Category = "Main", Price = 180.00m, IsAvailable = true },
            new MenuItem { Id = 3, Name = "Iced Tea", Category = "Beverage", Price = 45.00m, IsAvailable = true },
            new MenuItem { Id = 4, Name = "Halo-Halo", Category = "Dessert", Price = 95.00m, IsAvailable = true }
        );
    }
}
