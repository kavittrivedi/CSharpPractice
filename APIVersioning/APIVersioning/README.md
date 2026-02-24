# API Versioning Sample

This ASP.NET Core project demonstrates URI-based API versioning.

## Setup
- .NET 8.0
- Package: Microsoft.AspNetCore.Mvc.Versioning

## Usage
Run the application:
```
dotnet run
```

Test endpoints:
- V1: `GET http://localhost:5059/v1/weatherforecast`
- V2: `GET http://localhost:5059/v2/weatherforecast`

V2 includes an additional `Humidity` field.

## Code Changes
- Added API versioning in `Program.cs`
- Updated `WeatherForecastController.cs` with versioned routes and methods
- Created `WeatherForecastV2.cs` model

## Troubleshooting
- Ensure the app builds with `dotnet build`
- Check Swagger UI at `http://localhost:5059/swagger` for versioned endpoints