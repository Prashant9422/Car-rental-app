# Car Rental Management API
## Complete Technical Documentation

**Version:** 1.0  
**Framework:** .NET 8  
**Architecture:** Clean Architecture  
**Database:** SQLite (Production-ready for SQL Server/PostgreSQL)

---

## Table of Contents

1. [Executive Summary](#1-executive-summary)
2. [System Architecture](#2-system-architecture)
3. [Database Schema](#3-database-schema)
4. [Authentication Flow](#4-authentication-flow)
5. [API Endpoints Reference](#5-api-endpoints-reference)
6. [Business Workflows](#6-business-workflows)
7. [Deployment Guide](#7-deployment-guide)

---

## 1. Executive Summary

### 1.1 Project Overview

The **Car Rental Management API** is a comprehensive backend solution designed for car rental businesses to manage their fleet, customers, bookings, and payments efficiently. Built with .NET 8 and Clean Architecture principles, it provides a scalable, maintainable, and secure platform.

### 1.2 Key Business Goals

- **Fleet Management**: Track and manage vehicle inventory with real-time availability
- **Customer Management**: Maintain customer profiles with driver validation
- **Booking Automation**: Handle reservations with conflict detection
- **Payment Processing**: Support multiple payment methods with transaction tracking
- **Role-Based Access**: Separate interfaces for Admin, Staff, and Customers
- **Scalability**: Handle growing business needs with minimal changes

### 1.3 Technical Highlights

| Feature | Implementation |
|---------|----------------|
| Architecture | Clean Architecture (4 layers) |
| Framework | .NET 8 with ASP.NET Core |
| Database | Entity Framework Core with SQLite |
| Authentication | JWT Bearer Tokens |
| Validation | FluentValidation |
| Logging | Serilog (Console + File) |
| Documentation | Swagger/OpenAPI |
| Security | BCrypt password hashing |

---

## 2. System Architecture

### 2.1 Clean Architecture Overview

The application follows Clean Architecture principles with four distinct layers, ensuring separation of concerns and testability.

```mermaid
graph TB
    subgraph "Presentation Layer"
        A[API Controllers]
        B[Middleware]
        C[Filters]
    end
    
    subgraph "Application Layer"
        D[Services]
        E[DTOs]
        F[Validators]
        G[Interfaces]
    end
    
    subgraph "Infrastructure Layer"
        H[Repositories]
        I[DbContext]
        J[JWT Service]
    end
    
    subgraph "Domain Layer"
        K[Entities]
        L[Enums]
        M[Exceptions]
        N[Interfaces]
    end
    
    A --> D
    B --> D
    D --> H
    D --> K
    H --> I
    H --> N
    I --> K
    J --> G
    
    style A fill:#e1f5ff
    style D fill:#fff4e1
    style H fill:#ffe1f5
    style K fill:#e1ffe1
```

### 2.2 Layer Responsibilities

#### Domain Layer (CarRental.Domain)
**Purpose**: Core business entities and rules

**Components**:
- **Entities**: `Car`, `User`, `Rental`, `Payment`
- **Enums**: `UserRole`, `CarStatus`, `RentalStatus`, `PaymentMethod`, `PaymentStatus`, `CarCategory`
- **Interfaces**: `IRepository<T>`, `IUnitOfWork`, `ICarRepository`, `IUserRepository`, etc.
- **Exceptions**: Custom business exceptions

**Dependencies**: None (pure business logic)

#### Application Layer (CarRental.Application)
**Purpose**: Application business logic and use cases

**Components**:
- **Services**: `AuthService`, `CarService`, `UserService`, `RentalService`, `PaymentService`
- **Interfaces**: Service contracts
- **DTOs**: Request/Response models
- **Validators**: FluentValidation rules
- **Common**: Result patterns, pagination

**Dependencies**: Domain layer only

#### Infrastructure Layer (CarRental.Infrastructure)
**Purpose**: External systems and data persistence

**Components**:
- **DbContext**: Entity Framework Core configuration
- **Repositories**: Data access implementations
- **Services**: `JwtTokenService`
- **Data Seeding**: Initial data setup

**Dependencies**: Domain and Application layers

#### API Layer (CarRental.API)
**Purpose**: HTTP/REST endpoints and request handling

**Components**:
- **Controllers**: API endpoints for each resource
- **Middleware**: Global exception handling
- **Filters**: Validation filter
- **Configuration**: DI setup, CORS, JWT, Swagger

**Dependencies**: All layers

### 2.3 Request Processing Flow

```mermaid
sequenceDiagram
    participant Client
    participant Controller
    participant Validator
    participant Service
    participant Repository
    participant Database

    Client->>Controller: HTTP Request
    Controller->>Validator: Validate Input
    
    alt Validation Failed
        Validator-->>Client: 400 Bad Request
    else Validation Passed
        Validator->>Service: Process Request
        Service->>Repository: Query/Update Data
        Repository->>Database: SQL Query
        Database-->>Repository: Data
        Repository-->>Service: Domain Entities
        Service-->>Controller: DTO Response
        Controller-->>Client: 200 OK + JSON
    end
```

### 2.4 Dependency Injection Flow

```mermaid
graph LR
    A[Program.cs] --> B[Service Registration]
    B --> C[Infrastructure Services]
    B --> D[Application Services]
    B --> E[Framework Services]
    
    C --> C1[DbContext]
    C --> C2[Repositories]
    C --> C3[UnitOfWork]
    
    D --> D1[Auth Service]
    D --> D2[Car Service]
    D --> D3[Rental Service]
    D --> D4[Payment Service]
    
    E --> E1[JWT Auth]
    E --> E2[Swagger]
    E --> E3[CORS]
    E --> E4[Logging]
    
    style A fill:#ff6b6b
    style C fill:#4ecdc4
    style D fill:#45b7d1
    style E fill:#f9ca24
```

---

## 3. Database Schema

### 3.1 Entity Relationship Diagram

```mermaid
erDiagram
    USER ||--o{ RENTAL : places
    CAR ||--o{ RENTAL : "is rented in"
    RENTAL ||--o{ PAYMENT : "paid by"

    USER {
        guid Id PK
        string Email UK
        string PasswordHash
        string FirstName
        string LastName
        string PhoneNumber
        int Role
        bool IsActive
        datetime LastLoginAt
        string DriversLicenseNumber
        datetime DriversLicenseExpiry
        datetime DateOfBirth
    }

    CAR {
        guid Id PK
        string Make
        string Model
        int Year
        string LicensePlate UK
        string Color
        int Mileage
        decimal DailyRate
        int Status
        int Category
        string ImageUrl
        int SeatingCapacity
        string FuelType
        string Transmission
        bool HasAirConditioning
        bool HasGPS
    }

    RENTAL {
        guid Id PK
        guid CarId FK
        guid CustomerId FK
        datetime StartDate
        datetime EndDate
        datetime ActualReturnDate
        decimal DailyRate
        decimal TotalCost
        decimal LateFee
        decimal DamageCharges
        int Status
        string PickupLocation
        string ReturnLocation
        int OdometerAtPickup
        int OdometerAtReturn
    }

    PAYMENT {
        guid Id PK
        guid RentalId FK
        decimal Amount
        datetime PaymentDate
        int PaymentMethod
        int Status
        string TransactionId
        string FailureReason
    }
```

### 3.2 Entity Details

#### User Entity
```csharp
public class User : BaseEntity
{
    public string Email { get; set; }           // Unique, required
    public string PasswordHash { get; set; }    // BCrypt hashed
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }          // Admin, Staff, Customer
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
    
    // Driver information
    public string? DriversLicenseNumber { get; set; }
    public DateTime? DriversLicenseExpiry { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
    // Address
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
}
```

**Business Rules**:
- Email must be unique and valid
- Password must meet complexity requirements
- Only active users can login
- Admin can manage all resources

#### Car Entity
```csharp
public class Car : BaseEntity
{
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }    // Unique
    public string Color { get; set; }
    public int Mileage { get; set; }
    public decimal DailyRate { get; set; }
    public CarStatus Status { get; set; }       // Available, Rented, etc.
    public CarCategory Category { get; set; }   // Economy, Luxury, etc.
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public int SeatingCapacity { get; set; }
    public string FuelType { get; set; }
    public string Transmission { get; set; }
    public bool HasAirConditioning { get; set; }
    public bool HasGPS { get; set; }
}
```

**Business Rules**:
- License plate must be unique
- Cannot be deleted if has active rentals
- Status changes automatically with rental lifecycle

#### Rental Entity
```csharp
public class Rental : BaseEntity
{
    public Guid CarId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public decimal DailyRate { get; set; }
    public decimal TotalCost { get; set; }
    public decimal? LateFee { get; set; }
    public decimal? DamageCharges { get; set; }
    public RentalStatus Status { get; set; }
    public string? PickupLocation { get; set; }
    public string? ReturnLocation { get; set; }
    public int? OdometerAtPickup { get; set; }
    public int? OdometerAtReturn { get; set; }
    
    // Computed properties
    public int RentalDays => (EndDate - StartDate).Days + 1;
}
```

**Business Rules**:
- Cannot create rental for car that's already booked
- Total cost calculated as: DailyRate × RentalDays
- Late fees applied if ActualReturnDate > EndDate

#### Payment Entity
```csharp
public class Payment : BaseEntity
{
    public Guid RentalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public string? TransactionId { get; set; }
    public string? FailureReason { get; set; }
}
```

**Business Rules**:
- Payment must be linked to a rental
- Cannot delete completed payments
- Track transaction IDs for reconciliation

---

## 4. Authentication Flow

### 4.1 Registration Process

```mermaid
sequenceDiagram
    actor User
    participant API as Auth API
    participant Validator
    participant Service as Auth Service
    participant Repo as User Repository
    participant DB as Database

    User->>API: POST /api/Auth/register
    Note over User,API: Email, Password, FirstName, etc.
    
    API->>Validator: Validate Request
    alt Invalid Input
        Validator-->>User: 400 Bad Request
    else Valid Input
        Validator->>Service: RegisterAsync()
        Service->>Repo: EmailExistsAsync()
        Repo->>DB: Check Email
        
        alt Email Exists
            DB-->>User: 409 Conflict
        else Email Available
            Service->>Service: Hash Password (BCrypt)
            Service->>Repo: AddAsync(user)
            Repo->>DB: Insert User
            Service->>Service: Generate JWT Token
            Service-->>User: 200 OK + Token
        end
    end
```

### 4.2 Login Process

```mermaid
sequenceDiagram
    actor User
    participant API as Auth API
    participant Service as Auth Service
    participant Repo as User Repository
    participant JWT as JWT Service
    participant DB as Database

    User->>API: POST /api/Auth/login
    Note over User,API: Email + Password
    
    API->>Service: LoginAsync()
    Service->>Repo: GetByEmailAsync()
    Repo->>DB: SELECT by Email
    
    alt User Not Found
        DB-->>User: 401 Unauthorized
    else User Found
        DB-->>Service: User Entity
        Service->>Service: Verify Password (BCrypt)
        
        alt Invalid Password
            Service-->>User: 401 Unauthorized
        else Valid Password
            alt User Inactive
                Service-->>User: 401 Unauthorized
            else User Active
                Service->>Repo: Update LastLoginAt
                Service->>JWT: GenerateToken(user)
                JWT-->>Service: JWT Token
                Service-->>User: 200 OK + Token + UserInfo
            end
        end
    end
```

### 4.3 JWT Token Structure

```json
{
  "header": {
    "alg": "HS256",
    "typ": "JWT"
  },
  "payload": {
    "sub": "user-id-guid",
    "email": "user@example.com",
    "role": "Customer",
    "exp": "1737964800",
    "iss": "CarRentalAPI",
    "aud": "CarRentalClients"
  }
}
```

### 4.4 Protected Endpoint Access

```mermaid
sequenceDiagram
    actor User
    participant API
    participant Auth as Auth Middleware
    participant JWT as JWT Service
    participant Controller
    participant Service

    User->>API: Request with Bearer Token
    API->>Auth: Extract Token
    Auth->>JWT: ValidateToken()
    
    alt Invalid/Expired Token
        JWT-->>User: 401 Unauthorized
    else Valid Token
        JWT-->>Auth: Claims (UserId, Role)
        Auth->>Controller: Request + User Context
        
        alt Insufficient Permissions
            Controller-->>User: 403 Forbidden
        else Authorized
            Controller->>Service: Execute Business Logic
            Service-->>User: 200 OK + Response
        end
    end
```

---

## 5. API Endpoints Reference

### 5.1 Authentication Endpoints

#### Register New User
**Endpoint**: `POST /api/Auth/register`  
**Authentication**: None  
**Request Body**:
```json
{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890"
}
```

**Response (200 OK)**:
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "tokenType": "Bearer",
    "expiresAt": "2024-01-27T10:00:00Z",
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "email": "user@example.com",
    "fullName": "John Doe",
    "role": "Customer"
  }
}
```

**Validation Rules**:
- Email: Required, valid format, unique
- Password: Minimum 8 characters, at least one uppercase, one lowercase, one digit
- FirstName/LastName: Required, 2-50 characters
- PhoneNumber: Required, valid format

---

#### Login
**Endpoint**: `POST /api/Auth/login`  
**Authentication**: None  
**Request Body**:
```json
{
  "email": "admin@carrental.com",
  "password": "Admin123!"
}
```

**Response (200 OK)**:
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "tokenType": "Bearer",
    "expiresAt": "2024-01-27T10:00:00Z",
    "userId": "11111111-1111-1111-1111-111111111111",
    "email": "admin@carrental.com",
    "fullName": "System Admin",
    "role": "Admin"
  }
}
```

**Error Responses**:
- `401 Unauthorized`: Invalid credentials or inactive account

---

#### Refresh Token
**Endpoint**: `POST /api/Auth/refresh`  
**Authentication**: Required (expired token accepted)  
**Request Body**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response (200 OK)**: New token with extended expiration

---

### 5.2 Car Management Endpoints

#### Get All Cars
**Endpoint**: `GET /api/Cars?pageNumber=1&pageSize=10`  
**Authentication**: None  
**Query Parameters**:
- `pageNumber` (optional): Page number (default: 1)
- `pageSize` (optional): Items per page (default: 10, max: 100)

**Response (200 OK)**:
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "33333333-3333-3333-3333-333333333333",
        "make": "Toyota",
        "model": "Camry",
        "year": 2024,
        "licensePlate": "ABC-1234",
        "color": "Silver",
        "dailyRate": 45.00,
        "status": "Available",
        "category": "Standard",
        "seatingCapacity": 5,
        "transmission": "Automatic",
        "hasAirConditioning": true
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 1,
    "totalCount": 3
  }
}
```

---

#### Get Available Cars
**Endpoint**: `GET /api/Cars/available?startDate=2024-01-26T10:00&endDate=2024-01-28T10:00`  
**Authentication**: None  
**Query Parameters**:
- `startDate`: Rental start date (ISO 8601)
- `endDate`: Rental end date (ISO 8601)

**Response**: List of cars not booked for the specified period

---

#### Create Car
**Endpoint**: `POST /api/Cars`  
**Authentication**: Required (Admin/Staff only)  
**Request Body**:
```json
{
  "make": "Honda",
  "model": "Civic",
  "year": 2024,
  "licensePlate": "XYZ-789",
  "color": "Blue",
  "mileage": 0,
  "dailyRate": 40.00,
  "category": 2,
  "description": "Fuel-efficient compact car",
  "seatingCapacity": 5,
  "fuelType": "Gasoline",
  "transmission": "Automatic",
  "hasAirConditioning": true,
  "hasGPS": true
}
```

**Validation Rules**:
- LicensePlate: Unique, required
- DailyRate: Greater than 0
- Year: Between 1900 and current year + 1
- Category: Valid enum value (0-5)

---

#### Update Car
**Endpoint**: `PUT /api/Cars/{id}`  
**Authentication**: Required (Admin/Staff only)  
**Request Body**: Same as Create Car

---

#### Delete Car
**Endpoint**: `DELETE /api/Cars/{id}`  
**Authentication**: Required (Admin only)  
**Response**: 204 No Content (success) or 400 if car has active rentals

---

### 5.3 Rental Management Endpoints

#### Create Rental
**Endpoint**: `POST /api/Rentals`  
**Authentication**: Required  
**Request Body**:
```json
{
  "carId": "33333333-3333-3333-3333-333333333333",
  "startDate": "2024-01-26T10:00:00",
  "endDate": "2024-01-28T10:00:00",
  "pickupLocation": "Main Office - Downtown",
  "returnLocation": "Main Office - Downtown"
}
```

**Response (201 Created)**:
```json
{
  "success": true,
  "data": {
    "id": "rental-id-guid",
    "carId": "33333333-3333-3333-3333-333333333333",
    "customerId": "customer-id",
    "startDate": "2024-01-26T10:00:00",
    "endDate": "2024-01-28T10:00:00",
    "dailyRate": 45.00,
    "totalCost": 135.00,
    "status": "Pending",
    "pickupLocation": "Main Office - Downtown",
    "returnLocation": "Main Office - Downtown"
  }
}
```

**Business Logic**:
1. Check car availability for date range
2. Calculate total cost: DailyRate × (EndDate - StartDate).Days
3. Set status to "Pending"
4. Update car status to "Rented" (on confirmation)

---

#### Get Rental by ID
**Endpoint**: `GET /api/Rentals/{id}`  
**Authentication**: Required  
**Authorization**: Customers can only view their own rentals

---

#### Update Rental Status
**Endpoint**: `PUT /api/Rentals/{id}/status`  
**Authentication**: Required (Staff/Admin)  
**Request Body**:
```json
{
  "status": 2,
  "actualReturnDate": "2024-01-28T15:00:00",
  "odometerAtReturn": 12550,
  "damageCharges": 0,
  "notes": "Car returned in good condition"
}
```

**Status Codes**:
- 0: Pending
- 1: Active
- 2: Completed
- 3: Cancelled

---

#### Cancel Rental
**Endpoint**: `DELETE /api/Rentals/{id}`  
**Authentication**: Required  
**Business Logic**:
- Customers can only cancel "Pending" rentals
- Staff/Admin can cancel "Pending" or "Active" rentals
- Cannot cancel "Completed" rentals
- Car status reverts to "Available"

---

### 5.4 Payment Endpoints

#### Create Payment
**Endpoint**: `POST /api/Payments`  
**Authentication**: Required  
**Request Body**:
```json
{
  "rentalId": "rental-id-guid",
  "amount": 135.00,
  "paymentMethod": 1,
  "transactionId": "TXN-20240126-001"
}
```

**Payment Methods**:
- 0: CreditCard
- 1: DebitCard
- 2: Cash
- 3: BankTransfer

**Response (201 Created)**:
```json
{
  "success": true,
  "data": {
    "id": "payment-id-guid",
    "rentalId": "rental-id-guid",
    "amount": 135.00,
    "paymentDate": "2024-01-26T10:30:00",
    "paymentMethod": "DebitCard",
    "status": "Completed",
    "transactionId": "TXN-20240126-001"
  }
}
```

---

### 5.5 User Management Endpoints

#### Get User Profile
**Endpoint**: `GET /api/Users/profile`  
**Authentication**: Required  
**Response**: Current user's profile information

---

#### Update User Profile
**Endpoint**: `PUT /api/Users/profile`  
**Authentication**: Required  
**Request Body**:
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890",
  "street": "123 Main St",
  "city": "New York",
  "state": "NY",
  "postalCode": "10001",
  "country": "USA",
  "driversLicenseNumber": "D1234567",
  "driversLicenseExpiry": "2028-12-31",
  "dateOfBirth": "1990-05-15"
}
```

---

## 6. Business Workflows

### 6.1 Complete Rental Workflow

```mermaid
stateDiagram-v2
    [*] --> SearchCars: Customer browses
    SearchCars --> SelectDates: Choose dates
    SelectDates --> CheckAvailability: Query API
    
    CheckAvailability --> Login: Cars available
    CheckAvailability --> SearchCars: No cars available
    
    Login --> CreateRental: Authenticated
    CreateRental --> Pending: Rental created
    
    Pending --> Active: Staff confirms pickup
    Pending --> Cancelled: Customer cancels
    
    Active --> ProcessReturn: Customer returns car
    ProcessReturn --> CalculateFees: Check for late fees/damages
    CalculateFees --> CreatePayment: Calculate final amount
    CreatePayment --> Completed: Payment successful
    
    Completed --> [*]
    Cancelled --> [*]
```

### 6.2 Payment Processing Flow

```mermaid
flowchart TD
    A[Rental Completed] --> B{Payment Due?}
    B -->|Yes| C[Customer Initiates Payment]
    B -->|No| Z[End]
    
    C --> D[Select Payment Method]
    D --> E{Payment Method}
    
    E -->|Credit/Debit Card| F[Process Card Payment]
    E -->|Cash| G[Update Payment Status]
    E -->|Bank Transfer| H[Record Transaction ID]
    
    F --> I{Card Authorized?}
    I -->|Yes| J[Payment Completed]
    I -->|No| K[Payment Failed]
    
    G --> J
    H --> J
    
    J --> L[Update Rental Status]
    L --> M[Send Confirmation]
    M --> Z
    
    K --> N[Log Failure Reason]
    N --> O[Notify Customer]
    O --> C
```

### 6.3 Car Availability Check

```mermaid
flowchart TD
    A[User Requests Availability] --> B[Input: Start & End Dates]
    B --> C[Query All Cars]
    
    C --> D{For Each Car}
    D --> E[Check Rental Records]
    
    E --> F{Has Conflicting Rentals?}
    F -->|Yes| G[Exclude Car]
    F -->|No| H{Car Status?}
    
    H -->|Available| I[Include in Results]
    H -->|Maintenance/OutOfService| G
    
    G --> D
    I --> D
    
    D -->|All Cars Checked| J[Return Available Cars]
    J --> K[Display to User]
```

### 6.4 Late Fee Calculation

```mermaid
flowchart TD
    A[Car Returned] --> B{Actual Return > End Date?}
    B -->|No| C[No Late Fee]
    B -->|Yes| D[Calculate Late Days]
    
    D --> E[Late Days = Actual Return - End Date]
    E --> F[Late Fee = Late Days × Daily Rate × 1.5]
    F --> G[Add to Total Cost]
    
    C --> H[Final Amount = Total Cost]
    G --> I[Final Amount = Total + Late Fee + Damages]
    
    H --> J[Generate Invoice]
    I --> J
    J --> K[Request Payment]
```

---

## 7. Deployment Guide

### 7.1 Local Development Setup

```bash
# 1. Clone the repository
cd /Users/prashantkumar/Desktop/CarRental-backend

# 2. Restore dependencies
dotnet restore

# 3. Build the solution
dotnet build

# 4. Run the API
dotnet run --project src/CarRental.API/CarRental.API.csproj

# 5. Access Swagger
# Open browser to: http://localhost:5000/swagger
```

### 7.2 Production Deployment

#### Configuration for Production

**appsettings.Production.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=CarRental;User Id=sa;Password=****;"
  },
  "Jwt": {
    "Key": "*** SECURE KEY FROM ENVIRONMENT ***",
    "Issuer": "CarRentalAPI",
    "Audience": "CarRentalClients"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

#### Docker Deployment

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "CarRental.API.dll"]
```

### 7.3 Environment Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Environment name | Production |
| `JWT_KEY` | JWT signing key | 256-bit secret |
| `DB_CONNECTION` | Database connection | ConnectionString |
| `CORS_ORIGINS` | Allowed CORS origins | https://app.com |

---

## Appendix A: Error Codes

| HTTP Code | Error Type | Description |
|-----------|------------|-------------|
| 400 | Bad Request | Validation failed |
| 401 | Unauthorized | Invalid or missing token |
| 403 | Forbidden | Insufficient permissions |
| 404 | Not Found | Resource doesn't exist |
| 409 | Conflict | Duplicate resource |
| 500 | Server Error | Internal error |

---

## Appendix B: Default Test Accounts

### Admin Account
- **Email**: admin@carrental.com
- **Password**: Admin123!
- **Permissions**: Full access

### Customer Account
- **Email**: customer@example.com
- **Password**: Customer123!
- **Permissions**: View cars, create rentals, manage own profile

---

**Document Version**: 1.0  
**Last Updated**: January 26, 2024  
**Contact**: support@carrental.com
