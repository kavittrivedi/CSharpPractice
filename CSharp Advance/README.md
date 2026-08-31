## Modern C# Language Features

### 1. Records vs. Classes vs. Structs

#### Class

A class is a reference type. By default, two class objects are equal only when they refer to the same object.

```csharp
class Person
{
    public string Name { get; set; }
}

var p1 = new Person { Name = "Kavita" };
var p2 = new Person { Name = "Kavita" };

Console.WriteLine(p1 == p2); // False
```

Use a class when:

- The object has an identity.
- Its data will change over time.
- Multiple variables may need to reference the same object.
- You are creating services, entities, controllers, or other behavior-rich objects.

#### Record class

A `record class` is also a reference type, but it uses value equality by default. Two records are considered equal when their values are equal.

```csharp
public record Person(string Name, int Age);

var p1 = new Person("Kavita", 30);
var p2 = new Person("Kavita", 30);

Console.WriteLine(p1 == p2); // True
```

Use a record class for data-focused reference types such as:

- DTOs
- API request and response models
- Configuration data
- Immutable application data

The short declaration automatically creates properties, a constructor, equality methods, and useful `ToString()` output.

#### Record struct

A `record struct` is a value type with generated value equality and record features.

```csharp
public readonly record struct Point(int X, int Y);
```

Use it for small values such as coordinates, measurements, dates, or money-like objects. Avoid using large structs because copying them can be expensive.

#### The `with` expression

A `with` expression creates a copy while changing selected values. It does not modify the original object.

```csharp
var original = new Person("Kavita", 30);
var updated = original with { Age = 31 };

Console.WriteLine(original.Age); // 30
Console.WriteLine(updated.Age);  // 31
```

This is called non-destructive mutation.

One important detail: for reference-type properties, `with` normally performs a shallow copy. The original and copied records can still refer to the same nested object.

**Interview answer:** A class normally uses reference equality, while records are designed for data and use value equality. A record class is a reference type, and a record struct is a value type. The `with` expression creates a copy with selected values changed.

---

### 2. Pattern Matching

Pattern matching lets us check a value’s type, shape, or contents while keeping the code concise.

#### Relational patterns

Relational patterns compare values using `<`, `>`, `<=`, or `>=`.

```csharp
string GetTemperatureMessage(int temperature) =>
    temperature switch
    {
        < 0 => "Freezing",
        <= 20 => "Cold",
        <= 30 => "Comfortable",
        _ => "Hot"
    };
```

Use them for ranges such as age, price, score, or temperature.

#### Property patterns

Property patterns inspect an object’s properties.

```csharp
if (person is { Age: >= 18, Name: "Kavita" })
{
    Console.WriteLine("Kavita is an adult.");
}
```

They can also check nested properties:

```csharp
if (order is { Customer.Address.Country: "India" })
{
    Console.WriteLine("Indian order");
}
```

#### List patterns

List patterns match elements inside arrays, lists, spans, and other supported sequence types.

```csharp
int[] numbers = [1, 2, 3, 4];

if (numbers is [1, 2, ..])
{
    Console.WriteLine("Starts with 1 and 2");
}
```

Here, `..` means “zero or more remaining elements.”

Other examples:

```csharp
numbers is []              // Empty
numbers is [var first, ..] // Has at least one item
numbers is [1, .., 4]      // Starts with 1 and ends with 4
```

#### Switch expressions

A switch expression returns a value and is usually shorter than a traditional `switch` statement.

```csharp
string roleName = roleId switch
{
    1 => "Admin",
    2 => "Manager",
    3 => "User",
    _ => "Unknown"
};
```

The `_` pattern means any value not matched earlier.

**Interview answer:** Pattern matching checks the type, properties, range, or structure of a value. Relational patterns check ranges, property patterns inspect objects, list patterns inspect sequences, and switch expressions return a result using concise matching rules.

---

### 3. Primary Constructors

Before C# 12, constructor parameters and assignments were written separately:

```csharp
public class Employee
{
    private readonly string _name;
    private readonly int _age;

    public Employee(string name, int age)
    {
        _name = name;
        _age = age;
    }
}
```

With a primary constructor, parameters are placed after the class name:

```csharp
public class Employee(string name, int age)
{
    public string GetDetails() => $"{name}, {age}";
}
```

This reduces constructor boilerplate.

For a struct:

```csharp
public struct Point(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
}
```

Unlike positional records, class primary-constructor parameters do not automatically become public properties. Define properties explicitly if callers need access:

```csharp
public class Employee(string name, int age)
{
    public string Name { get; } = name;
    public int Age { get; } = age;
}
```

Primary constructor parameters are available throughout the class body, but they are still parameters, not ordinary fields or properties.

**Interview answer:** C# 12 primary constructors let us declare constructor parameters beside the class or struct name. This reduces boilerplate, but those parameters do not automatically become properties in normal classes and structs.

---

### 4. Collection Expressions and Spread Elements

Collection expressions provide a shorter, consistent syntax for creating collections:

```csharp
int[] numbers = [1, 2, 3];

List<string> names = ["Kavita", "Amit", "Ravi"];
```

The target type tells the compiler which collection to create.

#### Spread element

The `..` element inserts items from another collection:

```csharp
int[] first = [1, 2];
int[] all = [..first, 3, 4];

// Result: 1, 2, 3, 4
```

Multiple collections can be combined:

```csharp
int[] left = [1, 2];
int[] right = [5, 6];

int[] combined = [..left, 3, 4, ..right];
```

Although it is commonly called the spread operator, the language specification calls `..` a spread element in this context.

#### Using `ReadOnlySpan<T>`

A collection expression can target `ReadOnlySpan<T>`:

```csharp
ReadOnlySpan<int> numbers = [1, 2, 3, 4];
```

A span represents a continuous region of memory without requiring a separate collection object in many scenarios. The compiler and runtime can use efficient storage for collection expressions, reducing allocations and copying.

`ReadOnlySpan<T>` is a `ref struct`, so it has safety restrictions:

- It cannot normally be stored as a field in a regular class.
- It cannot be boxed.
- It cannot live across an `await` or `yield` boundary where its lifetime would become unsafe.

**Interview answer:** Collection expressions use square brackets to create arrays, lists, spans, and other collections. The `..` spread element copies items from another collection. When the target is a span, C# can often avoid unnecessary heap allocations.

---

### 5. Required Properties and `init` Accessors

#### `init` accessor

An `init` property can be assigned only while the object is being created.

```csharp
public class User
{
    public string Name { get; init; }
}

var user = new User
{
    Name = "Kavita"
};

// user.Name = "Amit"; // Compile-time error
```

This helps make objects immutable after initialization.

#### `required` property

A `required` property tells the compiler that the caller must provide a value when creating the object.

```csharp
public class User
{
    public required string Name { get; init; }
    public required string Email { get; init; }
}
```

Valid creation:

```csharp
var user = new User
{
    Name = "Kavita",
    Email = "kavita@example.com"
};
```

Leaving out a required property causes a compiler error:

```csharp
var user = new User
{
    Name = "Kavita"
}; // Error: Email is required
```

They are commonly used together:

```csharp
public required string Name { get; init; }
```

- `required` means it must be supplied.
- `init` means it cannot be changed after initialization.

This provides safe object initialization without creating many constructor overloads.

One limitation is that `required` is compile-time protection. It does not automatically validate values received from JSON, databases, reflection, or other runtime sources.

**Interview answer:** `required` makes sure the caller provides a property during object creation. `init` allows that property to be assigned during initialization but prevents normal changes afterward. Together, they provide safer initialization with less constructor boilerplate.

## Short Combined Interview Answer

Modern C# features reduce boilerplate and make code safer. Records are useful for data models because they provide value equality and support copying with `with`. Pattern matching makes checks on ranges, properties, and collections concise. Primary constructors reduce constructor code. Collection expressions provide a common `[1, 2, 3]` syntax, while `..` adds items from another collection. Finally, `required` ensures important properties are supplied, and `init` prevents them from being changed after initialization.