**Minimal API** is a lightweight way to build HTTP APIs in **ASP.NET Core** with much less boilerplate code than traditional MVC controllers. Introduced in **.NET 6**, it is designed for creating simple, fast, and high-performance REST APIs.

## Traditional Controller-Based API

```csharp
// ProductsController.cs
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(new[] { "Laptop", "Phone", "Tablet" });
    }
}
```

This approach requires:

* A controller class
* Routing attributes
* Dependency injection through constructors
* Multiple files

---

## Minimal API Example

The same API can be written directly in `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/products", () =>
{
    return new[] { "Laptop", "Phone", "Tablet" };
});

app.Run();
```

A GET request to:

```
GET /products
```

returns:

```json
[
  "Laptop",
  "Phone",
  "Tablet"
]
```

---

# HTTP Methods in Minimal API

### GET

```csharp
app.MapGet("/hello", () => "Hello World");
```

---

### POST

```csharp
app.MapPost("/products", (Product product) =>
{
    return Results.Created($"/products/{product.Id}", product);
});
```

---

### PUT

```csharp
app.MapPut("/products/{id}", (int id, Product product) =>
{
    product.Id = id;
    return Results.Ok(product);
});
```

---

### DELETE

```csharp
app.MapDelete("/products/{id}", (int id) =>
{
    return Results.NoContent();
});
```

---

# Route Parameters

```csharp
app.MapGet("/products/{id}", (int id) =>
{
    return $"Product ID: {id}";
});
```

Request:

```
GET /products/10
```

Response:

```
Product ID: 10
```

---

# Query Parameters

```csharp
app.MapGet("/search", (string name) =>
{
    return $"Searching for {name}";
});
```

Request:

```
GET /search?name=Laptop
```

Response:

```
Searching for Laptop
```

---

# Dependency Injection

Services can be injected directly into endpoint handlers.

```csharp
builder.Services.AddSingleton<ProductService>();

app.MapGet("/products", (ProductService service) =>
{
    return service.GetProducts();
});
```

No constructor injection is needed.

---

# Reading Request Body

```csharp
app.MapPost("/products", (Product product) =>
{
    return Results.Ok(product);
});

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
```

JSON request:

```json
{
    "id": 1,
    "name": "Laptop"
}
```

ASP.NET Core automatically binds the JSON to the `Product` object.

---

# Using a Database (Entity Framework Core)

```csharp
builder.Services.AddDbContext<AppDbContext>();

app.MapGet("/products", async (AppDbContext db) =>
{
    return await db.Products.ToListAsync();
});

app.MapPost("/products", async (Product product, AppDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();

    return Results.Created($"/products/{product.Id}", product);
});
```

---

# Benefits of Minimal APIs

* ✅ Less boilerplate code
* ✅ Easy to learn
* ✅ High performance
* ✅ Built-in dependency injection
* ✅ Great for microservices
* ✅ Ideal for REST APIs and serverless applications

---

# Limitations

* Can become harder to organize as the application grows.
* Not ideal for large enterprise applications with many endpoints.
* Complex business logic is often easier to manage with controllers and separate service layers.

---

# Minimal API vs Controller API

| Feature          | Minimal API                         | Controller API                |
| ---------------- | ----------------------------------- | ----------------------------- |
| Boilerplate      | Very little                         | More                          |
| Learning curve   | Easier                              | Moderate                      |
| Performance      | Slightly better                     | Very good                     |
| Best for         | Small to medium APIs, microservices | Large enterprise applications |
| Uses controllers | No                                  | Yes                           |
| Routing          | `MapGet`, `MapPost`, etc.           | Attributes like `[HttpGet]`   |

---

## When should you use Minimal APIs?

Use Minimal APIs when:

* Building microservices
* Creating lightweight REST APIs
* Developing prototypes or internal tools
* Building serverless APIs (such as with Azure Functions integration)
* You want to minimize boilerplate while still leveraging ASP.NET Core features

For larger applications with many endpoints, extensive filters, or complex business logic, the controller-based approach often provides better structure and maintainability.

**In summary:** Minimal APIs let you define endpoints directly in your application's startup code using methods like `MapGet()`, `MapPost()`, `MapPut()`, and `MapDelete()`, reducing ceremony while retaining ASP.NET Core features such as dependency injection, model binding, authentication, authorization, and middleware.
