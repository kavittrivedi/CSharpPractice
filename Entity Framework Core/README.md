## Interview Question: Explain  Lazy Loading and eager Loading in .net core 6 

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


## Interview Question: Explain Entity States in entity framework core. 

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
            .HasForeignKey(p => p.BlogId);
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
Interview Question: what is difference between Entity Framework core and Dapper? Entity Framework Core (EF Core) and Dapper are both popular data access technologies in .NET, but they serve different purposes and have distinct characteristics. Here’s a comparison of the two:

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
