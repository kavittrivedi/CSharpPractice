# C# Concepts Used In This Project

This file explains the main C# concepts used in the Kuber Finance project in simple language. The project is an ASP.NET Core application with separate Web, API, Core, Infrastructure, and Test layers.

## 1. Namespaces

A namespace is used to group related classes and avoid name conflicts.

Example:

```csharp
namespace KuberFinanceApp.Core.Services;
```

This tells us that classes like `AuthService` and `FleetImportService` belong to the Core services area of the application.

## 2. Classes

A class is a blueprint for creating objects. It contains data, called properties or fields, and behavior, called methods.

Example:

```csharp
public class FleetImportService : IFleetImportService
```

`FleetImportService` contains the logic for importing fleet data from Excel files.

## 3. Objects

An object is an actual instance of a class.

Example:

```csharp
var response = new AuthTokenResponse
{
    Token = authResult.AccessToken,
    TokenType = "Bearer"
};
```

Here, a new `AuthTokenResponse` object is created and filled with values.

## 4. Properties

Properties hold data inside a class. They usually have `get` and `set` so values can be read and changed.

Example:

```csharp
public string Email { get; set; } = string.Empty;
```

This means the object has an `Email` value. The default value is an empty string.

## 5. Fields

Fields are variables stored inside a class. They are often private and used internally by the class.

Example:

```csharp
private readonly IUnitOfWork _unitOfWork;
```

This field stores the unit-of-work dependency used by a service.

## 6. Access Modifiers

Access modifiers control where code can be used from.

- `public`: can be used from anywhere.
- `private`: can be used only inside the same class.
- `protected`: can be used inside the same class and child classes.
- `internal`: can be used inside the same project/assembly.

Example:

```csharp
private static string HashToken(string token)
```

`HashToken` is private because it is only needed inside `AuthService`.

## 7. Interfaces

An interface defines what a class must do, without saying how it should do it.

Example:

```csharp
public interface IAuthService
```

Classes such as `AuthService` implement interfaces. This makes the code easier to test and replace.

## 8. Interface Implementation

When a class implements an interface, it promises to provide the methods from that interface.

Example:

```csharp
public sealed class AuthService : IAuthService
```

`AuthService` must provide methods like `LoginAsync`, `RefreshAsync`, and `RevokeAsync`.

## 9. Inheritance

Inheritance allows one class to reuse or extend another class.

Example:

```csharp
public abstract class AppException : Exception
```

`AppException` inherits from the built-in `Exception` class. Other custom exceptions, such as `BadRequestException`, inherit from `AppException`.

## 10. Abstract Classes

An abstract class is a base class that cannot be created directly. It is meant to be inherited by other classes.

Example:

```csharp
public abstract class AppException : Exception
```

The project uses this as a common base for custom application exceptions.

## 11. Sealed Classes

A sealed class cannot be inherited by another class.

Example:

```csharp
public sealed class AuthService : IAuthService
```

This means no other class can extend `AuthService`.

## 12. Static Classes And Static Methods

`static` means something belongs to the class itself, not to an object created from the class.

Example:

```csharp
public static class HttpClientExtensions
```

The project uses static classes for helper methods and extension methods.

## 13. Extension Methods

An extension method adds a method-like feature to an existing type without changing that type.

Example:

```csharp
public static IHttpClientBuilder ConfigureApiClient(
    this IHttpClientBuilder builder,
    string baseUrl,
    int timeoutSeconds = 30)
```

The `this IHttpClientBuilder builder` part makes this an extension method. It lets the project call `ConfigureApiClient` as if it were built into `IHttpClientBuilder`.

## 14. Constructors

A constructor runs when an object is created. It is commonly used to provide dependencies or initial values.

Example:

```csharp
public AuthService(
    IUserService userService,
    IJwtTokenService jwtTokenService,
    IUnitOfWork unitOfWork,
    IOptions<JwtSettings> settings)
```

This constructor receives the services that `AuthService` needs.

## 15. Dependency Injection

Dependency injection means classes receive the services they need instead of creating them directly.

Example:

```csharp
services.AddScoped<IAuthService, AuthService>();
```

This tells ASP.NET Core: whenever something asks for `IAuthService`, create and provide an `AuthService`.

## 16. Service Lifetimes

The project uses `AddScoped`, which means one service instance is created per web request.

Example:

```csharp
services.AddScoped<IUnitOfWork, UnitOfWork>();
```

This is useful for database-related classes because a request usually works with one database context.

## 17. Methods

A method is a block of code that performs an action.

Example:

```csharp
public async Task<AuthResult?> LoginAsync(string email, string password)
```

This method tries to log in a user and returns an authentication result.

## 18. Async And Await

`async` and `await` are used for operations that take time, such as database calls, file uploads, and API calls.

Example:

```csharp
var user = await _userService.LoginAsync(email, password);
```

The code waits for the login operation without blocking the whole application thread.

## 19. Task And Task<T>

`Task` represents work that will finish in the future. `Task<T>` means the work will return a value of type `T`.

Example:

```csharp
Task<AuthResult?> LoginAsync(string email, string password)
```

This means the method is asynchronous and may return an `AuthResult`.

## 20. Nullable Reference Types

The `?` after a reference type means the value is allowed to be `null`.

Example:

```csharp
public string? PhoneNumber { get; set; }
```

This means `PhoneNumber` is optional.

Another example:

```csharp
public async Task<AuthResult?> RefreshAsync(string refreshToken)
```

This method may return an authentication result or `null`.

## 21. Null Checks

The project checks for null values before using them.

Example:

```csharp
if (authResult == null)
    return Unauthorized(ApiResponse<object>.Fail("Invalid credentials"));
```

This prevents errors when expected data is missing.

## 22. Null-Coalescing Operator

The `??` operator gives a fallback value when the left side is `null`.

Example:

```csharp
model.ErrorMessage = result.Message ?? "Unable to load users right now.";
```

If `result.Message` is null, the default message is used.

## 23. Null-Conditional Operator

The `?.` operator safely accesses a member only if the object is not null.

Example:

```csharp
var mustChangePassword = context.User.FindFirst("must_change_password")?.Value;
```

If the claim is missing, this returns null instead of throwing an error.

## 24. Null-Forgiving Operator

The `!` operator tells the compiler: "I know this might look null, but trust me."

Example from tests:

```csharp
public IUserRepository Users => null!;
```

This is used in fake test classes where a member is not needed for that test.

## 25. Object Initializers

Object initializers create an object and set its properties in one block.

Example:

```csharp
var model = new AdminUsersViewModel
{
    Page = page,
    PageSize = pageSize,
    Role = role,
    Search = search
};
```

This is cleaner than creating the object first and setting each property separately.

## 26. Collection Initializers

Collection initializers create collections with values.

Example:

```csharp
private static readonly Dictionary<string, string> HeaderAliases = new()
{
    ["srno"] = "SrNo",
    ["regtno"] = "RegistrationNumber"
};
```

This creates a dictionary of Excel header aliases.

## 27. Collection Expressions

The project uses modern C# collection expressions such as `[]`.

Example:

```csharp
private static readonly string[] RequiredHeaders =
[
    "RegistrationNumber",
    "RegistrationOwner"
];
```

This creates an array in a short, readable way.

## 28. Generics

Generics let a class or method work with different types while still being type-safe.

Example:

```csharp
public class ApiResponse<T>
```

`ApiResponse<UserResponse>` and `ApiResponse<AuthTokenResponse>` can use the same response structure with different data types.

## 29. Generic Collections

The project uses generic collections such as `List<T>`, `Dictionary<TKey, TValue>`, `IEnumerable<T>`, `HashSet<T>`, and `IReadOnlyDictionary<TKey, TValue>`.

Example:

```csharp
public IEnumerable<ClientModel> AssignedClients { get; set; } = Enumerable.Empty<ClientModel>();
```

This means the property contains many `ClientModel` objects.

## 30. Tuples

Tuples allow a method to return multiple values together.

Example:

```csharp
Task<(IEnumerable<UserModel> Users, int TotalCount)> GetAllUsersAsync(...)
```

This returns both the list of users and the total count.

## 31. Enums

An enum is a fixed list of named values.

Example:

```csharp
public enum UserRole
```

The project uses roles such as Admin and Client to control access and behavior.

## 32. Records

A record is a type designed mainly for holding data. It gives built-in value-style behavior.

Example:

```csharp
private sealed record RowValidationError(string ColumnName, string ErrorMessage, string? RawValue);
```

This is used to store row-level validation errors during fleet import.

## 33. Nested Classes

A nested class is a class declared inside another class.

Example:

```csharp
private sealed class FleetImportRow
```

`FleetImportRow` is inside `FleetImportService` because it is only used by that service.

## 34. Constants

`const` values do not change after compilation.

Example:

```csharp
private const int DefaultPageSize = 10;
```

This gives one named place for the default page size.

## 35. Readonly Fields

`readonly` fields can be assigned only when declared or inside the constructor.

Example:

```csharp
private readonly IUserService _userService;
```

This protects dependencies from being accidentally replaced later.

## 36. Pattern Matching

Pattern matching checks whether a value matches a certain shape or condition.

Example:

```csharp
pageSize = pageSize is 10 or 20 or 50 ? pageSize : DefaultPageSize;
```

This allows only page sizes 10, 20, or 50.

## 37. Switch Expressions

Switch expressions choose a value based on a condition in a compact way.

Example:

```csharp
var statusCode = exception switch
{
    BadRequestException => StatusCodes.Status400BadRequest,
    NotFoundException => StatusCodes.Status404NotFound,
    ConflictException => StatusCodes.Status409Conflict,
    _ => StatusCodes.Status500InternalServerError
};
```

This maps different exception types to different HTTP status codes.

## 38. Ternary Operator

The ternary operator is a short `if/else` expression.

Example:

```csharp
batch.Status = batch.FailedRows == 0
    ? "Completed"
    : batch.SuccessRows == 0
        ? "Failed"
        : "CompletedWithErrors";
```

This picks the import status based on success and failure counts.

## 39. If Statements

`if` statements run code only when a condition is true.

Example:

```csharp
if (file == null || file.Length == 0)
{
    ModelState.AddModelError(string.Empty, "Please select a non-empty .xlsx fleet file.");
}
```

This validates uploaded files before processing them.

## 40. Loops

Loops repeat work.

Example:

```csharp
foreach (var row in rows)
```

This processes each row from an uploaded Excel file.

Another example:

```csharp
for (var column = 1; column <= lastCell; column++)
```

This reads each column from the Excel header row.

## 41. Yield Return

`yield return` produces values one at a time from a method.

Example:

```csharp
yield return new FleetImportRow { ... };
```

This is used while reading rows from an Excel worksheet.

## 42. LINQ

LINQ is used to query and transform collections.

Examples:

```csharp
users.Select(u => _mapper.Map<UserModel>(u))
```

```csharp
vehicles.Count(v => v.IsFinanced)
```

```csharp
batch.Errors.OrderBy(e => e.RowNumber).ThenBy(e => e.ColumnName)
```

LINQ makes collection filtering, sorting, grouping, and mapping easier to read.

## 43. Lambda Expressions

A lambda expression is a short function written inline.

Example:

```csharp
client => client.ClientId
```

The project uses lambdas heavily in LINQ, dependency injection configuration, logging, and authentication events.

## 44. Delegates And Callbacks

Some framework methods accept small pieces of code to run later.

Example:

```csharp
options.Events = new JwtBearerEvents
{
    OnTokenValidated = context =>
    {
        return Task.CompletedTask;
    }
};
```

The JWT middleware calls this code when a token is validated.

## 45. Attributes

Attributes add metadata to classes, methods, or properties.

Example:

```csharp
[ApiController]
[Route("api/[controller]")]
```

These tell ASP.NET Core that a class is an API controller and define its route.

## 46. MVC Action Attributes

The project uses attributes to map controller methods to HTTP actions.

Examples:

```csharp
[HttpGet]
[HttpPost]
[HttpPut("{id:int}")]
[HttpDelete("{id:int}")]
```

These connect C# methods to web API endpoints.

## 47. Authorization Attributes

Authorization attributes protect pages and API endpoints.

Example:

```csharp
[Authorize(Roles = "Admin")]
```

Only users with the Admin role can access that action or controller.

## 48. Model Binding Attributes

Model binding attributes tell ASP.NET Core where values should come from.

Examples:

```csharp
[FromBody] LoginRequest request
[FromQuery] int page = 1
```

`FromBody` reads JSON from the request body. `FromQuery` reads values from the URL query string.

## 49. Data Annotation Attributes

Data annotations define validation rules for models.

Examples:

```csharp
[Required]
[StringLength(80)]
[EmailAddress]
[Compare(nameof(Password))]
```

These are used in view models such as `AdminCreateUserViewModel` to validate form input.

## 50. Exceptions

Exceptions represent errors.

Example:

```csharp
throw new BadRequestException("Only .xlsx fleet import files are supported.");
```

The project throws custom exceptions when business rules are violated.

## 51. Custom Exceptions

Custom exceptions make application errors more meaningful.

Examples:

```csharp
BadRequestException
NotFoundException
ConflictException
```

These help the API return the correct HTTP response.

## 52. Try, Catch, Finally

`try/catch/finally` handles errors and cleanup.

Example:

```csharp
try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Kuber Finance API terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
```

This logs startup failures and closes the logger properly.

## 53. Rethrowing Exceptions

The project sometimes catches an error, does cleanup, and throws the same error again.

Example:

```csharp
catch
{
    await _unitOfWork.RollbackTransactionAsync(transaction);
    throw;
}
```

This keeps the original error but rolls back the database transaction first.

## 54. Using Statements

`using` imports namespaces so classes can be referenced without their full names.

Example:

```csharp
using Microsoft.EntityFrameworkCore;
```

This allows the code to use EF Core types such as `DbContext` and `DbSet`.

## 55. Using Var

`using var` automatically disposes an object when the method or scope ends.

Example:

```csharp
using var workbook = new XLWorkbook(fileStream);
```

This closes and cleans up the workbook after import processing.

## 56. Await Using Var

`await using var` is used for objects that need asynchronous cleanup.

Example:

```csharp
await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
```

This is used for database transactions.

## 57. CancellationToken

`CancellationToken` allows long-running async work to be cancelled.

Example:

```csharp
Task<FleetDashboardModel> GetDashboardForUserAsync(
    int userId,
    CancellationToken cancellationToken = default)
```

This lets web requests stop database work if the request is cancelled.

## 58. ASP.NET Core Controllers

Controllers receive HTTP requests and return HTTP responses.

Example:

```csharp
public class AuthController : ControllerBase
```

`AuthController` handles login, refresh token, and revoke token API requests.

## 59. MVC Controllers And Views

The Web project uses MVC controllers that return views.

Example:

```csharp
public class AdminController : Controller
```

Methods like `Users`, `AddUser`, and `UploadFleet` return Razor views for the web UI.

## 60. IActionResult

`IActionResult` represents different possible HTTP responses.

Examples:

```csharp
return Ok(...);
return Unauthorized(...);
return NoContent();
return View(model);
return RedirectToAction(nameof(Users));
```

This lets one action return different response types depending on the situation.

## 61. View Models

View models are classes designed for screens or forms.

Example:

```csharp
public class AdminCreateUserViewModel
```

This model contains the fields needed by the Add User screen.

## 62. Request And Response DTOs

DTO means Data Transfer Object. These classes define the shape of API input and output.

Examples:

```csharp
LoginRequest
RefreshTokenRequest
AuthTokenResponse
UserResponse
```

They keep API models separate from database entities.

## 63. Entity Classes

Entity classes represent database tables.

Examples:

```csharp
User
Client
Vehicle
VehicleLoan
RefreshToken
```

Entity Framework Core uses these classes to read and write database rows.

## 64. DbContext

`DbContext` is the main EF Core class for database access.

Example:

```csharp
public class AppDbContext : DbContext, IAppDbContext
```

It contains `DbSet` properties for tables such as users, clients, vehicles, and refresh tokens.

## 65. DbSet

`DbSet<T>` represents a database table.

Example:

```csharp
public DbSet<User> Users { get; set; }
```

This lets the code query and save `User` records.

## 66. EF Core Fluent API

The Fluent API configures database tables and relationships in C# code.

Example:

```csharp
builder.Property(v => v.RegistrationNumber).IsRequired().HasMaxLength(30);
```

This says the vehicle registration number is required and has a maximum length.

## 67. Entity Relationships

The project defines relationships between entities.

Example:

```csharp
builder.HasOne(v => v.Client)
    .WithMany(c => c.Vehicles)
    .HasForeignKey(v => v.ClientId);
```

This means one client can have many vehicles.

## 68. EF Core Migrations

Migrations are C# files that describe database schema changes.

Example files:

```text
20260605121756_InitialCreate.cs
20260614103156_AddRefreshTokens.cs
```

They let the database structure evolve with the code.

## 69. Repository Pattern

Repositories group database operations for a specific area.

Examples:

```csharp
UserRepository
FleetRepository
RefreshTokenRepository
```

Instead of putting database queries everywhere, the project keeps them in repository classes.

## 70. Unit Of Work Pattern

Unit of Work groups repositories and saves changes together.

Example:

```csharp
public interface IUnitOfWork
```

The project uses it to access repositories and call `SaveChangesAsync`.

## 71. Transactions

A transaction makes several database operations succeed or fail together.

Example:

```csharp
await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
```

Fleet import uses a transaction so partial imports can be rolled back if something fails.

## 72. AutoMapper

AutoMapper copies data between similar objects.

Example:

```csharp
CreateMap<User, UserModel>();
```

The project uses it to map database entities to core models and API responses.

## 73. Options Pattern

The options pattern reads configuration into strongly typed classes.

Example:

```csharp
IOptions<JwtSettings> settings
```

`AuthService` and `JwtTokenService` use this to access JWT settings.

## 74. Logging

Logging records useful information about what the application is doing.

Example:

```csharp
_logger.LogWarning("Failed to load admin users page. Status: {StatusCode}", result.StatusCode);
```

The project uses `ILogger<T>` and Serilog for logging.

## 75. Middleware

Middleware is code that runs during an HTTP request pipeline.

Example:

```csharp
app.Use(async (context, next) =>
{
    await next();
});
```

The API uses middleware to block users who must change their temporary password.

## 76. Authentication And Authorization

Authentication checks who the user is. Authorization checks what the user is allowed to do.

Examples:

```csharp
builder.Services.AddAuthentication(...)
builder.Services.AddAuthorization()
[Authorize(Roles = "Admin")]
```

The project uses JWT authentication in the API and cookie authentication in the Web project.

## 77. JWT Tokens

JWT tokens are signed strings used to prove a user is logged in.

Example:

```csharp
_jwtTokenService.GenerateToken(user)
```

The project creates access tokens and refresh tokens during login.

## 78. Cryptography APIs

The project uses cryptography to hash passwords and refresh tokens.

Examples:

```csharp
SHA256.HashData(...)
HMACSHA512
RandomNumberGenerator.GetBytes(64)
```

Hashing protects sensitive values so raw passwords or refresh tokens are not stored directly.

## 79. String Interpolation

String interpolation inserts values into strings using `$`.

Example:

```csharp
$"Repayment schedule imported for loan {result.Data.LoanAccountNumber}."
```

This is easier to read than string concatenation.

## 80. nameof Operator

`nameof` returns the name of a variable, property, method, or class as a string.

Example:

```csharp
return RedirectToAction(nameof(Clients));
```

This is safer than typing `"Clients"` manually because renaming tools can update it.

## 81. String Comparison

The project uses explicit string comparisons for safer text matching.

Example:

```csharp
StringComparison.OrdinalIgnoreCase
```

This compares strings without caring about uppercase/lowercase differences.

## 82. Date And Time

The project uses `DateTime`, `TimeSpan`, and date parsing for tokens, imports, EMI dates, and policies.

Examples:

```csharp
DateTime.UtcNow
TimeSpan.FromDays(...)
DateTime.TryParseExact(...)
```

This helps handle expiry dates and uploaded spreadsheet dates.

## 83. CultureInfo

`CultureInfo` tells C# how to parse and format values for a specific culture.

Example:

```csharp
private static readonly CultureInfo IndianCulture = new("en-IN");
```

This is useful when parsing Indian date and number formats from Excel/PDF files.

## 84. Regular Expressions

Regular expressions search for text patterns.

Example:

```csharp
private static readonly Regex InterestRateRegex = new(...)
```

The repayment schedule parsers use regex to extract values from PDF text.

## 85. File And Stream Handling

The project handles uploaded files using streams.

Example:

```csharp
await using var stream = file.OpenReadStream();
```

Streams let the app read uploaded Excel and PDF files without loading everything as normal text first.

## 86. HTTP Client Usage

The Web project calls the API using HTTP client services.

Examples:

```csharp
ApiClient
UserApiClient
FleetImportApiClient
AdminFleetApiClient
```

These classes keep API-calling code separate from controllers.

## 87. JSON Serialization

The project sends and receives JSON between the Web and API layers.

Example:

```csharp
WriteAsJsonAsync(...)
```

API responses such as `ApiResponse<T>` are returned as JSON.

## 88. Partial Classes

Partial classes allow one class to be split across multiple files.

Example:

```csharp
public partial class InitialCreate : Migration
```

EF Core migration files use partial classes.

## 89. Method Overloading

Method overloading means having methods with the same name but different parameters.

Example:

```csharp
GetClientsAsync()
GetClientsAsync(int page, int pageSize, string? search, ...)
```

The repository can provide both "get all clients" and "get paged clients" behavior using the same method name.

## 90. Default Parameter Values

Parameters can have default values.

Example:

```csharp
public async Task<IActionResult> Users(int page = 1, int pageSize = DefaultPageSize, string? role = null)
```

If the caller does not provide values, the defaults are used.

## 91. params Parameters

`params` lets a method accept any number of arguments as an array.

Example from tests:

```csharp
protected static void SetUserClaims(ControllerBase controller, string userId, params string[] roles)
```

The test can pass zero, one, or many roles.

## 92. Unit Tests

Unit tests check small pieces of code in isolation.

Example:

```csharp
[Fact]
public async Task LoginAsync_IssuesAndPersistsRefreshToken()
```

The project uses xUnit tests for services and controllers.

## 93. Test Attributes

xUnit uses attributes to mark tests.

Examples:

```csharp
[Fact]
[Theory]
[InlineData("invalid-user-id")]
```

`Fact` is a normal test. `Theory` runs the same test with different input values.

## 94. Mocking And Fake Classes

Tests use fake classes and mocks to replace real dependencies.

Examples:

```csharp
private sealed class FakeJwtTokenService : IJwtTokenService
```

```csharp
Mock<IFormFile>
```

This lets tests focus on one class without needing a real database, API, or uploaded file.

## 95. Assertions

Assertions check expected test results.

Examples:

```csharp
Assert.NotNull(result);
Assert.Equal(2, fixture.RefreshTokens.Tokens.Count);
```

If an assertion fails, the test fails.

## 96. FluentAssertions

Some tests use FluentAssertions for readable assertions.

Example:

```csharp
createdResult.RouteValues["id"].Should().Be(1);
```

This reads almost like English.

## 97. Configuration

The project reads settings from configuration files and environment-specific sources.

Example:

```csharp
builder.Configuration.GetSection("Jwt")
```

This is used for JWT settings, API base URLs, retry settings, and connection strings.

## 98. Top-Level Statements

Modern C# allows executable code directly in `Program.cs` without manually writing a `Main` method.

Example:

```csharp
var builder = WebApplication.CreateBuilder(args);
```

The API and Web projects use this style.

## 99. Minimal Hosting Model

ASP.NET Core uses `WebApplication.CreateBuilder` and `builder.Build()` to configure and start the app.

Example:

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Run();
```

This is the modern way to start ASP.NET Core applications.

## 100. Layered Architecture

The project separates responsibilities into layers.

- `Web`: MVC UI and browser-facing pages.
- `Api`: REST API controllers and API responses.
- `Core`: business logic, contracts, models, and services.
- `Infrastructure`: database access, repositories, EF Core mappings, and migrations.
- `Tests`: automated tests.

This is not only a C# concept, but it is an important design pattern used throughout the project.
