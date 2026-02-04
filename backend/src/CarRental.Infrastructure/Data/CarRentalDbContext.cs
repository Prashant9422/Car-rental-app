using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for Car Rental application.
/// </summary>
public class CarRentalDbContext : DbContext
{
    public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
    {
    }

    public DbSet<Car> Cars => Set<Car>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<VehicleMaintenance> VehicleMaintenances => Set<VehicleMaintenance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Car configuration
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Make).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Model).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LicensePlate).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.LicensePlate).IsUnique();
            entity.Property(e => e.Color).IsRequired().HasMaxLength(30);
            entity.Property(e => e.DailyRate).HasPrecision(10, 2);
            entity.Property(e => e.FuelType).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Transmission).IsRequired().HasMaxLength(30);
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
        });

        // Rental configuration
        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DailyRate).HasPrecision(10, 2);
            entity.Property(e => e.TotalCost).HasPrecision(10, 2);
            entity.Property(e => e.LateFee).HasPrecision(10, 2);
            entity.Property(e => e.DamageCharges).HasPrecision(10, 2);

            entity.HasOne(e => e.Car)
                .WithMany(c => c.Rentals)
                .HasForeignKey(e => e.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Customer)
                .WithMany(u => u.Rentals)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Payment configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(10, 2);

            entity.HasOne(e => e.Rental)
                .WithMany(r => r.Payments)
                .HasForeignKey(e => e.RentalId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // VehicleMaintenance configuration
        modelBuilder.Entity<VehicleMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.ServiceType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Cost).HasPrecision(10, 2);
            entity.Property(e => e.ServiceProvider).HasMaxLength(100);

            entity.HasOne(e => e.Car)
                .WithMany(c => c.MaintenanceRecords)
                .HasForeignKey(e => e.CarId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Rental deposit precision
        modelBuilder.Entity<Rental>(entity =>
        {
            entity.Property(e => e.DepositAmount).HasPrecision(10, 2);
            entity.Property(e => e.PickupLocation).HasMaxLength(500);
            entity.Property(e => e.ReturnLocation).HasMaxLength(500);
        });

        // Seed initial data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var customerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        
        // Seed admin user
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = adminId,
            Email = "admin@carrental.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            FirstName = "System",
            LastName = "Admin",
            PhoneNumber = "+1234567890",
            Role = Domain.Enums.UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        // Seed sample customer
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = customerId,
            Email = "customer@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@123"),
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "+1987654321",
            Role = Domain.Enums.UserRole.Customer,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        // Seed sample cars
        modelBuilder.Entity<Car>().HasData(
            new Car
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Make = "Toyota",
                Model = "Camry",
                Year = 2024,
                LicensePlate = "ABC-1234",
                Color = "Silver",
                Mileage = 5000,
                DailyRate = 45.00m,
                Status = Domain.Enums.CarStatus.Available,
                Category = Domain.Enums.CarCategory.Midsize,
                SeatingCapacity = 5,
                FuelType = "Gasoline",
                Transmission = "Automatic",
                HasAirConditioning = true,
                HasGPS = true,
                Description = "Comfortable and fuel-efficient sedan",
                CreatedAt = DateTime.UtcNow
            },
            new Car
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Make = "Honda",
                Model = "CR-V",
                Year = 2024,
                LicensePlate = "XYZ-5678",
                Color = "Blue",
                Mileage = 3000,
                DailyRate = 65.00m,
                Status = Domain.Enums.CarStatus.Available,
                Category = Domain.Enums.CarCategory.SUV,
                SeatingCapacity = 5,
                FuelType = "Gasoline",
                Transmission = "Automatic",
                HasAirConditioning = true,
                HasGPS = true,
                Description = "Spacious SUV perfect for family trips",
                CreatedAt = DateTime.UtcNow
            },
            new Car
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Make = "BMW",
                Model = "3 Series",
                Year = 2024,
                LicensePlate = "LUX-9999",
                Color = "Black",
                Mileage = 2000,
                DailyRate = 120.00m,
                Status = Domain.Enums.CarStatus.Available,
                Category = Domain.Enums.CarCategory.Luxury,
                SeatingCapacity = 5,
                FuelType = "Gasoline",
                Transmission = "Automatic",
                HasAirConditioning = true,
                HasGPS = true,
                Description = "Premium luxury sedan with advanced features",
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
