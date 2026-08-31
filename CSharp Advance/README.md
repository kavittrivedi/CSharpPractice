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

## 1. `Task` vs. `ValueTask`

Both represent an operation that may finish now or later.

### `Task`

`Task` is a reference type. A method returning a result usually returns `Task<T>`:

```csharp
public async Task<int> GetCountAsync()
{
    await Task.Delay(100);
    return 10;
}
```

A `Task` object may require a heap allocation. It is easy and safe to use, so it should normally be the default choice.

### `ValueTask`

`ValueTask<T>` is a value type. It can return a result directly when the operation completes synchronously:

```csharp
public ValueTask<string> GetNameAsync(int id)
{
    if (id == 1)
    {
        return ValueTask.FromResult("Kavita");
    }

    return new ValueTask<string>(LoadNameFromDatabaseAsync(id));
}
```

If the value is already cached, no separate `Task<string>` may need to be created. This can reduce heap allocations in frequently called, performance-sensitive code.

Use `ValueTask<T>` when:

- The method is called very frequently.
- It usually completes synchronously.
- Measurements show that `Task` allocations are a real performance problem.

Use `Task<T>` for normal application code because it is simpler and less error-prone.

### Do not await a `ValueTask` multiple times

A `Task` can safely be awaited more than once:

```csharp
Task<int> task = GetCountAsync();

int first = await task;
int second = await task;
```

A `ValueTask` should normally be awaited only once:

```csharp
ValueTask<int> valueTask = GetCountValueAsync();

int result = await valueTask;
```

This can be unsafe:

```csharp
int first = await valueTask;
int second = await valueTask; // Do not rely on this
```

Some `ValueTask` objects are backed by reusable asynchronous sources that allow only one consumption. Awaiting them more than once can produce incorrect results or exceptions.

If multiple awaits are necessary, convert it to a `Task` once:

```csharp
Task<int> task = valueTask.AsTask();

int first = await task;
int second = await task;
```

### Interview answer

`Task` is the normal and safer choice for asynchronous methods. `ValueTask` can reduce allocations when an operation frequently completes synchronously, such as returning cached data. However, it adds complexity and should normally be awaited only once. I use it only after measuring a real performance benefit.

---

## 2. Async Internals and `SynchronizationContext`

### How `async` and `await` work internally

An `async` method does not keep a thread blocked while waiting.

The compiler converts the method into a state machine. The state machine remembers:

- Where execution stopped
- Local variable values
- What operation is being awaited
- Where execution should continue

Consider this method:

```csharp
public async Task<string> GetDataAsync()
{
    var data = await LoadDataAsync();
    return data.ToUpper();
}
```

Its simplified behavior is:

1. Start executing `GetDataAsync`.
2. Call `LoadDataAsync`.
3. If the operation is incomplete, save the current state.
4. Return a `Task` to the caller.
5. Do not block the current thread.
6. When loading finishes, resume from after the `await`.
7. Complete the returned `Task`.

If the awaited operation is already complete, execution can continue immediately without suspending the method.

`async` does not automatically create a new thread. It allows the current thread to perform other work while an I/O operation is in progress.

### What is `SynchronizationContext`?

A `SynchronizationContext` represents an environment to which asynchronous code may need to return.

For example, in a desktop UI application, controls must be updated from the UI thread:

```csharp
private async void Button_Click(object sender, EventArgs e)
{
    var data = await LoadDataAsync();

    ResultLabel.Text = data; // Continues on the UI thread
}
```

By default, `await` may capture the current context and continue on it afterward.

### What does `ConfigureAwait(false)` do?

`ConfigureAwait(false)` tells the awaiter that the continuation does not need to return to the captured context:

```csharp
var data = await LoadDataAsync().ConfigureAwait(false);
```

The code after `await` may continue on any appropriate thread-pool thread.

Possible benefits include:

- Avoiding unnecessary context-switching overhead
- Reducing certain deadlock risks in older application models
- Making reusable library code independent of the caller’s context

### Where is it necessary?

It is most useful in:

- Reusable class libraries
- Older ASP.NET applications
- WinForms, WPF, or similar UI applications when the continuation does not access UI controls
- Code that may run under a custom `SynchronizationContext`

In ASP.NET Core, there is normally no custom `SynchronizationContext`, so `ConfigureAwait(false)` usually makes little practical difference.

Do not use it before code that must return to the UI thread:

```csharp
var data = await LoadDataAsync().ConfigureAwait(false);

// Unsafe in UI applications because this may not run on the UI thread
ResultLabel.Text = data;
```

Also, avoid blocking async code with `.Result` or `.Wait()`:

```csharp
var result = GetDataAsync().Result; // Can block or cause deadlocks
```

Prefer:

```csharp
var result = await GetDataAsync();
```

### Interview answer

The compiler converts an async method into a state machine. When an incomplete operation is awaited, the method saves its state and returns control without blocking the thread. When the operation finishes, execution resumes. `ConfigureAwait(false)` says that execution does not need to return to the original context. It is useful mainly in libraries, UI-related code, and older ASP.NET applications; it is generally unnecessary in ASP.NET Core.

---

## 3. Thread-Safe Primitives

These tools protect shared data when multiple threads or requests may access it simultaneously.

### `lock`

A `lock` allows only one thread at a time to execute a section of synchronous code:

```csharp
private readonly object _sync = new();
private int _count;

public void Increment()
{
    lock (_sync)
    {
        _count++;
    }
}
```

Use `lock` when:

- Protecting a small section of synchronous code
- Several related operations must happen together
- Only one thread should enter at a time

Do not use `await` inside a `lock`:

```csharp
lock (_sync)
{
    await SaveAsync(); // Compilation error
}
```

A thread waiting for a `lock` is blocked.

Also, lock on a private object, not on `this`, strings, or publicly accessible objects:

```csharp
private readonly object _sync = new();
```

### `SemaphoreSlim`

`SemaphoreSlim` controls how many operations can enter a section at the same time. It supports asynchronous waiting:

```csharp
private readonly SemaphoreSlim _semaphore = new(1, 1);

public async Task UpdateAsync()
{
    await _semaphore.WaitAsync();

    try
    {
        await SaveChangesAsync();
    }
    finally
    {
        _semaphore.Release();
    }
}
```

A semaphore with a maximum count of one behaves like an asynchronous lock.

Use `SemaphoreSlim` when:

- The protected operation uses `await`.
- Waiting should not block a thread.
- You want to limit concurrency.

For example, allow only three concurrent operations:

```csharp
private readonly SemaphoreSlim _semaphore = new(3, 3);
```

Always call `Release()` in a `finally` block.

### `Interlocked`

`Interlocked` performs simple atomic operations without a traditional lock:

```csharp
private int _count;

public void Increment()
{
    Interlocked.Increment(ref _count);
}
```

Other operations include:

```csharp
Interlocked.Decrement(ref _count);
Interlocked.Exchange(ref _count, 0);
Interlocked.CompareExchange(ref _count, newValue, expectedValue);
```

Use `Interlocked` for simple operations on individual values, such as:

- Incrementing a counter
- Replacing a reference
- Setting a flag
- Comparing and replacing a value

It is usually faster than a lock for these small operations, but it is not suitable for complicated logic involving several values.

### Comparison

| Tool | Best use | Blocks a thread? | Supports `await`? |
|---|---|---:|---:|
| `lock` | Protect synchronous multi-step logic | Yes, while waiting | No |
| `SemaphoreSlim` | Protect or limit asynchronous operations | No with `WaitAsync` | Yes |
| `Interlocked` | Simple atomic operations | No traditional blocking | Not needed |

### Interview answer

I use `lock` to protect a short synchronous critical section, `SemaphoreSlim` when the protected work is asynchronous or concurrency must be limited, and `Interlocked` for simple atomic operations such as incrementing a counter. I never place `await` inside a `lock`.

---

## 4. `CancellationToken`

A `CancellationToken` allows the caller to request that an operation stop.

Cancellation is cooperative. It does not forcibly terminate the operation. The method must observe the token and stop safely.

### Accept and pass the token

A service method should accept a token:

```csharp
public async Task<Order> GetOrderAsync(
    int orderId,
    CancellationToken cancellationToken)
{
    return await _repository.GetOrderAsync(
        orderId,
        cancellationToken);
}
```

Pass the same token through every service layer:

```csharp
Controller
    -> Service
        -> Repository
            -> Database or HTTP call
```

Example controller:

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> Get(
    int id,
    CancellationToken cancellationToken)
{
    var order = await _service.GetOrderAsync(
        id,
        cancellationToken);

    return Ok(order);
}
```

In ASP.NET Core, this token is connected to the HTTP request. It is cancelled when the client disconnects or cancels the request.

### Pass it to supported async methods

```csharp
await dbContext.SaveChangesAsync(cancellationToken);

await httpClient.GetAsync(url, cancellationToken);

await Task.Delay(1000, cancellationToken);

await semaphore.WaitAsync(cancellationToken);
```

For custom or CPU-bound work, check it periodically:

```csharp
foreach (var item in items)
{
    cancellationToken.ThrowIfCancellationRequested();
    Process(item);
}
```

### Handling `OperationCanceledException`

`ThrowIfCancellationRequested()` and many framework methods throw `OperationCanceledException` when cancellation is requested.

Usually, do not treat expected cancellation as a normal application error:

```csharp
try
{
    await ProcessAsync(cancellationToken);
}
catch (OperationCanceledException)
    when (cancellationToken.IsCancellationRequested)
{
    // Optional cleanup or logging
    throw;
}
```

After cleanup, rethrow the exception so the caller knows that the operation was cancelled.

Do not swallow cancellation and return a successful result:

```csharp
catch (OperationCanceledException)
{
    return new Result { Success = true }; // Misleading
}
```

Also, avoid passing `CancellationToken.None` when the caller already supplied a token:

```csharp
await repository.SaveAsync(CancellationToken.None); // Breaks propagation
```

Once an important operation must complete for data consistency, cancellation may need careful handling. For example, do not stop halfway through a critical multi-step update unless a database transaction can roll it back safely.

### Interview answer

A cancellation token lets the caller request cancellation, but the operation must cooperate. I accept the token at the API or service boundary and pass it through services, repositories, database calls, HTTP calls, delays, and semaphore waits. For custom work, I check it with `ThrowIfCancellationRequested()`. I treat `OperationCanceledException` as expected cancellation, perform cleanup if needed, and normally rethrow it instead of reporting success.

## Short Combined Interview Answer

`Task` is the normal choice for asynchronous work, while `ValueTask` can reduce allocations when operations usually complete synchronously, but it should normally be awaited only once. The compiler implements async methods as state machines, and `ConfigureAwait(false)` prevents returning to a captured context when that context is unnecessary. For synchronization, I use `lock` for synchronous critical sections, `SemaphoreSlim` for asynchronous synchronization, and `Interlocked` for simple atomic updates. I propagate `CancellationToken` through every layer and treat `OperationCanceledException` as expected cancellation rather than an application failure.
