## What is CQRS pattern? 

The CQRS (Command Query Responsibility Segregation) pattern is a software architectural pattern that separates read operations (queries) from write operations (commands). The main goal of CQRS is to optimize and scale applications by treating reads and writes as distinct concerns. 

Key Concepts:

Command:

Represents an operation that modifies the state of the system (e.g., create, update, delete actions).

Example: Adding a new product to an inventory.

Query:

Represents an operation that retrieves data but does not modify the state of the system.

Example: Fetching a list of products from the inventory.

Why Use CQRS?

Separation of Concerns: By separating commands and queries, you can design them independently for better performance and scalability.

Optimized Read/Write Models: Queries and commands can use different data models. For example:

Commands can use normalized data for transactional consistency.

Queries can use denormalized data optimized for fast retrieval.

Improved Performance: Queries can be tuned for read-heavy workloads, while commands can focus on write-heavy workloads.

Scalability: Each side (read or write) can scale independently based on the application's needs.

Example in Simple Terms:

Imagine an e-commerce application:

Command: When a customer places an order, a command updates the database with the order details.

Query: When the customer checks the status of their order, a query retrieves the information from a read-optimized database.

How CQRS Works:

Write Side (Commands):

Handles all requests that modify data.

Often uses domain models to enforce business rules.

Writes data to the write database.

Read Side (Queries):

Handles requests to retrieve data.

Often uses read models optimized for fast querying.

Reads data from a read database, which may be a denormalized or even a separate database replicated from the write database.

Benefits of CQRS:

Performance Optimization:

Queries are faster because they use denormalized data tailored for retrieval.

Commands are optimized for ensuring data consistency.

Scalability:

You can scale the read side and write side independently. For example, if your application has many reads but few writes, you can scale only the read side.

Flexibility:

Queries and commands can evolve independently.

You can implement caching or replicate data on the read side without affecting the write side.

Separation of Concerns:

Encourages a clear distinction between the business logic for writing data and the logic for reading data.

Challenges of CQRS:

Increased Complexity:

Maintaining separate read and write models adds complexity.

Synchronization between the write database and the read database may require event-driven mechanisms.

Eventual Consistency:

The read database might not be immediately updated after a write operation due to replication or message queue delays.

Implementation Overhead:

Requires additional infrastructure for event handling, messaging systems, and maintaining two data models.

Technologies Commonly Used with CQRS:

Event Sourcing: Captures changes (events) to the data rather than storing only the current state.

Messaging Systems: Tools like Azure Service Bus, RabbitMQ, or Kafka are used to synchronize data between the write and read sides.

Databases:

Write side: Relational databases for transactional consistency.

Read side: NoSQL or in-memory databases for fast data retrieval.

Simple Implementation Example:

In a .NET Core application:

Write Side:

```csharp
public class CreateOrderCommand
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderCommandHandler
{
    public void Handle(CreateOrderCommand command)
    {
        // Business logic to process the order
        // Save to write database
    }
}
```
 Read Side: 

```csharp 
public class OrderQuery
{
    public int OrderId { get; set; }
}

public class OrderQueryHandler
{
    public OrderDto Handle(OrderQuery query)
    {
        // Retrieve order from read database
        return new OrderDto { OrderId = query.OrderId, Status = "Shipped" };
    }
}

```
When to Use CQRS:

Applications with complex business logic and a clear separation between read and write operations.

Systems that require high scalability or have read-heavy workloads.

Applications that benefit from different data models for reads and writes, such as e-commerce or real-time analytics systems.

In Summary:

CQRS is a powerful pattern for separating read and write operations to optimize scalability, performance, and flexibility, especially in complex or high-traffic applications. However, it adds complexity and should be used only when the benefits outweigh the challenges.

## What is CQRS pattern? Explain in short and simple language with simple example. 

The CQRS (Command Query Responsibility Segregation) pattern separates write operations (commands) from read operations (queries).

Key Idea:

Command: Modifies data (e.g., adding an item to a database).

Query: Reads data without changing it (e.g., fetching an item's details).

Simple Example:

Imagine an online shopping app:

Command:

When a user places an order, it updates the system (e.g., saves the order to the database).

Action: PlaceOrder(orderDetails) updates the database.

Query:

When the user checks their order status, it reads the order details.

Action: GetOrder(orderId) retrieves the status.

Why Use CQRS?

Better performance by optimizing separately for reads and writes.

Flexibility to scale reading and writing independently.

Key Point:

Commands and queries can even use different databases: one for fast reads and another for safe writes.

## Now give me proper example of CQRS pattern in .net core. 

Here’s a proper example of implementing the CQRS Pattern in a .NET Core application. We'll use separate handlers for commands (write operations) and queries (read operations).

Scenario:

Let’s manage products in an inventory system:

Command: Add a new product (write operation).

Query: Get a list of all products (read operation).

Step-by-Step Example:
1. Define the Models: 
```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
``` 

2. Create Command to Add a Product:

Command: 
```csharp
public class AddProductCommand
{
    public string Name { get; set; }
    public decimal Price { get; set; }
} 
```
Command Handler: 

```csharp
public class AddProductCommandHandler

{
    private readonly ApplicationDbContext _context;

    public AddProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(AddProductCommand command)
    {
        var product = new Product
        {
            Name = command.Name,
            Price = command.Price
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }
}
```
 3. Create Query to Get Products:

Query:  
```csharp
public class GetProductsQuery
{
}
```
Query Handler: 
```csharp
public class GetProductsQueryHandler
{
    private readonly ApplicationDbContext _context;

    public GetProductsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> Handle(GetProductsQuery query)
    {
        return await _context.Products.ToListAsync();
    }
}
```
4. Set Up the Database Context: 
```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
}
```
5. Register Services in Startup.cs or Program.cs: 
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AddProductCommandHandler>();
builder.Services.AddScoped<GetProductsQueryHandler>(); 
```
6. Create the API Controller: 
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AddProductCommandHandler _addProductHandler;
    private readonly GetProductsQueryHandler _getProductsHandler;

    public ProductsController(AddProductCommandHandler addProductHandler, GetProductsQueryHandler getProductsHandler)
    {
        _addProductHandler = addProductHandler;
        _getProductsHandler = getProductsHandler;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] AddProductCommand command)
    {
        await _addProductHandler.Handle(command);
        return Ok("Product added successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _getProductsHandler.Handle(new GetProductsQuery());
        return Ok(products);
    }
} 
```
Summary:

Command (Write): Add a product.

Query (Read): Get the list of products.

Separate handlers for each operation ensure separation of concerns.

This structure makes the application more maintainable, scalable, and adaptable to changes.