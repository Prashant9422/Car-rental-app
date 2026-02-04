# Car Rental Management API

A comprehensive **Car Rental Management System** built with **.NET 8** and **Clean Architecture** principles. This RESTful API provides complete functionality for managing car rentals, including vehicle inventory, customer management, booking system, and payment processing.

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![SQLite](https://img.shields.io/badge/SQLite-Database-003B57?logo=sqlite)
![License](https://img.shields.io/badge/license-MIT-green)

## 📋 Table of Contents

- [Why This Project?](#-why-this-project)
- [Features](#-features)
- [Architecture](#-architecture)
- [Technology Stack](#-technology-stack)
- [Getting Started](#-getting-started)
- [API Documentation](#-api-documentation)
- [Database Schema](#-database-schema)
- [Authentication](#-authentication)
- [Project Structure](#-project-structure)
- [Configuration](#-configuration)
- [Testing the API](#-testing-the-api)

## 🎯 Why This Project?

### Business Purpose

The Car Rental Management API solves real-world problems faced by car rental businesses:

1. **Inventory Management** - Track and manage a fleet of vehicles with detailed specifications
2. **Customer Management** - Maintain customer profiles with driver's license validation
3. **Booking System** - Handle rental reservations with date conflict detection
4. **Payment Processing** - Support multiple payment methods and track transaction history
5. **Business Analytics** - Monitor rental revenue, car utilization, and customer behavior
6. **Multi-tenant Support** - Role-based access control for Admin, Staff, and Customer roles

### Technical Purpose

This project demonstrates:

- **Clean Architecture** principles for maintainable, testable code
- **Repository Pattern** with Unit of Work for data access
- **CQRS-like** separation with DTOs and domain models
- **JWT Authentication** for secure API access
- **Input Validation** using FluentValidation
- **Global Exception Handling** for consistent error responses
- **Logging** with Serilog for debugging and monitoring
- **API Documentation** with Swagger/OpenAPI

## ✨ Features

### Core Features

- ✅ **User Management**
  - User registration and authentication
  - Role-based access control (Admin, Staff, Customer)
  - Profile management with driver's license tracking
  - Password hashing with BCrypt

- ✅ **Car Management**
  - Complete CRUD operations for vehicle inventory
  - Car categorization (Economy, Standard, Premium, SUV, Luxury, Sports)
  - Status tracking (Available, Rented, Maintenance, OutOfService)
  - Detailed car specifications (fuel type, transmission, features)
  - License plate uniqueness validation

- ✅ **Rental Management**
  - Create, update, and cancel rental bookings
  - Date conflict detection (prevent double-booking)
  - Automatic cost calculation based on rental duration
  - Late fee calculation for overdue returns
  - Odometer tracking at pickup and return
  - Rental status workflow (Pending → Active → Completed/Cancelled)

- ✅ **Payment Processing**
  - Multiple payment methods (CreditCard, DebitCard, Cash, BankTransfer)
  - Payment status tracking (Pending, Completed, Failed, Refunded)
  - Associate payments with rentals
  - Transaction ID tracking

### Additional Features

- 🔐 JWT-based authentication with refresh tokens
- 📊 Pagination support for large datasets
- ✔️ Input validation with FluentValidation
- 🛡️ Global exception handling
- 📝 Structured logging with Serilog
- 🏥 Health check endpoints
- 📖 Swagger UI for API testing
- 🌐 CORS configuration for frontend integration

## 🏗️ Architecture

This project follows **Clean Architecture** (also known as Onion Architecture) principles, ensuring separation of concerns and maintainability.

```
┌─────────────────────────────────────────────────┐
│              CarRental.API                      │
│  (Controllers, Middleware, Filters)             │
└────────────────┬────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────┐
│         CarRental.Application                   │
│  (Services, DTOs, Validators, Interfaces)       │
└────────────────┬────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────┐
│         CarRental.Infrastructure                │
│  (Repositories, DbContext, JWT Service)         │
└────────────────┬────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────┐
│          CarRental.Domain                       │
│  (Entities, Enums, Exceptions, Interfaces)      │
└─────────────────────────────────────────────────┘
```

### Layer Responsibilities

#### 1. **Domain Layer** (`CarRental.Domain`)
- **Purpose**: Core business logic and entities
- **Contains**:
  - Domain entities (Car, User, Rental, Payment)
  - Business enums (UserRole, CarStatus, RentalStatus, etc.)
  - Domain interfaces (IRepository, IUnitOfWork)
  - Custom exceptions (NotFoundException, ValidationException)
- **Dependencies**: None (pure domain logic)

#### 2. **Application Layer** (`CarRental.Application`)
- **Purpose**: Application business logic and use cases
- **Contains**:
  - Service interfaces and implementations
  - DTOs (Data Transfer Objects) for API contracts
  - FluentValidation validators
  - Common result patterns
- **Dependencies**: Domain layer only

#### 3. **Infrastructure Layer** (`CarRental.Infrastructure`)
- **Purpose**: External concerns and data access
- **Contains**:
  - Entity Framework Core DbContext
  - Repository implementations
  - JWT token service
  - Database migrations and seeding
- **Dependencies**: Domain and Application layers

#### 4. **API Layer** (`CarRental.API`)
- **Purpose**: HTTP/REST API endpoints
- **Contains**:
  - Controllers (Auth, Cars, Users, Rentals, Payments)
  - Middleware (Global exception handler)
  - Filters (Validation filter)
  - Dependency injection configuration
- **Dependencies**: All layers

## 🛠️ Technology Stack

### Backend Framework
- **.NET 8** - Latest long-term support version
- **ASP.NET Core Web API** - RESTful API framework
- **C# 12** - Modern language features

### Database
- **Entity Framework Core 8** - ORM for data access
- **SQLite** - Lightweight, file-based database (easily replaceable with SQL Server, PostgreSQL, etc.)

### Security & Authentication
- **JWT Bearer Tokens** - Stateless authentication
- **BCrypt.Net** - Password hashing

### Validation & Documentation
- **FluentValidation** - Input validation
- **Swashbuckle (Swagger)** - API documentation

### Logging & Monitoring
- **Serilog** - Structured logging to console and files
- **Health Checks** - Database and application health monitoring

### Development Tools
- **Hot Reload** - Fast development cycle
- **Nullable Reference Types** - Improved null safety

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed
- Code editor (Visual Studio, VS Code, Rider, etc.)
- (Optional) Postman or similar API testing tool

### Installation

1. **Clone or navigate to the project directory**
   ```bash
   cd /Users/prashantkumar/Desktop/CarRental-backend
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the API**
   ```bash
   dotnet run --project src/CarRental.API/CarRental.API.csproj
   ```

5. **Access the application**
   - API Base URL: `http://localhost:5000`
   - Swagger UI: `http://localhost:5000/swagger`
   - Health Check: `http://localhost:5000/health`

### Database Initialization

The database is automatically created and seeded with sample data on first run:

- **Database File**: `CarRental.db` (created in the API project directory)
- **Sample Data**:
  - 2 users (Admin and Customer)
  - 3 sample cars (Toyota Camry, Honda CR-V, BMW 3 Series)

## 📚 API Documentation

### Base URL
```
http://localhost:5000/api
```

### Authentication Endpoints

#### Register New User
```http
POST /api/Auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890"
}
```

#### Login
```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "admin@carrental.com",
  "password": "Admin123!"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "tokenType": "Bearer",
  "expiresAt": "2024-01-27T10:00:00Z",
  "userId": "...",
  "email": "admin@carrental.com",
  "fullName": "System Admin",
  "role": "Admin"
}
```

#### Refresh Token
```http
POST /api/Auth/refresh
Content-Type: application/json

{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

### Car Management Endpoints

#### Get All Cars (with pagination)
```http
GET /api/Cars?pageNumber=1&pageSize=10
```

#### Get Car by ID
```http
GET /api/Cars/{id}
```

#### Create Car (Admin/Staff only)
```http
POST /api/Cars
Authorization: Bearer {token}
Content-Type: application/json

{
  "make": "Toyota",
  "model": "Corolla",
  "year": 2024,
  "licensePlate": "ABC-123",
  "color": "White",
  "mileage": 0,
  "dailyRate": 50.00,
  "category": 2,
  "seatingCapacity": 5,
  "fuelType": "Gasoline",
  "transmission": "Automatic",
  "hasAirConditioning": true,
  "hasGPS": true
}
```

#### Update Car
```http
PUT /api/Cars/{id}
Authorization: Bearer {token}
```

#### Delete Car
```http
DELETE /api/Cars/{id}
Authorization: Bearer {token}
```

#### Get Available Cars
```http
GET /api/Cars/available?startDate=2024-01-26&endDate=2024-01-28
```

### Rental Management Endpoints

#### Create Rental
```http
POST /api/Rentals
Authorization: Bearer {token}
Content-Type: application/json

{
  "carId": "33333333-3333-3333-3333-333333333333",
  "startDate": "2024-01-26T10:00:00",
  "endDate": "2024-01-28T10:00:00",
  "pickupLocation": "Main Office",
  "returnLocation": "Main Office"
}
```

#### Get All Rentals
```http
GET /api/Rentals?pageNumber=1&pageSize=10
```

#### Get Rental by ID
```http
GET /api/Rentals/{id}
Authorization: Bearer {token}
```

#### Update Rental Status
```http
PUT /api/Rentals/{id}/status
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": 2,
  "actualReturnDate": "2024-01-28T15:00:00",
  "odometerAtReturn": 150
}
```

#### Cancel Rental
```http
DELETE /api/Rentals/{id}
Authorization: Bearer {token}
```

### Payment Endpoints

#### Create Payment
```http
POST /api/Payments
Authorization: Bearer {token}
Content-Type: application/json

{
  "rentalId": "{rental-id}",
  "amount": 150.00,
  "paymentMethod": 1,
  "transactionId": "TXN123456"
}
```

#### Get All Payments
```http
GET /api/Payments?pageNumber=1&pageSize=10
Authorization: Bearer {token}
```

### User Management Endpoints

#### Get All Users (Admin only)
```http
GET /api/Users?pageNumber=1&pageSize=10
Authorization: Bearer {token}
```

#### Get User Profile
```http
GET /api/Users/profile
Authorization: Bearer {token}
```

#### Update User Profile
```http
PUT /api/Users/profile
Authorization: Bearer {token}
```

For complete API documentation with request/response schemas, visit: **http://localhost:5000/swagger**

## 🗄️ Database Schema

### Entities

#### User
- Personal information (Name, Email, Phone)
- Authentication (Password Hash)
- Role (Admin, Staff, Customer)
- Driver's License information
- Address details

#### Car
- Basic info (Make, Model, Year, License Plate)
- Specifications (Color, Mileage, Fuel Type, Transmission)
- Features (Air Conditioning, GPS)
- Pricing (Daily Rate)
- Status (Available, Rented, Maintenance, Out of Service)
- Category (Economy, Standard, Premium, SUV, Luxury, Sports)

#### Rental
- Car and Customer references
- Date range (Start Date, End Date, Actual Return Date)
- Pricing (Daily Rate, Total Cost, Late Fees, Damage Charges)
- Status (Pending, Active, Completed, Cancelled)
- Locations (Pickup, Return)
- Odometer readings

#### Payment
- Rental reference
- Amount and Payment Date
- Payment Method (Credit Card, Debit Card, Cash, Bank Transfer)
- Status (Pending, Completed, Failed, Refunded)
- Transaction ID

### Relationships

```
User 1──────* Rental
              │
Car  1──────* Rental 1──────* Payment
```

## 🔐 Authentication

### JWT Token Structure

The API uses **JWT (JSON Web Tokens)** for stateless authentication:

- **Token Expiration**: 24 hours
- **Issuer**: CarRentalAPI
- **Claims**: UserId, Email, Role
- **Algorithm**: HMAC SHA256

### Using Authentication

1. **Login** to receive a JWT token
2. **Include the token** in the `Authorization` header:
   ```
   Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
   ```
3. **Access protected endpoints** with the token
4. **Refresh the token** before expiration using the refresh endpoint

### Default Test Accounts

#### Admin Account
```
Email: admin@carrental.com
Password: Admin123!
Role: Admin
```

#### Customer Account
```
Email: customer@example.com
Password: Customer123!
Role: Customer
```

## 📁 Project Structure

```
CarRental-backend/
├── src/
│   ├── CarRental.API/
│   │   ├── Controllers/          # API endpoints
│   │   ├── Middleware/           # Global exception handler
│   │   ├── Filters/              # Validation filter
│   │   └── Program.cs            # Application entry point
│   │
│   ├── CarRental.Application/
│   │   ├── DTOs/                 # Data Transfer Objects
│   │   ├── Services/             # Business logic services
│   │   ├── Interfaces/           # Service interfaces
│   │   ├── Validators/           # FluentValidation rules
│   │   └── Common/               # Result patterns
│   │
│   ├── CarRental.Domain/
│   │   ├── Entities/             # Domain models
│   │   ├── Enums/                # Business enumerations
│   │   ├── Exceptions/           # Custom exceptions
│   │   └── Interfaces/           # Repository interfaces
│   │
│   └── CarRental.Infrastructure/
│       ├── Data/                 # DbContext and migrations
│       ├── Repositories/         # Data access implementations
│       └── Services/             # Infrastructure services (JWT)
│
├── CarRental.sln                 # Solution file
└── README.md                     # This file
```

## ⚙️ Configuration

### Connection String

Edit `appsettings.json` to configure the database:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=CarRental.db"
  }
}
```

**For other databases:**
- **SQL Server**: `Server=localhost;Database=CarRental;Trusted_Connection=True;`
- **PostgreSQL**: `Host=localhost;Database=CarRental;Username=user;Password=pass;`

Then update the `UseSqlite()` call in `DependencyInjection.cs` to use the appropriate provider.

### JWT Configuration

```json
{
  "Jwt": {
    "Key": "CarRentalSecretKey2024!@#$%^&*()_+",
    "Issuer": "CarRentalAPI",
    "Audience": "CarRentalClients"
  }
}
```

### CORS

CORS is configured to allow all origins for development. For production, update the policy in `Program.cs`:

```csharp
options.AddPolicy("AllowAll", policy =>
{
    policy.WithOrigins("https://yourdomain.com")
          .AllowAnyMethod()
          .AllowAnyHeader();
});
```

## 🧪 Testing the API

### Using Swagger UI

1. Navigate to `http://localhost:5000/swagger`
2. Click on an endpoint to expand it
3. Click "Try it out"
4. Fill in the request parameters
5. Click "Execute"
6. View the response

### Using Postman/Insomnia

1. **Login** to get a JWT token
2. **Save the token** from the response
3. **Set Authorization** header for protected endpoints:
   - Type: Bearer Token
   - Token: Paste the JWT token
4. **Make requests** to the API

### Sample Workflow

```bash
# 1. Login as admin
POST http://localhost:5000/api/Auth/login
{ "email": "admin@carrental.com", "password": "Admin123!" }

# 2. Get all available cars
GET http://localhost:5000/api/Cars

# 3. Create a rental (use Bearer token)
POST http://localhost:5000/api/Rentals
Authorization: Bearer {your-token}

# 4. Create a payment
POST http://localhost:5000/api/Payments
Authorization: Bearer {your-token}
```

## 📝 Logging

Logs are written to:
- **Console** - All log levels
- **File** - `logs/carrental-{Date}.log` (daily rolling)

Log levels: Information, Warning, Error

## 🔄 Next Steps

Potential enhancements:
- Add unit and integration tests
- Implement email notifications
- Add file upload for car images
- Create admin dashboard
- Add reporting and analytics
- Implement caching (Redis)
- Add rate limiting per user
- Set up CI/CD pipeline

## 📄 License

This project is licensed under the MIT License.

## 👥 Support

For questions or support, contact: support@carrental.com

---

**Built with ❤️ using .NET 8 and Clean Architecture**
