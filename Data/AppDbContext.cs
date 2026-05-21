namespace OP_Project.Data;
using OP_Project.Models;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> users { get; set; }

    public DbSet<Car> cars { get; set; }

    public DbSet<Feature> features { get; set; }

    public DbSet<CarFeature> car_features { get; set; }

    public DbSet<Rental> rentals { get; set; }
}