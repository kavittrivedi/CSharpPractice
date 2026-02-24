# API Versioning Sample (Media Type Versioning)

This ASP.NET Core project demonstrates media type-based API versioning.

## Setup
- .NET 8.0
- Package: Microsoft.AspNetCore.Mvc.Versioning

## Usage
Run the application:
```
dotnet run
```

Test endpoints using a tool like curl or Postman:
- V1: `GET http://localhost:5059/weatherforecast` with header `Accept: application/vnd.apiversioning.v1+json`
- V2: `GET http://localhost:5059/weatherforecast` with header `Accept: application/vnd.apiversioning.v2+json`

V2 includes an additional `Humidity` field.

## Code Changes
- Added API versioning in `Program.cs` with `MediaTypeApiVersionReader`
- Updated `WeatherForecastController.cs` with version attributes and `[Produces]` for media types
- Created `WeatherForecastV2.cs` model

## Troubleshooting
- Ensure the app builds with `dotnet build`
- Check Swagger UI at `http://localhost:5059/swagger` for versioned endpoints