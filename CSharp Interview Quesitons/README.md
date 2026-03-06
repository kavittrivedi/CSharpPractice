# CSharpPractice

## How foreach Loop Works

how foreach loop works. explain technically. 

The foreach loop in C# works by leveraging the IEnumerable and IEnumerator interfaces. Here's a technical explanation of how it functions:

### Step-by-Step Execution of foreach

1. **Check if the Object Implements IEnumerable**  
   The compiler checks if the collection object being iterated implements the IEnumerable or IEnumerable<T> interface. This interface provides the GetEnumerator() method.  
   If the object implements IEnumerable, foreach can proceed.  
   If not, a compile-time error occurs.

2. **Call GetEnumerator Method**  
   The GetEnumerator() method of IEnumerable is called, which returns an IEnumerator (or IEnumerator<T> for generic collections). The IEnumerator is responsible for actual iteration.

3. **Use IEnumerator to Traverse**  
   The IEnumerator interface provides these key members:  
   - MoveNext(): Advances the enumerator to the next element in the collection. Returns true if there’s another element, otherwise false.  
   - Current: Retrieves the current element in the collection.  
   - Reset(): Resets the enumerator to its initial position (before the first element). However, Reset is rarely used and is optional.  

   The foreach loop internally works like this:  
   - Calls MoveNext() to check if there’s a next element.  
   - Accesses the Current property to retrieve the element.  
   - Repeats the process until MoveNext() returns false.

4. **Dispose Enumerator**  
   After the loop completes, the Dispose() method of the IEnumerator (if it implements IDisposable) is called to release any unmanaged resources.

### Technical Representation of foreach

The following foreach loop:  
```csharp
foreach (var item in collection)
{
    Console.WriteLine(item);
}
```  
is equivalent to this code using IEnumerator:  
```csharp
var enumerator = collection.GetEnumerator();
try
{
    while (enumerator.MoveNext()) // Move to the next element
    {
        var item = enumerator.Current; // Get the current element
        Console.WriteLine(item);
    }
}
finally
{
    // Dispose enumerator if necessary
    if (enumerator is IDisposable disposable)
    {
        disposable.Dispose();
    }
}
```

### Interfaces Involved

- **IEnumerable Interface**: Provides the GetEnumerator() method.
- **IEnumerator Interface**:  
  - MoveNext(): Advances the enumerator.  
  - Current: Retrieves the current item.  
  - Dispose(): Cleans up resources.

### Example: Custom foreach Implementation

Let's create a custom collection that supports the foreach loop:  
```csharp
using System;
using System.Collections;
using System.Collections.Generic;

class MyCollection : IEnumerable<int>
{
    private int[] data = { 1, 2, 3 };

    public IEnumerator<int> GetEnumerator()
    {
        foreach (var item in data)
        {
            yield return item; // Return items one by one
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class Program
{
    static void Main()
    {
        var myCollection = new MyCollection();

        // foreach uses GetEnumerator internally
        foreach (var item in myCollection)
        {
            Console.WriteLine(item);
        }
    }
}
```

### Key Advantages of foreach

- **Simplifies Iteration**: No need to manually manage the enumerator.
- **Safe**: Automatically handles resource cleanup with Dispose().
- **Readable**: Cleaner and less error-prone compared to while loops with enumerators.

## Difference Between for and foreach Loop

Difference between for and foreach loop.  

| **Aspect**                             | **`for` Loop**                                                        | **`foreach` Loop**                                                        |
| -------------------------------------- | --------------------------------------------------------------------- | ------------------------------------------------------------------------- |
| **Purpose**                            | Iterates using an **index** or **counter**.                           | Iterates through each element of a collection directly.                   |
| **Collection Type**                    | Works with **indexed collections** like arrays or lists.              | Works with any collection that implements `IEnumerable` or `IEnumerator`. |
| **Control**                            | Provides more **control** over iteration (e.g., skipping, reversing). | Simplifies iteration without needing an index or counter.                 |
| **Modification**                       | Allows modifying the collection during iteration.                     | Does **not allow modification** of the collection directly.               |
| **Readability**                        | Requires manual management of the index.                              | Easier to read and write.                                                 |
| **Example**                            |                                                                       |                                                                           |
| **Example with `for`**                 |                                                                       | **Example with `foreach`**                                                |
| ```csharp                              | ```csharp                                                             | ```csharp                                                                 |
| for (int i = 0; i < array.Length; i++) | foreach (var item in array)                                           |                                                                           |
| {                                      | {                                                                     |                                                                           |
| Console.WriteLine(array[i]);           | Console.WriteLine(item);                                              |                                                                           |
| }                                      | }                                                                     |                                                                           |

### When to Use

**Use `for`:**  
- When you need an index or specific iteration logic (e.g., skip elements).  
- When you want to iterate backwards or access specific elements.

**Use `foreach`:**  
- When you just want to access all elements in a collection.  
- For better readability and less error-prone code.

## Explain IQueryable with Simple Example

Explain me IQueryable with simple example. In which situation we can use IEnumerator?

IQueryable is an interface in C# used for querying data from a remote source like a database. Unlike IEnumerable, IQueryable supports deferred execution and query translation into the query language of the underlying data source (e.g., SQL for a database).

### Key Features of IQueryable:

- **Deferred Execution**: The query is not executed until you access the results (e.g., with ToList(), First(), or foreach).
- **Efficient Query Translation**: Queries are translated to the underlying data source's native language (e.g., SQL for Entity Framework).
- **Supports LINQ**: Works well with LINQ queries for filtering, sorting, and grouping.

### Simple Example of IQueryable:

**Scenario: Querying a Database (Mock Example)**  
```csharp
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        // Simulating a database using an array
        var data = new[] 
        { 
            new { Id = 1, Name = "Alice", Age = 30 },
            new { Id = 2, Name = "Bob", Age = 25 },
            new { Id = 3, Name = "Charlie", Age = 35 }
        };

        // Convert the array to IQueryable
        IQueryable<dynamic> queryableData = data.AsQueryable();

        // Define a query (not executed yet)
        var query = queryableData.Where(person => person.Age > 30);

        // Execute the query when accessing the results
        foreach (var person in query)
        {
            Console.WriteLine($"{person.Name} is {person.Age} years old.");
        }
    }
}
```

**Output:**  
Charlie is 35 years old.

### Key Points:

- The `.Where(person => person.Age > 30)` query is not executed immediately.
- Execution happens when you start iterating with `foreach`.

### When to Use IQueryable

**Working with Databases:**  
Use IQueryable when querying data from Entity Framework, LINQ to SQL, or other ORMs.  
Example: Querying a database table with LINQ in Entity Framework:  
```csharp
using (var context = new MyDbContext())
{
    IQueryable<Employee> employees = context.Employees.Where(e => e.Salary > 50000);
    var result = employees.ToList(); // Executes the query in SQL
}
```

**Large Data Sources:**  
Ideal for working with large datasets where filtering should happen on the server-side instead of loading all data into memory.

**Deferred Execution:**  
When you want to construct a query dynamically and execute it later.

### When to Use IEnumerator

**Working with Databases:**  

Use IEnumerator in scenarios where you need manual control over iteration. 

Examples:

Custom Iteration: Define how a custom collection is traversed.

Lazy Loading: Generate items one at a time (e.g., generating Fibonacci numbers).

Coroutines: In game development, IEnumerator is used to pause/resume code execution.

IEnumerator is not typically used for querying data. Instead, use it for iterating collections or custom workflows, while IQueryable is used for data querying.

## Explain Value Type and Reference Type

Explain Value type and reference type? Memory allocation for both.  

### Value Type and Reference Type: Explanation

1. **Value Type**  
   Definition: Stores the actual value directly.  
   Examples: int, float, bool, struct, enum.  
   Memory Allocation: Stored on the stack.  
   The stack is a small, fast memory area used for temporary storage.  
   Behavior:  
   - When copied, a new copy of the value is created.  
   - Changes to one copy do not affect the other.  

   Example:  
   ```csharp
   int a = 5;  // a stores 5
   int b = a;  // b gets a copy of 5
   b = 10;     // Changing b doesn't affect a
   Console.WriteLine(a);  // Output: 5
   ```

2. **Reference Type**  
   Definition: Stores a reference (or address) to the value, not the value itself.  
   Examples: class, array, string, object, delegate.  
   Memory Allocation: Stored on the heap, with the reference stored on the stack.  
   The heap is a larger, slower memory area used for long-lived objects.  
   Behavior:  
   - When copied, the reference is copied, not the actual value.  
   - Changes to one reference affect all references pointing to the same object.  

   Example:  
   ```csharp
   int[] arr1 = { 1, 2, 3 };  // arr1 points to an array in the heap
   int[] arr2 = arr1;         // arr2 points to the same array
   arr2[0] = 10;              // Modifying arr2 affects arr1
   Console.WriteLine(arr1[0]); // Output: 10
   ```

### Memory Allocation Summary

| **Aspect**        | **Value Type**     | **Reference Type**              |
| ----------------- | ------------------ | ------------------------------- |
| **Stored On**     | Stack              | Heap (value), Stack (reference) |
| **Copy Behavior** | Creates a new copy | Copies the reference            |
| **Lifetime**      | Temporary          | Longer-lived                    |

### Key Takeaway

- Use value types for small, simple data (e.g., numbers, structs).
- Use reference types for complex objects (e.g., arrays, classes).

## Memory Allocation for int Property in a Class

If we have a class with an int property, then how is memory allocated for the int property? 

When a class has an int property, memory allocation depends on whether the class instance is created (reference type) and where it resides. Let’s break it down:

### Memory Allocation for a Class with an int Property

1. **Class Definition**  
   Classes are reference types, meaning their objects are stored on the heap, and a reference to the object is stored on the stack.  
   ```csharp
   class MyClass
   {
       public int MyProperty { get; set; }
   }
   ```  
   Here:  
   - MyClass is a reference type.  
   - MyProperty is a value type (int).

2. **When an Instance of the Class is Created**  
   ```csharp
   MyClass obj = new MyClass();
   obj.MyProperty = 42;
   ```  
   - **Heap**: The actual memory for the MyClass object, including its MyProperty field (value type int), is allocated on the heap.  
   - **Stack**: A reference to the obj instance is stored on the stack.  

   So:  
   - The int property (MyProperty) is stored within the object on the heap.  
   - The object reference (obj) is stored on the stack.

### Memory Breakdown

| **Aspect**            | **Location** | **Details**                                |
| --------------------- | ------------ | ------------------------------------------ |
| **Class Instance**    | Heap         | Contains memory for `MyProperty`.          |
| **Property Value**    | Heap         | Stored as part of the object memory.       |
| **Reference (`obj`)** | Stack        | Points to the memory location on the heap. |

### Illustration

**Stack:**  
  obj -> Reference to the object on the heap  

**Heap:**  
  MyClass object:  
    MyProperty = 42

### Key Points

- A value type inside a reference type (like int inside a class) is stored on the heap as part of the reference type object.
- The reference to the class object is stored on the stack.

## Additional Details on Memory Allocation

1. **Object Layout on the Heap**  
   When a class contains a value type property (e.g., int), the value is stored inline within the object on the heap.  
   The exact memory layout of an object depends on factors like the runtime environment (CLR) and alignment rules. For example:  
   ```csharp
   class MyClass
   {
       public int MyProperty { get; set; }
       public string Name { get; set; }
   }
   MyClass obj = new MyClass { MyProperty = 42, Name = "John" };
   ```  
   The MyProperty (an int) is directly stored in the memory allocated for obj on the heap.  
   The Name (a string) is a reference type, so only a reference is stored inline within obj, and the actual string data is in a separate location on the heap.

2. **Garbage Collection**  
   Since the class instance resides on the heap, it is subject to garbage collection.  
   The int property, being part of the object, is managed by the garbage collector as part of the object lifecycle.

3. **Boxing and Unboxing**  
   If the int property is assigned to an object or dynamic, it undergoes boxing, which allocates additional memory on the heap to store the value type.  
   Example:  
   ```csharp
   object boxedValue = obj.MyProperty; // Boxing: int (value type) is wrapped in an object.
   int unboxedValue = (int)boxedValue; // Unboxing: Extracting the value back.
   ```  
   Boxing adds overhead, so avoid unnecessary boxing/unboxing for better performance.

4. **Thread Safety**  
   If multiple threads access and modify the int property, memory contention issues can arise.  
   Use locking or Interlocked methods for thread-safe access to properties in a class.  
   Example:  
   ```csharp
   class MyClass
   {
       private int _myProperty;
       public int MyProperty
       {
           get => _myProperty;
           set => Interlocked.Exchange(ref _myProperty, value);
       }
   }
   ```

5. **Structs vs. Classes**  
   If the same property were part of a struct instead of a class:  
   The int property would be allocated on the stack when the struct is used as a local variable.  
   No reference is involved unless the struct is boxed.  
   Example:  
   ```csharp
   struct MyStruct
   {
       public int MyProperty { get; set; }
   }
   ```

## Async/Await vs TPL

Async await vs TPL. Both async/await and the Task Parallel Library (TPL) are used for handling asynchronous operations and parallelism in C#. Here's a simple comparison:

### 1. Async/Await

**What is it?**  
Async/Await is a syntax used for writing asynchronous code in a readable, linear way.  
It simplifies the handling of tasks that take time to complete (e.g., I/O operations, HTTP requests).

**How it works?**  
- `async` is used to mark a method as asynchronous, allowing it to run non-blocking operations.  
- `await` is used to pause the execution of the method until the task completes.

**Usage Example:**  
```csharp
public async Task<string> GetDataAsync()
{
    var result = await HttpClient.GetStringAsync("https://example.com");
    return result;
}
```  
Here, `GetDataAsync()` doesn't block the thread while it waits for the HTTP request to finish. Instead, the thread is free to do other work.

**When to use:**  
Ideal for I/O-bound operations (e.g., reading files, web requests).  
It improves performance by freeing up threads during waiting time.

### 2. TPL (Task Parallel Library)

**What is it?**  
TPL is a library that allows parallel and asynchronous execution of tasks, usually for CPU-bound operations (e.g., processing data in parallel).

**How it works?**  
TPL provides constructs like `Task`, `Parallel.For`, `Task.WhenAll`, etc., to run tasks concurrently, either in parallel or asynchronously.

**Usage Example:**  
```csharp
public void ProcessData()
{
    Task task1 = Task.Run(() => { /* CPU-bound task */ });
    Task task2 = Task.Run(() => { /* CPU-bound task */ });
    
    Task.WhenAll(task1, task2).Wait();
}
```  
In this case, `Task.Run()` executes both tasks concurrently, potentially using multiple threads for CPU-bound work.

**When to use:**  
Ideal for CPU-bound operations (e.g., performing calculations, data processing).  
You can parallelize work to make the best use of multi-core processors.

### Comparison Summary

| **Aspect**           | **Async/Await**                                     | **TPL (Task Parallel Library)**                   |
| -------------------- | --------------------------------------------------- | ------------------------------------------------- |
| **Primary Focus**    | I/O-bound operations (e.g., web requests, file I/O) | CPU-bound parallel tasks (e.g., data processing)  |
| **Syntax**           | Easy-to-read, sequential syntax for async code      | More complex, using `Task` and `Parallel` classes |
| **Concurrency Type** | Asynchronous, without blocking the thread           | Concurrent or parallel execution of tasks         |
| **Performance**      | Improves performance for I/O-bound operations       | Improves performance for CPU-bound operations     |

### Key Takeaway

- Use async/await for non-blocking, asynchronous operations (I/O-bound).
- Use TPL for concurrent execution and parallelism, especially for CPU-bound operations.

## Heap vs Stack Memory

Heap vs stack memory.  

### Heap vs Stack Memory: A Simple Comparison

#### 1. Stack Memory

**What is it?**  
The stack is used for storing local variables and method calls (function call stack).

**Where is it stored?**  
It is stored in a region of memory that works in a Last In, First Out (LIFO) manner.

**Memory Allocation**  
When a method is called, its local variables are pushed onto the stack. When the method finishes, those variables are popped off the stack.

**Size**  
Stack memory is usually small and limited in size.

**Access Speed**  
It’s faster than heap memory because of its simple LIFO structure.

**Example:**  
```csharp
void MyMethod()
{
    int num = 5;  // num is stored on the stack
}
```

#### 2. Heap Memory

**What is it?**  
The heap is used for storing objects and dynamic memory allocation (i.e., when you use `new` to create an object).

**Where is it stored?**  
It’s a larger region of memory used for long-lived objects that need to persist beyond the method call.

**Memory Allocation**  
When an object is created, memory is allocated on the heap, and the reference (address) to the object is stored on the stack.

**Size**  
Heap memory is larger but slower to access than the stack.

**Access Speed**  
It’s slower because of the need to manage free memory, and it involves more complex operations (allocation and garbage collection).

**Example:**  
```csharp
class MyClass
{
    public int Number;
}

MyClass obj = new MyClass();  // obj is stored on the heap, reference on the stack
```

### Key Differences Summary

| **Aspect**            | **Stack**                        | **Heap**                             |
| --------------------- | -------------------------------- | ------------------------------------ |
| **Storage**           | Local variables and method calls | Objects and dynamic memory           |
| **Size**              | Small and limited                | Large and flexible                   |
| **Access Speed**      | Fast (LIFO structure)            | Slower (more complex management)     |
| **Memory Management** | Managed automatically (push/pop) | Managed by garbage collector         |
| **Lifetime**          | Temporary (within method scope)  | Long-lived (until garbage collected) |

### Key Takeaway

- **Stack**: Fast, small, used for local variables and method calls.
- **Heap**: Slower, large, used for objects and dynamic memory.

## Call by Value vs Call by Reference

Call by value vs Call by ref.  

### Call by Value vs Call by Reference

#### 1. Call by Value

**What it is:**  
In Call by Value, a copy of the variable's value is passed to the method.

**How it works:**  
Changes made to the parameter inside the method do not affect the original variable outside the method because only a copy of the value is passed.

**Example:**  
```csharp
void ModifyValue(int x)
{
    x = 10;  // This change doesn't affect the original variable
}

int number = 5;
ModifyValue(number);
Console.WriteLine(number);  // Output: 5
```

**Use case:**  
Used for primitive data types (like int, float, etc.), where the value of the variable should not change outside the method.

#### 2. Call by Reference

**What it is:**  
In Call by Reference, the memory address (reference) of the variable is passed to the method. This means the method can directly modify the original variable.

**How it works:**  
Any changes made to the parameter inside the method will affect the original variable because both the original and the parameter refer to the same memory location.

**Example:**  
```csharp
void ModifyValue(ref int x)
{
    x = 10;  // This changes the original variable
}

int number = 5;
ModifyValue(ref number);
Console.WriteLine(number);  // Output: 10
```

**Use case:**  
Used when you want the method to modify the original variable. This is typically used with reference types or when you want the method to update multiple variables.

### Key Differences

| **Aspect**                      | **Call by Value**                          | **Call by Reference**                                   |
| ------------------------------- | ------------------------------------------ | ------------------------------------------------------- |
| **What is passed**              | A **copy** of the value                    | The **reference** (memory address) of the variable      |
| **Effect on original variable** | No effect (original variable is unchanged) | Changes the original variable                           |
| **Use case**                    | Primitive types, data integrity            | Reference types or when changes need to reflect outside |

### Summary

- **Call by Value**: Only a copy of the value is passed. Changes don't affect the original variable.
- **Call by Reference**: The reference (address) is passed, so changes affect the original variable.

## Throw vs throw ex

Explain Throw vs throw ex.  

### Throw vs throw ex

#### 1. throw

**What it is:**  
The `throw` keyword is used to raise an exception in C#. It can be used to rethrow the current exception or throw a new one.

**How it works:**  
When `throw` is used alone, it rethrows the current exception without losing its original stack trace.

**Example:**  
```csharp
try
{
    throw new InvalidOperationException("An error occurred.");
}
catch (Exception ex)
{
    Console.WriteLine("Caught exception: " + ex.Message);
    throw;  // Rethrows the current exception
}
```

**Use case:**  
It's useful when you want to rethrow the exception to propagate it further up the call stack without changing its details.

#### 2. throw ex

**What it is:**  
The `throw ex` expression rethrows the exception object explicitly. However, this causes the original stack trace to be lost and replaced by a new one.

**How it works:**  
When `throw ex` is used, the exception is rethrown, but it loses the original stack trace and the new stack trace starts from the `throw ex` statement.

**Example:**  
```csharp
try
{
    throw new InvalidOperationException("An error occurred.");
}
catch (Exception ex)
{
    Console.WriteLine("Caught exception: " + ex.Message);
    throw ex;  // Rethrows the exception, but stack trace is reset
}
```

**Use case:**  
Avoid using `throw ex`, as it causes loss of stack trace information, making debugging harder.

### Key Difference

- **throw**: Retains the original stack trace and allows rethrowing the exception without modifying it.
- **throw ex**: Rethrows the exception but loses the original stack trace, which can make it harder to trace the source of the error.

### Summary

- Use `throw` to preserve the stack trace when rethrowing exceptions.
- Avoid `throw ex` because it loses the stack trace and makes it difficult to diagnose the issue.

## Readonly vs Constant

What is different between readonly and constant.  

### Readonly vs Constant

#### 1. readonly

**What it is:**  
A `readonly` field is a field that can only be assigned a value at the time of its declaration or in the constructor of the class. Its value can change during the object's lifetime, but only during object construction.

**Key Point:**  
The value can be set dynamically (but only once during object construction).

**Example:**  
```csharp
class MyClass
{
    public readonly int x;
    
    public MyClass(int value)
    {
        x = value;  // The value can be set in the constructor
    }
}

var obj = new MyClass(10);
Console.WriteLine(obj.x);  // Output: 10
```

**Use case:**  
Used when you want a field to be initialized once but based on a value passed at runtime (e.g., constructor).

#### 2. const

**What it is:**  
A `const` field is a constant value that is assigned at the time of declaration and cannot be changed. It must be initialized with a fixed value at compile time and remains constant throughout the program.

**Key Point:**  
The value is constant and cannot change during the program's execution.

**Example:**  
```csharp
class MyClass
{
    public const int x = 10;  // The value is set at compile time and cannot be changed
}

Console.WriteLine(MyClass.x);  // Output: 10
```

**Use case:**  
Used when you have a value that must remain unchanged throughout the entire program, like mathematical constants (e.g., Math.PI).

### Key Differences

| **Aspect**     | **`readonly`**                                  | **`const`**                                 |
| -------------- | ----------------------------------------------- | ------------------------------------------- |
| **Assignment** | Can be assigned at runtime (e.g., constructor). | Must be assigned at compile-time.           |
| **Changeable** | Can change in the constructor, but not later.   | Cannot be changed once set.                 |
| **Scope**      | Can be instance-level or static.                | Must be static and accessed via class type. |
| **Value type** | Can be any type (primitive or object).          | Must be a primitive type or string.         |

### Summary

- **`readonly`**: Can be set at runtime (usually in constructor), but can't be changed after the object is created.
- **`const`**: Must be set at compile-time and cannot be changed during the program's execution.

## What is difference between FirstOrDefault and SingleOrDefault

FirstOrDefault and SingleOrDefault are methods in LINQ used to retrieve elements from a collection, but they behave differently:

**FirstOrDefault:**

- Returns the first element that matches a condition or null if no match is found.
- Useful when you expect multiple matching elements but only want the first one.

**Example:**
```csharp
var item = list.FirstOrDefault(x => x.Id == 1);
```

**SingleOrDefault:**

- Returns only one element if it matches a condition or null if none exist.
- Throws an exception if there are multiple matches.
- Useful when you expect exactly one match.

**Example:**
```csharp
var item = list.SingleOrDefault(x => x.Id == 1);
```

**Summary:** Use FirstOrDefault when multiple results are possible but only the first is needed. Use SingleOrDefault when exactly one result is expected; otherwise, it will throw an error if multiple exist.

## Why stored procedures are faster than LINQ and Entity Framework Core?

Stored procedures are often faster than LINQ and Entity Framework Core for several reasons:

- **Precompiled Execution:** Stored procedures are precompiled and cached by the database, reducing query parsing and execution time.
- **Optimized Query Plans:** Stored procedures generate optimized execution plans that the database reuses.
- **Reduced Network Traffic:** Procedures allow you to run complex logic within the database server, minimizing data transfer between the application and the database.
- **Direct Database Access:** They avoid the extra abstraction layers that LINQ and Entity Framework use, resulting in faster query execution.

While Entity Framework Core is powerful for complex applications, stored procedures are preferable for performance-critical scenarios.

## What is difference between First and FirstOrDefault explain with simple example

### Difference Between First and FirstOrDefault in C#

**Behavior When No Match is Found:**

- **First:** Throws an exception if no matching element is found.
- **FirstOrDefault:** Returns the default value (null for reference types, 0 for value types) if no match is found.

**Use Case:**

- Use First when you are sure that a matching element exists.
- Use FirstOrDefault when there is a possibility of no match and you want to handle that gracefully.

**Example:**
```csharp
using System;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

        // Using First
        try
        {
            int first = numbers.First(x => x > 10); // Throws InvalidOperationException
            Console.WriteLine($"First: {first}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"First Exception: {ex.Message}");
        }

        // Using FirstOrDefault
        int firstOrDefault = numbers.FirstOrDefault(x => x > 10); // Returns default value (0)
        Console.WriteLine($"FirstOrDefault: {firstOrDefault}");
    }
}
```

**Output:**
```
First Exception: Sequence contains no matching element
FirstOrDefault: 0
```

### Example with reference type

**Example:**
```csharp
using System;
using System.Linq;
using System.Collections.Generic;

class Program
{
    class Person
    {
        public string Name { get; set; }
    }

    static void Main()
    {
        List<Person> people = new List<Person>
        {
            new Person { Name = "Alice" },
            new Person { Name = "Bob" },
            new Person { Name = "Charlie" }
        };

        // Using First
        try
        {
            Person first = people.First(p => p.Name == "David"); // Throws InvalidOperationException
            Console.WriteLine($"First: {first.Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"First Exception: {ex.Message}");
        }

        // Using FirstOrDefault
        Person firstOrDefault = people.FirstOrDefault(p => p.Name == "David"); // Returns null
        if (firstOrDefault == null)
        {
            Console.WriteLine("FirstOrDefault: No match found");
        }
        else
        {
            Console.WriteLine($"FirstOrDefault: {firstOrDefault.Name}");
        }
    }
}
```

**Output:**
```
First Exception: Sequence contains no matching element
FirstOrDefault: No match found
```

**Key Points:**

- First throws an exception when no match is found.
- FirstOrDefault safely returns null (for reference types) if no match exists, allowing you to handle it without an exception.

## What is difference between ++i and i++ explain with simple example

The difference between ++i (pre-increment) and i++ (post-increment) lies in when the increment operation happens relative to the expression evaluation.

**Explanation with Example:**
```csharp
using System;

class Program
{
    static void Main()
    {
        int i = 5;

        // Pre-increment (++i): Increment happens first, then the value is used
        int preIncrement = ++i; // i becomes 6, and 6 is assigned to preIncrement
        Console.WriteLine($"Pre-Increment: i = {i}, preIncrement = {preIncrement}");

        // Reset i
        i = 5;

        // Post-increment (i++): Value is used first, then the increment happens
        int postIncrement = i++; // 5 is assigned to postIncrement, then i becomes 6
        Console.WriteLine($"Post-Increment: i = {i}, postIncrement = {postIncrement}");
    }
}
```

**Output:**
```
Pre-Increment: i = 6, preIncrement = 6
Post-Increment: i = 6, postIncrement = 5
```

**Key Points:**

- **Pre-Increment (++i):** Increments the value before using it in the expression.
- **Post-Increment (i++):** Uses the current value in the expression before incrementing it.



## What's New in C# 12

C# 12 introduces several new features that make coding simpler, more expressive, and efficient. Here are some key features explained in simple language with examples:

### 1. Primary Constructors

**What it is:**  
Primary constructors allow you to define constructors directly in the class or struct declaration, reducing boilerplate code.

**Simple Example:**  
```csharp
public class Person(string name, int age)
{
    public string Name { get; } = name;
    public int Age { get; } = age;
}

// Usage
var person = new Person("Alice", 30);
Console.WriteLine($"{person.Name} is {person.Age} years old.");
```

**Why useful:**  
It simplifies class definitions by combining constructor parameters with property initialization.

### 2. Collection Expressions

**What it is:**  
A new syntax for creating collections like arrays, lists, or spans in a more readable way.

**Simple Example:**  
```csharp
// Old way
int[] numbers = new int[] { 1, 2, 3 };

// New way
int[] numbers = [1, 2, 3];

// For lists
List<string> names = ["Alice", "Bob", "Charlie"];
```

**Why useful:**  
Makes creating collections shorter and more intuitive.

### 3. Inline Arrays

**What it is:**  
Allows defining fixed-size arrays directly in structs for better performance.

**Simple Example:**  
```csharp
[System.Runtime.CompilerServices.InlineArray(10)]
public struct Buffer
{
    private int _element0;
}

// Usage
var buffer = new Buffer();
buffer[0] = 42;  // Access like an array
```

**Why useful:**  
Improves performance for low-level code by avoiding heap allocations.

### 4. Default Lambda Parameters

**What it is:**  
Lambda expressions can now have default parameter values.

**Simple Example:**  
```csharp
var greet = (string name = "World") => $"Hello, {name}!";

Console.WriteLine(greet());        // Output: Hello, World!
Console.WriteLine(greet("Alice")); // Output: Hello, Alice!
```

**Why useful:**  
Makes lambdas more flexible, similar to methods.

### 5. Alias Any Type

**What it is:**  
You can create aliases for any type, not just simple ones.

**Simple Example:**  
```csharp
using MyList = System.Collections.Generic.List<int>;
using Point = (int X, int Y);

// Usage
MyList numbers = new MyList { 1, 2, 3 };
Point p = (10, 20);
```

**Why useful:**  
Simplifies complex type names and improves code readability.

### Key Takeaway

C# 12 focuses on reducing code verbosity and improving performance. Features like primary constructors and collection expressions make everyday coding easier, while inline arrays and other features help with high-performance scenarios. For interviews, mention that C# 12 builds on previous versions to make C# more modern and efficient.

