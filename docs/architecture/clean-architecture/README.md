# Clean Architecture

## Simple Definition

Clean Architecture is a way of organizing an application so that the **business logic is independent of the UI, database, and external services**.

In simple words, the important business rules stay at the center, while technologies such as ASP.NET Core, Entity Framework Core, SQL Server, and third-party APIs stay outside. This makes the application easier to test, maintain, and change.

> **Interview answer:** Clean Architecture separates business logic from technical details. Its main rule is that dependencies point inward, toward the business rules. Therefore, the core application does not depend directly on the database, UI, or external frameworks.

## The Dependency Rule

The most important rule is:

**Outer layers can depend on inner layers, but inner layers must not depend on outer layers.**

```text
Presentation -> Application -> Domain
Infrastructure -> Application -> Domain
```

For example, the Application layer may define an `IOrderRepository` interface. The Infrastructure layer implements it using Entity Framework Core. The Application layer knows only the interface, so it does not care whether data is stored in SQL Server, MongoDB, or an in-memory collection.

## Common Layers

### 1. Domain Layer

This is the center of the application. It contains the main business concepts and rules.

Examples:

- Entities, such as `Order`, `Customer`, and `Product`
- Value objects
- Domain rules and domain exceptions

The Domain layer should not depend on Entity Framework Core, ASP.NET Core, or other outer layers.

### 2. Application Layer

This layer contains the application's use cases. It explains what the application can do.

Examples:

- `CreateOrder`
- `CancelOrder`
- `GetCustomerDetails`
- Interfaces such as `IOrderRepository` and `IEmailService`
- Commands, queries, DTOs, and validation

It coordinates domain objects but should not contain database or HTTP-specific code.

### 3. Infrastructure Layer

This layer contains technical implementations required by the application.

Examples:

- Entity Framework Core and `DbContext`
- Repository implementations
- Email, file storage, payment, and third-party API services
- Logging implementations

Infrastructure depends on interfaces defined by the inner layers and implements them.

### 4. Presentation Layer

This is the entry point through which users or other systems interact with the application.

Examples:

- ASP.NET Core Web API controllers
- MVC controllers and views
- Minimal API endpoints
- Request and response models

It accepts a request, calls an Application use case, and returns a response. It should not contain business rules.

## Example Request Flow

For a `POST /orders` request:

1. The controller receives the HTTP request.
2. It sends the data to the `CreateOrder` use case in the Application layer.
3. The use case applies business rules using Domain objects.
4. It calls `IOrderRepository`, an interface defined in an inner layer.
5. The Infrastructure repository saves the order through Entity Framework Core.
6. The controller returns the result to the client.

The Application layer does not know that Entity Framework Core or SQL Server performed the save.

## Simple C# Example

The Application layer defines the abstraction:

```csharp
public interface IOrderRepository
{
    Task AddAsync(Order order);
}

public sealed class CreateOrderService
{
    private readonly IOrderRepository _repository;

    public CreateOrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateAsync(int productId, int quantity)
    {
        var order = new Order(productId, quantity);
        await _repository.AddAsync(order);
    }
}
```

The Infrastructure layer supplies the implementation:

```csharp
public sealed class EfOrderRepository : IOrderRepository
{
    private readonly AppDbContext _dbContext;

    public EfOrderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
    }
}
```

Dependency Injection connects them at the application's entry point:

```csharp
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
```

This is an example of the **Dependency Inversion Principle**: business code depends on an abstraction, while the technical code implements that abstraction.

## Typical .NET Solution Structure

```text
MyApp.Domain/
  Entities/
  ValueObjects/

MyApp.Application/
  Interfaces/
  UseCases/
  DTOs/

MyApp.Infrastructure/
  Persistence/
  Repositories/
  ExternalServices/

MyApp.WebApi/
  Controllers/
  Program.cs
```

## Benefits

- Business logic is easier to unit test.
- The database, UI, or external service can be replaced more easily.
- Responsibilities are clearly separated.
- Large applications are easier to maintain and extend.
- Framework-specific code does not spread into the business logic.

## Disadvantages

- It creates more projects, interfaces, and classes.
- It has a learning curve for new developers.
- It can be unnecessary for a small CRUD application.
- Poorly designed abstractions can add complexity without providing value.

Clean Architecture is most useful for applications with important business rules, a long expected lifetime, or multiple external integrations.

## Common Interview Questions

### Is Clean Architecture the same as N-Tier Architecture?

No. Both separate responsibilities, but Clean Architecture strictly emphasizes that source-code dependencies point inward. Traditional N-Tier applications often allow the business layer to depend directly on the data-access layer.

### Where should business logic be written?

Core business rules belong in the Domain layer. Application workflow and use-case coordination belong in the Application layer. Controllers and repositories should not contain business rules.

### Where should repository interfaces be placed?

Place them in the Domain or Application layer, depending on which layer needs them. Their implementations belong in Infrastructure. The important point is that the inner layer owns the abstraction.

### Can the Domain layer use Entity Framework attributes?

Ideally, the Domain remains persistence-independent. Entity Framework configuration can be placed in Infrastructure using Fluent API. In smaller projects, limited attributes may be a practical compromise.

### How is Dependency Injection related to Clean Architecture?

Dependency Injection provides outer-layer implementations to inner-layer abstractions at runtime. It helps enforce loose coupling, but Dependency Injection alone does not make an application Clean Architecture.

## Short Final Answer for an Interview

> Clean Architecture organizes software into layers such as Domain, Application, Infrastructure, and Presentation. The Domain contains business rules, the Application layer contains use cases, Infrastructure handles technical details such as databases, and Presentation handles user or HTTP interaction. Its main rule is that dependencies point inward. This keeps business logic independent of frameworks and makes the system easier to test, maintain, and change.
