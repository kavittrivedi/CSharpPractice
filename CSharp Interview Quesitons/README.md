# C# Interview Practice

## How foreach Loop Works

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

## Explain IQueryable with Simple Example. In which situation we can use IEnumerator?

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

## Explain Value Type and Reference Type and explain memory allocation for both. 

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

Async await vs TPL. Both async/await and the Task Parallel Library (TPL) are used for handling asynchronous operations and parallelism in C#. 

Here's a simple comparison:

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
When you’re working with multiple asynchronous tasks in .NET, think of `Task` methods like a **Project Manager**. Depending on the method you use, the manager waits for different results before moving the team forward.

Here is a breakdown of the most common methods in simple terms:

---

## 1. `Task.WhenAll`

**The "Wait for Everyone" Method**
Use this when you have multiple tasks (like downloading 5 images) and you can't proceed until **every single one** is finished.

* **Behavior:** It starts all tasks at once. It returns a single task that completes only when all the individual tasks are done.
* **Result:** If the tasks return data (e.g., `Task<int>`), `WhenAll` gives you an **array** of all the results.
* **Failure:** If one task fails, it still waits for the others to finish, but then it throws an exception containing all the errors.

## 2. `Task.WaitAll`

**The "Stop Everything" Method**
This is the **blocking** version of `WhenAll`.

* **Difference:** `WhenAll` is "asynchronous" (the thread is free to do other things while waiting). `WaitAll` is "synchronous"—it freezes the current thread until everything is done.
* **Rule of Thumb:** Use `WhenAll` with `await` in modern code. Avoid `WaitAll` in UI or Web apps as it can cause deadlocks.

---

## 3. `Task.WhenAny`

**The "First One Wins" Method**
Use this when you have multiple tasks but you only care about the **fastest** one.

* **Scenario:** You ask three different servers for the price of Gold. You only need the first answer that comes back.
* **Behavior:** It returns a task that completes as soon as **any** of the tasks in the list finish.
* **Result:** It returns the `Task` object that finished first. You then have to `await` that specific task to get its actual data.

## 4. `Task.WaitAny`

**The "Freeze for the Winner" Method**
Just like `WaitAll`, this is the **blocking** version of `WhenAny`. It stops the current thread until at least one task finishes, then returns the **index** of that task in the array.

---

## 5. `Task.Run`

**The "Offload" Method**
This isn't for managing multiple tasks, but for **starting** one. It tells the CPU: "Take this heavy piece of work and do it on a background thread so the UI/Main thread doesn't freeze."

---

### Quick Comparison Table

| Method | Returns | Blocking? | Use Case |
| --- | --- | --- | --- |
| **`WhenAll`** | `Task` or `Task<T[]>` | No (Awaitable) | Processing a batch of items. |
| **`WhenAny`** | `Task<Task>` | No (Awaitable) | Timeouts or "Fastest wins" scenarios. |
| **`WaitAll`** | `void` | **Yes** | Console apps or legacy code. |
| **`Delay`** | `Task` | No | A "smart" sleep that doesn't freeze the app. |

---

### Pro-Tip: The "Timeout" Pattern

One of the coolest ways to use `WhenAny` is to create a timeout. You "race" your real task against a `Task.Delay`:

```csharp
Task webTask = DownloadFileAsync();
Task timeoutTask = Task.Delay(5000); // 5 seconds

Task finishedTask = await Task.WhenAny(webTask, timeoutTask);

if (finishedTask == timeoutTask) {
    // It took too long!
}

```

## In this case, `Task.Run()` executes both tasks concurrently, potentially using multiple threads for CPU-bound work.

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

## Readonly vs Constan. What is different between readonly and constant.  

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

**Summary:** 

- Use FirstOrDefault when multiple results are possible but only the first is needed. 
- Use SingleOrDefault when exactly one result is expected; otherwise, it will throw an error if multiple exist.

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

## How to restrict object creation in C#?

To restrict object creation in C#, you can use the following techniques:

1. **Private Constructor**:

   * Make the constructor `private`, preventing object instantiation from outside the class.
   * Common in **singleton** patterns or static utility classes.

   ```csharp
   public class MyClass
   {
       private MyClass() { }
   }
   ```

2. **Static Class**:

   * Define the class as `static`, which cannot be instantiated.

   ```csharp
   public static class MyStaticClass { }
   ```

3. **Factory Pattern**:

   * Control object creation by using a **factory method**.

Each method is used based on design needs.

## What is different in async/await and Task.Run()?

`async/await` and `Task.Run()` are both used in asynchronous programming in C#, but they serve different purposes and have different behaviors. Here's a breakdown of their differences:

### 1. **Purpose**:

* **`async/await`**:

  * Used to simplify asynchronous programming by allowing you to write asynchronous code in a more readable, synchronous-like manner.
  * The `async` keyword is used to declare a method as asynchronous, and `await` is used to pause the execution of that method until the awaited task is complete.

* **`Task.Run()`**:

  * Used to offload work to a separate thread, particularly for CPU-bound operations. It runs a specified action or function on a thread pool thread.
  * Typically used to run blocking code asynchronously.

### 2. **Usage**:

* **`async/await`**:

  * Commonly used with I/O-bound operations, such as file access, network calls, or database queries, where the task can be awaited.

  ```csharp
  public async Task<string> GetDataAsync()
  {
      using (var client = new HttpClient())
      {
          var result = await client.GetStringAsync("https://example.com");
          return result;
      }
  }
  ```

* **`Task.Run()`**:

  * Suitable for CPU-bound tasks that need to be executed asynchronously. It creates a new task that runs on a thread pool thread.

  ```csharp
  public Task<string> ProcessDataAsync()
  {
      return Task.Run(() =>
      {
          // Simulating a CPU-bound operation
          Thread.Sleep(5000);
          return "Data processed.";
      });
  }
  ```

### 3. **Thread Management**:

* **`async/await`**:

  * It does not create new threads; instead, it allows other operations to run while waiting for the completion of an awaited task. It is more efficient in handling I/O-bound operations.

* **`Task.Run()`**:

  * It creates a new task on a separate thread from the thread pool, which can introduce overhead. It's primarily meant for CPU-bound tasks that may block for a while.

### 4. **Scalability**:

* **`async/await`**:

  * More scalable for I/O-bound tasks since it does not block threads while waiting for the completion of tasks.

* **`Task.Run()`**:

  * Can lead to thread pool exhaustion if overused for I/O-bound tasks since it utilizes additional threads, which can be less efficient.

### Summary:

* Use **`async/await`** for I/O-bound operations to improve code readability and performance without blocking threads.
* Use **`Task.Run()`** for CPU-bound operations to run them asynchronously on a separate thread but be cautious of overusing it for I/O-bound tasks.

## How to call async method in non async method without using await?

To call an asynchronous method from a non-asynchronous method without using `await`, you can use one of the following approaches. However, be cautious with these methods, as they can lead to blocking the calling thread or potential deadlocks.

### 1. **Using `Task.Result`**:

You can call the asynchronous method and get the result using `Task.Result`, which blocks the calling thread until the task completes.

```csharp
public string CallAsyncMethod()
{
    var result = MyAsyncMethod().Result; // Blocks until the async method is complete
    return result;
}

public async Task<string> MyAsyncMethod()
{
    // Simulate async work
    await Task.Delay(1000);
    return "Hello from async!";
}
```

### 2. **Using `Task.Wait()`**:

You can call the asynchronous method and wait for it to complete using `Task.Wait()`.

```csharp
public void CallAsyncMethod()
{
    var task = MyAsyncMethod(); // Starts the async method
    task.Wait(); // Blocks until the async method is complete
    var result = task.Result; // Access the result
}

public async Task<string> MyAsyncMethod()
{
    // Simulate async work
    await Task.Delay(1000);
    return "Hello from async!";
}
```

### 3. **Using `GetAwaiter().GetResult()`**:

You can also use `GetAwaiter().GetResult()` to call the asynchronous method synchronously.

```csharp
public string CallAsyncMethod()
{
    var result = MyAsyncMethod().GetAwaiter().GetResult(); // Blocks until the async method is complete
    return result;
}

public async Task<string> MyAsyncMethod()
{
    // Simulate async work
    await Task.Delay(1000);
    return "Hello from async!";
}
```

### Important Considerations:

* **Blocking**: All the methods mentioned above block the calling thread until the asynchronous operation completes. This can lead to performance issues and reduce the responsiveness of your application, especially in UI applications.
* **Deadlocks**: In certain synchronization contexts (like UI threads), using `Result` or `Wait()` can lead to deadlocks if the asynchronous method attempts to marshal back to the calling context.

### Best Practice:

If possible, refactor your code to make the calling method asynchronous, allowing you to use `await` instead. This approach enhances code readability and avoids potential blocking issues:

```csharp
public async Task<string> CallAsyncMethod()
{
    var result = await MyAsyncMethod(); // Proper async call
    return result;
}
```


However, if you must call an async method from a synchronous context, use one of the above methods with caution.

## Extension Methods in C#

**Extension Methods** in C# are a powerful feature that allows you to "add" new methods to existing types without modifying their source code or creating a new derived type. This is particularly useful for adding functionality to types for which you do not have the source code, such as types defined in the .NET Framework or third-party libraries.

### How Extension Methods Work

An extension method is defined as a static method in a static class, with the first parameter specifying the type that the method will operate on. This first parameter must be preceded by the `this` keyword, indicating that the method is an extension method.

### Defining an Extension Method

Here's how to define an extension method:

1. **Create a Static Class**: Extension methods must be defined in a static class.
2. **Define a Static Method**: The method must be static and include the `this` keyword in the first parameter.

#### Example

Let's say you want to add a method to the `string` class to count the number of vowels in a string.

```csharp
using System;

public static class StringExtensions
{
    public static int CountVowels(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return 0;

        int count = 0;
        foreach (char c in str.ToLower())
        {
            if ("aeiou".Contains(c))
            {
                count++;
            }
        }
        return count;
    }
}
```

### Using an Extension Method

Once you have defined an extension method, you can use it as if it were a regular method of the type.

```csharp
class Program
{
    static void Main(string[] args)
    {
        string myString = "Hello, World!";
        int vowelCount = myString.CountVowels(); // Using the extension method
        Console.WriteLine($"Number of vowels: {vowelCount}");
    }
}
```

### Key Points

1. **Static Class and Method**: Extension methods must be defined in a static class and declared as static methods.
2. **`this` Keyword**: The first parameter of the method must use the `this` keyword to indicate that it is an extension method for the specified type.
3. **Namespaces**: To use an extension method, the namespace containing the static class must be included in the file where you want to use it (via a `using` directive).
4. **Intellisense Support**: Extension methods show up in Intellisense, allowing developers to discover them easily when working with the type.

### Limitations

* **Cannot Override Existing Methods**: Extension methods do not override existing methods. If an instance of a type already has a method with the same name and signature, that method will take precedence.
* **Scope**: Extension methods are only available when the appropriate namespace is included.
* **Not Part of the Type**: They are not part of the actual type's definition, so they cannot access private members of the type.

### Summary

Extension methods in C# provide a way to add new functionality to existing types without modifying their code. They are defined in static classes and allow developers to extend the capabilities of classes, making code more modular and reusable. Proper use of extension methods can lead to cleaner and more readable code.

## Explain IQueryable vs IEnumerable

`IQueryable` and `IEnumerable` are two important interfaces in C# that are used for querying collections, but they have different purposes, capabilities, and performance characteristics. Here's a breakdown of the differences between the two:

### 1. **Definition**:

* **IEnumerable<T>**:

  * Represents a forward-only cursor that can be used to iterate through a collection. It is part of the `System.Collections.Generic` namespace.
  * It is typically used for in-memory collections (like arrays, lists, etc.).

* **IQueryable<T>**:

  * Represents a collection of objects that can be queried using LINQ (Language Integrated Query). It is part of the `System.Linq` namespace.
  * It is used for querying data from external sources, such as databases or web services, where the query can be translated into a query language (e.g., SQL).

### 2. **Execution**:

* **IEnumerable<T>**:

  * Executes queries in-memory and loads the entire collection into memory before querying.
  * Queries are executed when you iterate over the collection (deferred execution).
* **IQueryable<T>**:

  * Executes queries against the data source directly and can be optimized by translating the query into the appropriate format (e.g., SQL for a database).
  * Allows for more efficient querying since only the required data is fetched from the data source.

### 3. **Usage**:

* **IEnumerable<T>**:

  * Used for working with in-memory collections. It is suitable for scenarios where the entire collection is available and needs to be filtered or processed.
* **IQueryable<T>**:

  * Used for querying data from remote data sources (like databases) and supports building complex queries.
  * It allows for querying that takes advantage of the underlying data source's capabilities (e.g., SQL Server's execution plans).

### 4. **Performance**:

* **IEnumerable<T>**:

  * May lead to performance issues if the collection is large, as it loads all data into memory before processing.
  * All filtering is done in memory after the data has been retrieved.

* **IQueryable<T>**:

  * More efficient for large datasets as it can execute the query against the data source, returning only the relevant data.
  * It allows for optimizations by the underlying data source.

### 5. **LINQ**:

* **IEnumerable<T>**:

  * Supports LINQ but operates on the collection in memory.
  * Any LINQ queries performed on an `IEnumerable<T>` are executed after the entire collection is loaded.

* **IQueryable<T>**:

  * Supports LINQ and translates the LINQ queries into a format that can be executed against the data source (like SQL).
  * Queries can be composed and executed as a single command.

### Example

Here's a simple example to illustrate the differences:

#### Using `IEnumerable`:

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
IEnumerable<int> evenNumbers = numbers.Where(n => n % 2 == 0); // Filtering in memory

foreach (var number in evenNumbers)
{
    Console.WriteLine(number); // Outputs: 2, 4
}
```

#### Using `IQueryable`:

```csharp
using (var context = new YourDbContext())
{
    IQueryable<int> evenNumbers = context.Numbers.Where(n => n % 2 == 0); // Filtering in the database

    foreach (var number in evenNumbers)
    {
        Console.WriteLine(number); // Outputs even numbers from the database
    }
}
```

### Summary

In summary, `IEnumerable<T>` is best suited for in-memory collections where you want to work with data already loaded into memory, while `IQueryable<T>` is designed for querying data from external data sources efficiently, translating queries to a format the data source can understand. Choosing between them depends on the context of your application and how you intend to work with the data.

## Explain Lambda Expression

A **lambda expression** in C# is a concise way to represent an anonymous function (a function without a name) that can contain expressions and statements. Lambda expressions are particularly useful when you need to pass a small piece of functionality as an argument to methods or to create inline functions without the boilerplate of defining a separate method.

### Syntax

The syntax of a lambda expression consists of:

1. **Input Parameters**: Defined within parentheses. If there is a single parameter, parentheses can be omitted.
2. **Arrow Operator (`=>`)**: Separates the input parameters from the body of the lambda expression.
3. **Expression or Statement Block**: The body can be a single expression or a block of statements.

#### Basic Syntax

```csharp
(parameters) => expression
```

or

```csharp
(parameters) => { statements }
```

### Examples

#### 1. **Basic Lambda Expression**

Here's a simple example of a lambda expression that takes an integer and returns its square:

```csharp
Func<int, int> square = x => x * x;

int result = square(5); // result will be 25
```

In this example:

* `Func<int, int>` is a delegate that represents a function taking an `int` and returning an `int`.
* `x => x * x` is the lambda expression that computes the square of `x`.

#### 2. **Lambda with Multiple Parameters**

You can also define lambda expressions with multiple parameters:

```csharp
Func<int, int, int> add = (x, y) => x + y;

int sum = add(3, 4); // sum will be 7
```

#### 3. **Lambda with Statement Block**

If the body of the lambda expression contains more than one statement, use curly braces:

```csharp
Action<string> greet = name =>
{
    var message = $"Hello, {name}!";
    Console.WriteLine(message);
};

greet("Alice"); // Outputs: Hello, Alice!
```

### Common Uses

1. **LINQ Queries**: Lambda expressions are often used in LINQ (Language Integrated Query) to filter, select, and manipulate collections.

   ```csharp
   var numbers = new List<int> { 1, 2, 3, 4, 5 };
   var evenNumbers = numbers.Where(n => n % 2 == 0);
   ```

2. **Event Handlers**: Lambda expressions can be used to define event handlers inline.

   ```csharp
   button.Click += (sender, e) => { Console.WriteLine("Button clicked!"); };
   ```

3. **Delegates**: They provide a convenient way to create delegate instances.

   ```csharp
   Predicate<string> isLong = s => s.Length > 5;
   ```

### Benefits

* **Conciseness**: Lambda expressions allow you to write less code and improve readability, especially for short functions.
* **Inline Definition**: They can be defined inline, which helps to keep related logic together.
* **Functional Programming**: They facilitate functional programming styles, making it easier to work with collections and functional patterns.

### Summary

Lambda expressions are a powerful feature in C# that enable you to write concise and flexible code for representing anonymous functions. They are widely used in LINQ queries, event handling, and anywhere delegates are required. By providing a way to define functionality inline, lambda expressions enhance readability and maintainability of code.

## How many types of constructor we can add in c# class?

In C#, you can have several types of constructors in a class:

1. **Default Constructor**: This is parameterless and automatically provided by C# if no constructor is defined.

   ```csharp
   public class MyClass {
       public MyClass() { }
   }
   ```

2. **Parameterized Constructor**: Accepts parameters to initialize the object with specific values.

   ```csharp
   public class MyClass {
       public MyClass(string name) { }
   }
   ```

3. **Copy Constructor**: Initializes an object by copying values from another object of the same class.

   ```csharp
   public class MyClass {
       public MyClass(MyClass other) { }
   }
   ```

4. **Static Constructor**: Used to initialize static members. It is parameterless and called only once.

   ```csharp
   public class MyClass {
       static MyClass() { }
   }
   ```

5. **Private Constructor**: Used to restrict object creation, often in Singleton patterns.

   ```csharp
   public class MyClass {
       private MyClass() { }
   }
   ```

## What is diamond problem?

The **diamond problem** occurs in multiple inheritance, where a class inherits from two or more classes that have a common base class, leading to ambiguity. It’s named after the diamond-shaped structure in the inheritance hierarchy.

For example, if class `A` is the base class, and both `B` and `C` inherit from `A`, and class `D` inherits from both `B` and `C`, `D` would have two paths to `A`, causing ambiguity in accessing `A`'s methods or properties.

In C#, this problem is avoided because C# doesn’t support multiple inheritance for classes, but it supports interface inheritance, which handles this using the "explicit implementation" feature.

##  how to restrict virtual method?

To restrict a **virtual method** in C#, you can use the **`sealed`** keyword in an override of the method. This prevents any further overriding in derived classes. Here's an example:

```csharp
public class BaseClass
{
    public virtual void Show() { }
}

public class DerivedClass : BaseClass
{
    public sealed override void Show() 
    {
        // Prevents further overriding in any class derived from DerivedClass
    }
}
```

In this example, `Show` in `DerivedClass` is sealed, so no further class can override this method.

##  Explain Interface vs abstract class.

### Key Differences Between Interface and Abstract Class:

1. **Multiple Inheritance**: A class can implement multiple interfaces but can inherit only one abstract class.
2. **Implementation**: Interfaces can only have method signatures (no implementation), while abstract classes can have both method implementations and abstract methods.
3. **Fields**: Abstract classes can have fields, whereas interfaces cannot.
4. **Default Modifiers**: Interface members are public and abstract by default, while abstract class members can have different access modifiers.

### Example:

```csharp
public interface IAnimal { void Speak(); }
public abstract class Animal { public abstract void Speak(); }
```

##  What are the members of the interface?

The members of an interface in C# can include:

1. **Methods**: Define method signatures without a body.

   ```csharp
   void Speak();
   ```
2. **Properties**: Define property signatures without a body.

   ```csharp
   string Name { get; set; }
   ```
3. **Events**: Declare events that classes can subscribe to.

   ```csharp
   event EventHandler OnSpeak;
   ```
4. **Indexers**: Allow objects to be indexed like arrays.

   ```csharp
   string this[int index] { get; set; }
   ```

All members of an interface are abstract and public by default, without any implementation.

##  Can we inherit static class to another class?

No, we **cannot inherit** a static class in C#. Static classes are designed to hold only static members (methods, properties, fields) and cannot be instantiated or inherited. They exist at the class level and are not meant to participate in inheritance hierarchies.

If you need to share functionality, use static methods directly, or consider using inheritance in regular (non-static) classes. Static classes are often used as utility or helper classes that provide shared functionality across the application.

##  Explain async vs sync with simple example.

### Synchronous (Sync)

In synchronous programming, tasks are executed one after another. Each task must complete before the next one starts. This can lead to delays if one task takes a long time.

**Example**:

```csharp
public void SyncMethod()
{
    Console.WriteLine("Start Sync");
    Thread.Sleep(2000); // Simulate a delay
    Console.WriteLine("End Sync");
}
```

Here, the second message will only print after a 2-second delay.

### Asynchronous (Async)

In asynchronous programming, tasks can run concurrently. This allows other tasks to execute while waiting for a long-running task to complete, improving responsiveness.

**Example**:

```csharp
public async Task AsyncMethod()
{
    Console.WriteLine("Start Async");
    await Task.Delay(2000); // Simulate a delay
    Console.WriteLine("End Async");
}
```

In this case, the second message will print after 2 seconds, but the program can continue doing other work during that time.

Here's a simple example of an asynchronous method in a C# console application:

### Example Code

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Fetching data asynchronously...");
        var data = await FetchDataAsync("https://jsonplaceholder.typicode.com/posts/1");
        Console.WriteLine($"Data fetched: {data}");
    }

    static async Task<string> FetchDataAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync(url);
            return response;
        }
    }
}
```

### Explanation

1. **Main Method**: The `Main` method is marked as `async` and calls `FetchDataAsync` using `await`.
2. **FetchDataAsync Method**: This method fetches data from a URL asynchronously without blocking the main thread.

### Running the Example

1. Create a new Console Application in Visual Studio or your preferred IDE.
2. Replace the code in `Program.cs` with the example above.
3. Run the application to see how it fetches data asynchronously.

Here's an example of a console application in C# with multiple asynchronous methods:

### Example Code

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting data fetch...");

        var postData = await GetPostDataAsync(1);
        Console.WriteLine($"Post Data: {postData}");

        var userData = await GetUserDataAsync(1);
        Console.WriteLine($"User Data: {userData}");

        Console.WriteLine("All data fetched.");
    }

    static async Task<string> GetPostDataAsync(int postId)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync($"https://jsonplaceholder.typicode.com/posts/{postId}");
            return response;
        }
    }

    static async Task<string> GetUserDataAsync(int userId)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync($"https://jsonplaceholder.typicode.com/users/{userId}");
            return response;
        }
    }
}
```

### Explanation

1. **Main Method**: Calls `GetPostDataAsync` and `GetUserDataAsync` asynchronously, awaiting each task.
2. **GetPostDataAsync**: Fetches a post by ID.
3. **GetUserDataAsync**: Fetches user information by ID.

### Running the Example

1. Create a new Console Application in Visual Studio or your preferred IDE.
2. Replace the code in `Program.cs` with the example above.
3. Run the application to see how it fetches post and user data asynchronously.

## Explain await

The `await` keyword in C# is used to asynchronously wait for a task to complete without blocking the executing thread. When an asynchronous method is called with `await`, the method can return control to the caller until the awaited task finishes. This helps improve application responsiveness, especially in UI applications, by allowing other operations to run while waiting for potentially long-running tasks, like file I/O or network calls.

Here's a simple usage example:

```csharp
public async Task<int> GetDataAsync()
{
    int result = await SomeLongRunningOperation();
    return result;
}
```

Here are several different examples of asynchronous programming in C# using the `async` and `await` keywords:

### 1. File I/O Operations

```csharp
using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string content = await ReadFileAsync("example.txt");
        Console.WriteLine(content);
    }

    static async Task<string> ReadFileAsync(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {
            return await reader.ReadToEndAsync();
        }
    }
}
```

### 2. Asynchronous Database Call

```csharp
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string data = await FetchDataAsync("SELECT TOP 1 Name FROM Users");
        Console.WriteLine(data);
    }

    static async Task<string> FetchDataAsync(string query)
    {
        using (SqlConnection conn = new SqlConnection("YourConnectionString"))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return (string)await cmd.ExecuteScalarAsync();
            }
        }
    }
}
```

### 3. Multiple Async Calls with `Task.WhenAll`

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var tasks = new[]
        {
            GetDataAsync("https://jsonplaceholder.typicode.com/posts/1"),
            GetDataAsync("https://jsonplaceholder.typicode.com/users/1")
        };
        
        var results = await Task.WhenAll(tasks);
        foreach (var result in results)
        {
            Console.WriteLine(result);
        }
    }

    static async Task<string> GetDataAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            return await client.GetStringAsync(url);
        }
    }
}
```

### 4. Using `ConfigureAwait`

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        await Task.Delay(2000).ConfigureAwait(false); // Do not capture the context
        Console.WriteLine("Executed without capturing context.");
    }
}
```

### 5. Exception Handling in Async

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var data = await GetDataAsync("https://invalid-url");
            Console.WriteLine(data);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching data: {ex.Message}");
        }
    }

    static async Task<string> GetDataAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            return await client.GetStringAsync(url);
        }
    }
}
```

These examples cover a range of scenarios including file I/O, database calls, multiple asynchronous operations, and error handling, illustrating the versatility of async programming in C#.

Here are a few additional examples of asynchronous programming that highlight various aspects and uses of `async` and `await` in C#:

### 6. Async Method Returning `Task`

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        await PerformTaskAsync();
        Console.WriteLine("Task completed.");
    }

    static async Task PerformTaskAsync()
    {
        await Task.Delay(1000); // Simulates a long-running task
        Console.WriteLine("Task in progress...");
    }
}
```

### 7. Async Programming with Progress Reporting

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var progress = new Progress<int>(value => Console.WriteLine($"Progress: {value}%"));
        await ReportProgressAsync(progress);
    }

    static async Task ReportProgressAsync(IProgress<int> progress)
    {
        for (int i = 0; i <= 100; i += 20)
        {
            await Task.Delay(500); // Simulate work
            progress.Report(i);
        }
    }
}
```

### 8. Async Method Using Cancellation Token

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() =>
        {
            Thread.Sleep(2000); // Simulate delay before cancellation
            cancellationTokenSource.Cancel();
        });

        try
        {
            await LongRunningOperationAsync(cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was canceled.");
        }
    }

    static async Task LongRunningOperationAsync(CancellationToken cancellationToken)
    {
        for (int i = 0; i < 5; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(1000); // Simulates work
            Console.WriteLine("Working...");
        }
    }
}
```

### 9. Using Async in Event Handlers

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        await Button_ClickAsync();
    }

    static async Task Button_ClickAsync()
    {
        await Task.Delay(1000); // Simulate a click event delay
        Console.WriteLine("Button clicked, operation performed asynchronously.");
    }
}
```

### 10. Async with LINQ

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var results = await GetDataAsync();
        foreach (var item in results)
        {
            Console.WriteLine(item);
        }
    }

    static async Task<List<string>> GetDataAsync()
    {
        await Task.Delay(1000); // Simulate data fetching
        return new List<string> { "Data1", "Data2", "Data3" }.OrderBy(x => x).ToList();
    }
}
```

These additional examples demonstrate different scenarios where async programming can be effectively utilized, including task reporting, cancellation, event handling, and using LINQ. Each of these scenarios showcases the flexibility and benefits of using asynchronous programming in C#.


## Explain SOA

Service-Oriented Architecture (SOA) is a design paradigm that allows different services to communicate over a network. It promotes the development of software applications as a collection of loosely coupled, reusable services, which can be developed, deployed, and maintained independently. SOA emphasizes interoperability and integration, enabling services to be accessed and reused across various platforms. This architecture enhances flexibility and scalability, making it easier to adapt to changing business needs and integrate with other systems.

For more information, you can check resources like [IBM's explanation of SOA](https://www.ibm.com/cloud/learn/soa).

Here’s a simple example of implementing a Service-Oriented Architecture (SOA) in .NET Core 6.0 using two services: a Product Service and an Order Service.

### 1. Product Service

**ProductService.cs**

```csharp
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductServiceController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = new List<string> { "Product1", "Product2", "Product3" };
            return Ok(products);
        }
    }
}
```

### 2. Order Service

**OrderService.cs**

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderServiceController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public OrderServiceController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderProducts()
        {
            var response = await _httpClient.GetStringAsync("http://localhost:5001/api/products"); // URL of Product Service
            return Ok($"Order Products: {response}");
        }
    }
}
```

### 3. Startup Configuration

**Program.cs**

```csharp
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient
builder.Services.AddHttpClient<OrderServiceController>();

var app = builder.Build();

app.MapControllers();

app.Run();
```

### Explanation

* **Product Service** provides a simple API to return a list of products.
* **Order Service** calls the Product Service to fetch the products and return them as part of an order response.
* **HttpClient** is used for inter-service communication, showcasing how services interact in an SOA environment.

This structure allows for independent development and deployment of services, demonstrating the principles of SOA.


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

## What's New in C# 13

C# 13 introduces several features to make code more flexible, efficient, and easier to write. These are part of .NET 9. I'll explain them simply with examples so you can discuss in interviews.

### 1. Params Collections
**What it is:** You can now use `params` with any collection type, not just arrays. This includes `Span<T>`, `ReadOnlySpan<T>`, and other collections that implement `IEnumerable<T>` with an `Add` method.

**Simple Explanation:** Before, `params` only worked with arrays. Now, it works with modern collection types for better performance.

**Example:**
```csharp
public void PrintNumbers(params ReadOnlySpan<int> numbers)
{
    foreach (var num in numbers)
    {
        Console.WriteLine(num);
    }
}

// Usage
PrintNumbers(1, 2, 3, 4); // Works like before, but uses Span internally
```

**Interview Tip:** Mention this improves performance for large data by avoiding array allocations.

### 2. New Lock Object
**What it is:** A new `System.Threading.Lock` type for thread synchronization, better than the old `lock` with `Monitor`.

**Simple Explanation:** It's a modern way to lock resources in multi-threaded code, using a disposable scope.

**Example:**
```csharp
private Lock myLock = new Lock();

public void SafeMethod()
{
    using (myLock.EnterScope())
    {
        // Critical section
        Console.WriteLine("Locked!");
    } // Automatically unlocks here
}
```

**Interview Tip:** Say it provides better performance and safety compared to traditional `lock`.

### 3. New Escape Sequence
**What it is:** `\e` as a shortcut for the ESCAPE character (Unicode U+001B).

**Simple Explanation:** Instead of `\u001b`, you can use `\e` in strings.

**Example:**
```csharp
string escape = "\e"; // Same as "\u001b"
Console.WriteLine(escape + "Hello"); // Moves cursor or something in terminals
```

**Interview Tip:** Useful for console apps or text formatting.

### 4. Method Group Natural Type Improvements
**What it is:** Better overload resolution for method groups, pruning invalid candidates earlier.

**Simple Explanation:** Compiler is smarter in choosing the right method overload, especially with generics.

**Example:** (Subtle change, no direct code example needed) It helps in complex generic scenarios.

**Interview Tip:** Mention it improves compile-time performance and accuracy.

### 5. Implicit Index Access in Object Initializers
**What it is:** Use `^` (from-end index) in object initializers for arrays.

**Simple Explanation:** Initialize arrays from the end without knowing the length.

**Example:**
```csharp
int[] arr = new int[5] { [^1] = 10, [^2] = 20 }; // Sets last two elements
// arr = [0, 0, 0, 20, 10]
```

**Interview Tip:** Great for countdowns or reverse indexing.

### 6. Ref and Unsafe in Iterators and Async Methods
**What it is:** Allow `ref` locals and `unsafe` code in iterators (`yield`) and async methods.

**Simple Explanation:** You can use references and unsafe code in more places, but safely.

**Example:**
```csharp
public async IAsyncEnumerable<int> GetNumbersAsync()
{
    int value = 0;
    ref int refValue = ref value; // Allowed now
    yield return refValue;
}
```

**Interview Tip:** Enables better performance in async scenarios with spans.

### 7. Allows Ref Struct
**What it is:** New constraint `allows ref struct` for generics.

**Simple Explanation:** Generics can now accept `ref struct` types like `Span<T>`.

**Example:**
```csharp
public class Container<T> where T : allows ref struct
{
    public void Process(scoped T item) { }
}
```

**Interview Tip:** Useful for high-performance libraries.

### 8. Ref Struct Interfaces
**What it is:** `ref struct` types can implement interfaces.

**Simple Explanation:** Span-like types can have interfaces, but with safety rules.

**Example:**
```csharp
public ref struct MySpan : IEnumerable<int>
{
    // Implementation
}
```

**Interview Tip:** But can't box to interface, maintains safety.

### 9. More Partial Members
**What it is:** Partial properties and indexers.

**Simple Explanation:** Split property/indexer declarations across files.

**Example:**
```csharp
// File1.cs
partial class MyClass
{
    public partial int MyProperty { get; set; }
}

// File2.cs
partial class MyClass
{
    private int _prop;
    public partial int MyProperty { get => _prop; set => _prop = value; }
}
```

**Interview Tip:** Like partial methods, for large classes.

### 10. Overload Resolution Priority
**What it is:** Attribute to prefer certain overloads.

**Simple Explanation:** Library authors can mark better overloads.

**Example:**
```csharp
[OverloadResolutionPriority(1)]
public void Method(int x) { }

[OverloadResolutionPriority(2)]
public void Method(string x) { } // Preferred
```

**Interview Tip:** Helps evolve libraries without breaking changes.

### 11. The Field Keyword (Preview)
**What it is:** `field` keyword to access auto-property backing field.

**Simple Explanation:** In property accessors, `field` refers to the hidden field.

**Example:**
```csharp
public int MyProperty
{
    get => field; // Accesses the backing field
    set => field = value * 2; // Modifies it
}
```

**Interview Tip:** Reduces need for explicit fields, but preview feature.

### Best Practices for C# 13
- Use new features where they improve performance or readability.
- Test thoroughly, especially with preview features like `field`.
- For interviews, highlight how C# 13 enhances safety and efficiency in modern .NET apps.



