using AutoMapper;
using CarRental.Application.DTOs.Car;
using CarRental.Application.DTOs.Payment;
using CarRental.Application.DTOs.Rental;
using CarRental.Application.DTOs.User;
using CarRental.Application.DTOs.VehicleMaintenance;
using CarRental.Domain.Entities;

namespace CarRental.Application.Common;

/// <summary>
/// AutoMapper profile for entity-to-DTO mappings.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Car mappings
        CreateMap<Car, CarDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.ToString()));

        // Rental mappings
        CreateMap<Rental, RentalDto>()
            .ForMember(d => d.CarInfo, o => o.MapFrom(s => s.Car != null 
                ? $"{s.Car.Year} {s.Car.Make} {s.Car.Model}" 
                : string.Empty))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null 
                ? s.Customer.FullName 
                : string.Empty))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        // Payment mappings
        CreateMap<Payment, PaymentDto>()
            .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.PaymentMethod.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(d => d.Role, o => o.MapFrom(s => s.Role.ToString()));

        // VehicleMaintenance mappings
        CreateMap<VehicleMaintenance, VehicleMaintenanceDto>()
            .ForMember(d => d.CarInfo, o => o.MapFrom(s => s.Car != null 
                ? $"{s.Car.Year} {s.Car.Make} {s.Car.Model}" 
                : string.Empty));
    }
}
