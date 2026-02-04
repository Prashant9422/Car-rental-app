using CarRental.Application.Interfaces;
using CarRental.Application.Services;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using CarRental.Infrastructure.Repositories;
using CarRental.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.Infrastructure;

/// <summary>
/// Extension methods for configuring infrastructure services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<CarRentalDbContext>(options =>
            options.UseSqlite(connectionString));

        // Add repositories
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add services
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRentalService, RentalService>();
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}
