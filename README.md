A repository containing the backend project required for https://github.com/njurk/WSB-Project-Mobile-BookStore

## Description

BookStoreApi is a RESTful API built with ASP.NET Core (.NET 6) for a client mobile app - BookStore, 
where the user can browse and save books, place orders, read and post reviews. 

## Specifications

- **Framework:** .NET 6
- **Architecture:** Entity Framework Core for data access, layered DTOs for API models

## How to run

1. **Prerequisites**
   - [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
   - SQL Server or compatible database (update connection string as needed)

2. **Setup**
   - Clone the repository
   - Update `appsettings.json` with your database connection string

3. **Database Migration**
  - dotnet ef database update

4. **Run the API**
  - dotnet run --project BookStoreApi

5. **API Usage**
   - The API will be available at `https://localhost:5001` (or as configured)
   - Use tools like Postman or Swagger UI to test endpoints
