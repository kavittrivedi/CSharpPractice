# Entity Framework Core Interview Practice

## Which packages are required if you want to use EF Core in a .NET Core application?

To use **Entity Framework Core** in a .NET Core application, you usually need the following NuGet packages:

### Required Packages

1. **Microsoft.EntityFrameworkCore**

   * This is the main EF Core package.
   * It provides the core functionality of Entity Framework Core.

2. **Database Provider Package**

   * EF Core needs a database provider package based on the database you are using.
   * Common examples:

     | Database | Package |
     | -------- | ------- |
     | SQL Server | `Microsoft.EntityFrameworkCore.SqlServer` |
     | SQLite | `Microsoft.EntityFrameworkCore.Sqlite` |
     | PostgreSQL | `Npgsql.EntityFrameworkCore.PostgreSQL` |
     | MySQL | `Pomelo.EntityFrameworkCore.MySql` |

3. **Microsoft.EntityFrameworkCore.Tools**

   * This package is used for EF Core commands such as migrations.
   * Example commands include `Add-Migration`, `Update-Database`, and `Script-Migration`.

### Optional Packages

1. **Microsoft.EntityFrameworkCore.Design**

   * This package is commonly used at design time for migrations and scaffolding.

2. **Microsoft.EntityFrameworkCore.Proxies**

   * This package is required only if you want to use lazy loading proxies.

### Example for SQL Server

```powershell
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.EntityFrameworkCore.Design
```

### Summary

For a basic EF Core setup with SQL Server, the most commonly used packages are:

* `Microsoft.EntityFrameworkCore`
* `Microsoft.EntityFrameworkCore.SqlServer`
* `Microsoft.EntityFrameworkCore.Tools`
* `Microsoft.EntityFrameworkCore.Design`

## EF Core Migration Commands

Here’s a handy list of the **Entity Framework Core migration commands** you’ll often use — both the `dotnet ef` CLI style and the **Package Manager Console (PMC)** style (`add-migration`, `update-database`, etc.):

---

### ⚙️ Using .NET CLI (VS Code / Terminal)
- **Add a migration**  
  ```bash
  dotnet ef migrations add InitialCreate
  ```
- **Update database with latest migration**  
  ```bash
  dotnet ef database update
  ```
- **Remove last migration (if not applied)**  
  ```bash
  dotnet ef migrations remove
  ```
- **List all migrations**  
  ```bash
  dotnet ef migrations list
  ```
- **Generate SQL script for migrations**  
  ```bash
  dotnet ef migrations script
  ```

---

### ⚙️ Using Package Manager Console (Visual Studio)
- **Add a migration**  
  ```powershell
  Add-Migration InitialCreate
  ```
- **Update database**  
  ```powershell
  Update-Database
  ```
- **Remove last migration**  
  ```powershell
  Remove-Migration
  ```
- **Script migrations to SQL**  
  ```powershell
  Script-Migration
  ```

---

💡 **Quick workflow memory tip:**  
1. **Add-Migration** → creates migration file.  
2. **Update-Database** → applies migration to DB.  
3. **Remove-Migration** → undo if needed.  
4. **Script-Migration** → generate SQL script.  

---

## How to set 1 to many and many to many relationship in entity framework core? 

In Entity Framework Core, you can set up **one-to-many** and **many-to-many** relationships using navigation properties and the Fluent API or data annotations. Here’s how to do both:

### One-to-Many Relationship

A one-to-many relationship means that one entity can be related to many instances of another entity. For example, a `Blog` can have many `Posts`.

#### 1. **Define the Entities**

```csharp
public class Blog
{
    public int BlogId { get; set; }
    public string Name { get; set; }

    // Navigation property
    public ICollection<Post> Posts { get; set; }
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }

    // Foreign key
    public int BlogId { get; set; }

    // Navigation property
    public Blog Blog { get; set; }
}
```

#### 2. **Configure the Relationship using Fluent API**

In your `DbContext`, override the `OnModelCreating` method to configure the relationship:

```csharp
public class MyDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .HasMany(b => b.Posts)
            .WithOne(p => p.Blog)
            .HasForeignKey(p => p.BlogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

### Many-to-Many Relationship

In a many-to-many relationship, multiple instances of one entity can be related to multiple instances of another entity. For example, `Students` and `Courses` can have many-to-many relationships.

#### 1. **Define the Entities**

In EF Core 5.0 and later, you can use a simpler way to define many-to-many relationships without needing a separate join entity.

```csharp
public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }

    // Navigation property
    public ICollection<Course> Courses { get; set; }
}

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }

    // Navigation property
    public ICollection<Student> Students { get; set; }
}
```

#### 2. **Configure the Relationship using Fluent API**

In your `DbContext`, configure the many-to-many relationship:

```csharp
public class MyDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Courses)
            .WithMany(c => c.Students)
            .UsingEntity(j => j.ToTable("StudentCourses")); // Optional: specify the join table name
    }
}
```

### Summary

* **One-to-Many**: Use a foreign key in the child entity and configure the relationship using navigation properties and Fluent API.
* **Many-to-Many**: Use navigation properties in both entities and configure the relationship using Fluent API. In EF Core 5.0 and later, you can directly define many-to-many relationships without a separate join entity.

These configurations allow Entity Framework Core to manage the relationships and ensure that the appropriate data is saved to and retrieved from the database.

## How to Call Stored Procedure Using Entity Framework Core

In Entity Framework Core, we can call a stored procedure in two common ways:

* Use `FromSqlRaw()` when the stored procedure returns data.
* Use `ExecuteSqlRaw()` when the stored procedure does insert, update, or delete work.

### 1. Stored Procedure That Returns Data

Suppose we have this stored procedure in SQL Server:

```sql
CREATE PROCEDURE GetEmployees
AS
BEGIN
    SELECT Id, Name, Email, Salary
    FROM Employees
END
```

We can call it from EF Core like this:

```csharp
var employees = _context.Employees
    .FromSqlRaw("EXEC GetEmployees")
    .ToList();
```

Here, `Employees` is a `DbSet` in the `DbContext`.

```csharp
public DbSet<Employee> Employees { get; set; }
```

The stored procedure result columns should match the `Employee` class properties.

### 2. Stored Procedure With Parameter

Suppose we have this stored procedure:

```sql
CREATE PROCEDURE GetEmployeeById
    @Id INT
AS
BEGIN
    SELECT Id, Name, Email, Salary
    FROM Employees
    WHERE Id = @Id
END
```

We can call it like this:

```csharp
int employeeId = 1;

var employee = _context.Employees
    .FromSqlRaw("EXEC GetEmployeeById @p0", employeeId)
    .FirstOrDefault();
```

`@p0` is replaced by the value of `employeeId`.

### 3. Stored Procedure for Insert, Update, or Delete

If the stored procedure does not return data, use `ExecuteSqlRaw()`.

Example stored procedure:

```sql
CREATE PROCEDURE UpdateEmployeeSalary
    @Id INT,
    @Salary DECIMAL(18, 2)
AS
BEGIN
    UPDATE Employees
    SET Salary = @Salary
    WHERE Id = @Id
END
```

Call it from EF Core:

```csharp
int employeeId = 1;
decimal newSalary = 60000;

_context.Database.ExecuteSqlRaw(
    "EXEC UpdateEmployeeSalary @p0, @p1",
    employeeId,
    newSalary);
```

### Simple Summary

* If stored procedure returns rows, use `FromSqlRaw()`.
* If stored procedure changes data, use `ExecuteSqlRaw()`.
* Use parameters instead of joining values into SQL strings.
* This helps avoid SQL injection.

### How many ways we can add master data using migration?

### Ways to add master (reference) data using EF Core migrations

Below are the common approaches, with short code examples, pros/cons, and practical tips.

---

#### 1. **Model seeding with `HasData` (convention + migrations)**
**What:** Define seed data in `OnModelCreating` using `modelBuilder.Entity<T>().HasData(...)`. When you add a migration, EF generates `InsertData`/`UpdateData` calls in the migration.

**Example**
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Country>().HasData(
        new Country { CountryId = 1, Code = "IN", Name = "India" },
        new Country { CountryId = 2, Code = "US", Name = "United States" }
    );
}
```

**Pros:** Simple; migrations auto-generate; works well for small, static master data.  
**Cons:** Requires stable primary keys; not ideal for large datasets or environment-specific data.

---

#### 2. **Manual `InsertData` / `UpdateData` in migration `Up`/`Down`**
**What:** Edit the generated migration (or create one) and use `migrationBuilder.InsertData`, `UpdateData`, `DeleteData` explicitly.

**Example**
```csharp
public partial class SeedCountries : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Countries",
            columns: new[] { "CountryId", "Code", "Name" },
            values: new object[,]
            {
                { 1, "IN", "India" },
                { 2, "US", "United States" }
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(table: "Countries", keyColumn: "CountryId", keyValue: 1);
        migrationBuilder.DeleteData(table: "Countries", keyColumn: "CountryId", keyValue: 2);
    }
}
```

**Pros:** Full control; explicit and versioned; easy to update/remove.  
**Cons:** Manual work for many rows; must manage keys and idempotency.

---

#### 3. **Raw SQL in migration (`migrationBuilder.Sql`)**
**What:** Execute raw SQL statements inside `Up`/`Down`. Useful for complex inserts, conditional logic, or bulk inserts.

**Example**
```csharp
migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM Countries WHERE Code = 'IN')
    INSERT INTO Countries (Code, Name) VALUES ('IN', 'India');
");
```

**Pros:** Flexible; can write idempotent SQL; good for large/bulk inserts or DB-specific features.  
**Cons:** Less portable; harder to maintain than structured `InsertData`.

---

#### 4. **Use an explicit join/seed table or CSV + migration script**
**What:** Keep master data in a file (CSV/JSON) and have the migration run a SQL script that loads it (e.g., `BULK INSERT` or multiple `INSERT`s).

**Pros:** Keeps large datasets out of code; easier to update data files.  
**Cons:** More setup; provider-specific; migration must include script execution.

---

#### 5. **Call a seeding method from migration (discouraged but possible)**
**What:** Instantiate a `DbContext` inside the migration and run code to seed. This is possible but generally discouraged because migrations should be deterministic and not depend on DI or runtime services.

**Example (not recommended)**
```csharp
using (var context = new MyDbContext(...))
{
    if (!context.Countries.Any())
    {
        context.Countries.Add(new Country { Code="IN", Name="India" });
        context.SaveChanges();
    }
}
```

**Pros:** Can reuse application seeding logic.  
**Cons:** Can break in different environments; harder to guarantee idempotency and determinism.

---

#### 6. **Generate SQL script and run separately (`dotnet ef migrations script`)**
**What:** Create a SQL script from migrations and run it manually or via deployment pipeline; you can append custom seed SQL to the script.

**Pros:** Good for DBAs and controlled deployments; can include environment-specific logic.  
**Cons:** Manual step unless automated in CI/CD.

---

## Explain  Lazy Loading and eager Loading in .net core 6

**Lazy Loading** and **Eager Loading** are two strategies for loading related data in Entity Framework Core. They help manage how data is retrieved from the database, particularly when working with relationships between entities.

### Lazy Loading

* **Definition**: Lazy loading means that related data is loaded on demand, only when it is accessed. This can help improve performance and reduce memory usage, especially if related data is not always needed.

* **How It Works**: When you access a navigation property of an entity, EF Core automatically loads the related data from the database. This typically requires a proxy for the entity.

* **Setup**: To use lazy loading, you need to install the `Microsoft.EntityFrameworkCore.Proxies` package and configure it in your `DbContext`.

  ```csharp
  services.AddDbContext<MyDbContext>(options =>
      options.UseLazyLoadingProxies()
             .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
  ```

* **Example**:

  ```csharp
  public class Blog
  {
      public int BlogId { get; set; }
      public string Name { get; set; }
      public virtual ICollection<Post> Posts { get; set; } // Navigation property
  }

  // Usage
  var blog = context.Blogs.Find(1);
  var posts = blog.Posts; // Posts are loaded here (on-demand)
  ```

### Eager Loading

* **Definition**: Eager loading means that related data is loaded at the same time as the main entity when the query is executed. This is useful when you know you will need the related data immediately.

* **How It Works**: You specify which related entities to include in the query using the `Include` method.

* **Setup**: Eager loading does not require any special setup in EF Core; you simply use the `Include` method in your queries.

* **Example**:

  ```csharp
  var blogs = context.Blogs
      .Include(b => b.Posts) // Eagerly load related Posts
      .ToList();
  ```

### Comparison

| Feature        | Lazy Loading                                                            | Eager Loading                                      |
| -------------- | ----------------------------------------------------------------------- | -------------------------------------------------- |
| Data Retrieval | Loads related data on demand                                            | Loads related data with the main entity            |
| Performance    | May result in multiple database queries if many properties are accessed | Usually requires fewer database queries            |
| Memory Usage   | Can use less memory initially                                           | Uses more memory as related data is loaded upfront |
| Control        | Less control over when data is loaded                                   | More control over what data is loaded              |

### Summary

* **Lazy Loading** is useful for reducing initial load times and memory usage, but it can lead to the "N+1" query problem, where multiple queries are executed for related data.
* **Eager Loading** is beneficial for loading related data efficiently when you know you'll need it, reducing the number of queries and improving performance when accessing related entities.
**Lazy Loading** and **Eager Loading** are two strategies for loading related data in Entity Framework Core. They help manage how data is retrieved from the database, particularly when working with relationships between entities.


##  Explain Entity States in entity framework core. 

In Entity Framework Core, **entity states** represent the status of an entity as it interacts with the database. Understanding these states is crucial for managing how changes to entities are tracked and persisted. The primary entity states in EF Core are:

### 1. **Added**

* **Definition**: The entity is newly created and is marked for insertion into the database.
* **Example**: When you create a new instance of an entity and call `DbContext.Add(entity)`.
* **Behavior**: When `SaveChanges()` is called, the entity will be inserted into the database.

### 2. **Unchanged**

* **Definition**: The entity exists in the database and has not been modified since it was loaded.
* **Example**: When an entity is retrieved from the database but not modified.
* **Behavior**: When `SaveChanges()` is called, no action is taken for this entity since it is unchanged.

### 3. **Modified**

* **Definition**: The entity has been modified after it was loaded into the context.
* **Example**: Changing a property value of an entity that has been retrieved from the database.
* **Behavior**: When `SaveChanges()` is called, an `UPDATE` statement is generated for this entity.

### 4. **Deleted**

* **Definition**: The entity is marked for deletion from the database.
* **Example**: When you call `DbContext.Remove(entity)` or set an entity's state to `EntityState.Deleted`.
* **Behavior**: When `SaveChanges()` is called, a `DELETE` statement is executed for this entity.

### 5. **Detached**

* **Definition**: The entity is not being tracked by the context. This usually occurs when the entity has been removed from the context or was never added to it.
* **Example**: When an entity is created but not added to the `DbContext`, or after calling `DbContext.Entry(entity).State = EntityState.Detached`.
* **Behavior**: The entity is not saved to the database, and any changes made to it will not be tracked.

### Summary of Entity States

| Entity State | Description                             | Action on SaveChanges()    |
| ------------ | --------------------------------------- | -------------------------- |
| Added        | New entity to be inserted               | Generates INSERT statement |
| Unchanged    | Entity exists and has not been modified | No action                  |
| Modified     | Entity has been changed after loading   | Generates UPDATE statement |
| Deleted      | Entity marked for removal               | Generates DELETE statement |
| Detached     | Entity not tracked by the context       | No action                  |

### Managing Entity States

* **Explicit State Management**: You can explicitly set the state of an entity using the `DbContext.Entry(entity).State` property to `EntityState` values.
* **Tracking Changes**: EF Core automatically tracks changes made to entities as long as they are attached to the context, allowing for efficient updates to the database.

Understanding these states helps developers effectively manage data changes and interactions with the database in Entity Framework Core applications.

## What is difference between Entity Framework core and Dapper? 

Entity Framework Core (EF Core) and Dapper are both popular data access technologies in .NET, but they serve different purposes and have distinct characteristics. Here’s a comparison of the two:

### Entity Framework Core (EF Core)

1. **Type**:

   * Object-Relational Mapper (ORM)

2. **Abstraction**:

   * Provides a high level of abstraction. Developers work with strongly-typed domain models rather than dealing directly with SQL queries.

3. **Change Tracking**:

   * Automatically tracks changes made to the entities and handles updates automatically when `SaveChanges()` is called.

4. **Lazy Loading**:

   * Supports lazy loading, eager loading, and explicit loading of related data through navigation properties.

5. **Migrations**:

   * Provides built-in support for database migrations, allowing easy schema updates based on changes in the entity model.

6. **Performance**:

   * Generally slower than Dapper due to the overhead of change tracking and abstraction. Suitable for applications where ease of use and rapid development are prioritized.

7. **Complex Queries**:

   * Can handle complex queries, but performance may degrade for highly complex scenarios. It translates LINQ queries into SQL.

8. **Use Cases**:

   * Ideal for applications that require extensive data manipulation and where working with object graphs is common.

### Dapper

1. **Type**:

   * Micro ORM

2. **Abstraction**:

   * Provides a minimal level of abstraction. Developers write SQL queries directly, making it lightweight and faster for simple scenarios.

3. **Change Tracking**:

   * Does not provide built-in change tracking. Developers must handle state management and updates manually.

4. **Loading Related Data**:

   * Requires manual handling of related data. You can use multiple queries or `JOIN` statements to load related entities.

5. **Migrations**:

   * Does not have built-in migration support. Developers usually handle database schema changes manually or with separate tools.

6. **Performance**:

   * Generally faster than Dapper due to the lack of overhead. It’s efficient for simple queries and when high performance is required.

7. **Complex Queries**:

   * Better suited for scenarios with complex SQL queries, as it allows for direct execution of SQL commands.

8. **Use Cases**:

   * Ideal for performance-critical applications, simple CRUD operations, or when you need to run complex queries.

### Summary of Differences

| Feature         | Entity Framework Core       | Dapper                           |
| --------------- | --------------------------- | -------------------------------- |
| Type            | ORM                         | Micro ORM                        |
| Abstraction     | High-level abstraction      | Minimal abstraction              |
| Change Tracking | Automatic tracking          | Manual handling                  |
| Lazy Loading    | Supported                   | Not supported                    |
| Migrations      | Built-in support            | No built-in support              |
| Performance     | Slower due to overhead      | Faster due to lightweight design |
| Complex Queries | Translates LINQ to SQL      | Direct SQL execution             |
| Use Cases       | Extensive data manipulation | Performance-critical scenarios   |

### Conclusion

Choose **Entity Framework Core** when you need a robust, feature-rich ORM with high-level abstractions and automatic change tracking. Opt for **Dapper** when you require high performance and prefer to write raw SQL queries, especially for simple CRUD operations or complex queries where performance is critical.

## How transaction works in Entity framework core explain with simple example.

In Entity Framework Core, transactions ensure that a series of database operations are executed as a single unit, meaning either all operations succeed, or none of them are committed. Transactions can be manually handled using `BeginTransaction()` and `Commit()`, or automatically using `SaveChanges()`.

### Example:

```csharp
using (var transaction = await _context.Database.BeginTransactionAsync())
{
    try
    {
        _context.Students.Add(new Student { Name = "John" });
        await _context.SaveChangesAsync();

        _context.Courses.Add(new Course { Title = "Math" });
        await _context.SaveChangesAsync();

        await transaction.CommitAsync(); // Commits both operations
    }
    catch (Exception)
    {
        await transaction.RollbackAsync(); // Rolls back if any operation fails
    }
}
```

Here, both `Students` and `Courses` additions are part of the same transaction. If one fails, the transaction is rolled back, ensuring data consistency.

## AddDbContext VS AddDbContextPool in EF Core

In .NET Core, both `AddDbContext` and `AddDbContextPool` are used to register `DbContext` in dependency injection.

But they work in a slightly different way.

### AddDbContext

`AddDbContext` creates a new `DbContext` object for each request.

Example:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
```

Here, when a request comes, .NET creates a new object of `AppDbContext`.

After the request is completed, that object is disposed.

Simple meaning:

```text
New request = New DbContext object
```

### AddDbContextPool

`AddDbContextPool` reuses `DbContext` objects from a pool.

Example:

```csharp
builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
```

Here, .NET does not always create a new `DbContext` object.

It takes an existing `DbContext` object from the pool, uses it, resets it, and keeps it back in the pool.

Simple meaning:

```text
New request = Reuse DbContext object from pool
```

### Real Life Example

Think about a hotel.

`AddDbContext` is like buying a new plate for every customer and throwing it away after use.

`AddDbContextPool` is like using plates, washing them, and reusing them for the next customer.

So, `AddDbContextPool` can improve performance because it reduces object creation.

### Difference Between AddDbContext and AddDbContextPool

| Feature | AddDbContext | AddDbContextPool |
|---|---|---|
| Object creation | Creates new DbContext object | Reuses DbContext object |
| Performance | Good | Better for high traffic |
| Memory usage | More object creation | Less object creation |
| State handling | Safer for custom state | Need to be careful with custom state |
| Common use | Normal applications | High-performance applications |

### When to Use AddDbContext

Use `AddDbContext` when:

* Your application is simple or medium size.
* You do not have very high traffic.
* Your `DbContext` has custom state or extra properties.
* You want the safest and most common approach.

### When to Use AddDbContextPool

Use `AddDbContextPool` when:

* Your application has high traffic.
* You want better performance.
* Your `DbContext` does not store custom request-specific state.
* You want to reduce object creation.

### Important Point About AddDbContextPool

When using `AddDbContextPool`, do not store request-specific data inside your `DbContext`.

Bad example:

```csharp
public class AppDbContext : DbContext
{
    public int CurrentUserId { get; set; }
}
```

This can be risky with pooling because the same `DbContext` object may be reused for another request.

### Interview Answer

`AddDbContext` creates a new `DbContext` instance for each request and disposes it after the request is completed.

`AddDbContextPool` keeps `DbContext` objects in a pool and reuses them for better performance.

`AddDbContext` is simple and safe for most applications. `AddDbContextPool` is useful for high-performance applications, but we should avoid storing request-specific state inside the `DbContext`.

### Simple Summary

* `AddDbContext` creates a new object every time.
* `AddDbContextPool` reuses objects from a pool.
* `AddDbContext` is safer and commonly used.
* `AddDbContextPool` can improve performance.
* Be careful with custom state when using `AddDbContextPool`.

