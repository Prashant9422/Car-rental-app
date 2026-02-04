using System.Text;
using CarRental.API.Filters;
using CarRental.API.Middleware;
using CarRental.Application.Common;
using CarRental.Application.Interfaces;
using CarRental.Application.Services;
using CarRental.Application.Validators;
using CarRental.Infrastructure;
using CarRental.Infrastructure.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/carrental-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCarValidator>();

// Add Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Configure RentalSettings
builder.Services.Configure<RentalSettings>(
    builder.Configuration.GetSection(RentalSettings.SectionName));

// Register PricingService
builder.Services.AddScoped<IPricingService, PricingService>();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "CarRentalSecretKey2024!@#$%^&*()_+";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "CarRentalAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "CarRentalClients";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Car Rental API",
        Version = "v1",
        Description = "A comprehensive Car Rental Management API built with .NET 8 and Clean Architecture",
        Contact = new OpenApiContact
        {
            Name = "Car Rental Support",
            Email = "support@carrental.com"
        }
    });

    // Include XML comments for documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Include XML comments from Application layer
    var appXmlFile = "CarRental.Application.xml";
    var appXmlPath = Path.Combine(AppContext.BaseDirectory, appXmlFile);
    if (File.Exists(appXmlPath))
    {
        c.IncludeXmlComments(appXmlPath);
    }

    // Use default values from DefaultValue attributes
    c.UseAllOfToExtendReferenceSchemas();

    // Add JWT Security Definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<CarRentalDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Rental API V1");
    c.RoutePrefix = "swagger";
});

// Use global exception handler
app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// Ensure database is created and seed data is applied
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
    db.Database.EnsureCreated();
}

Log.Information("Car Rental API is starting...");
app.Run();
