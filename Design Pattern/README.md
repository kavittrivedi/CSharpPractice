# CSharpPractice

## How many types of design patterns are there?

Design patterns fall into three categories. Creational patterns focus on how objects are created. Structural patterns focus on how objects are organized and connected. Behavioral patterns focus on how objects communicate and share responsibilities. 

A simple way to remember is: Create, Connect, Communicate. 

Design patterns are generally categorized into three main types, each serving different purposes in software design:

Create (Creational) → Connect (Structural) → Communicate (Behavioral).

### 1. **Creational Patterns**:

These patterns deal with object creation mechanisms, allowing for greater flexibility and reuse of existing code. They help manage the process of creating objects in a manner suitable to the situation.

Creational Patterns → “Creational patterns focus on how objects are created.”

**Common Creational Patterns**:

* **Singleton**: Ensures a class has only one instance and provides a global point of access to it.
* **Factory Method**: Defines an interface for creating an object but lets subclasses alter the type of objects that will be created.
* **Abstract Factory**: Provides an interface for creating families of related or dependent objects without specifying their concrete classes.
* **Builder**: Separates the construction of a complex object from its representation, allowing the same construction process to create different representations.
* **Prototype**: Creates new objects by copying an existing object, known as the prototype.

### 2. **Structural Patterns**:

These patterns focus on how classes and objects are composed to form larger structures. They help ensure that if one part of a system changes, the entire system doesn't need to change.

Structural Patterns → “Structural patterns focus on how objects are organized and combined.”

**Common Structural Patterns**:

* **Adapter**: Allows incompatible interfaces to work together by converting the interface of a class into another interface clients expect.
* **Decorator**: Attaches additional responsibilities to an object dynamically, providing a flexible alternative to subclassing for extending functionality.
* **Facade**: Provides a simplified interface to a complex subsystem, making it easier to use.
* **Composite**: Composes objects into tree structures to represent part-whole hierarchies, allowing clients to treat individual objects and compositions uniformly.
* **Proxy**: Provides a surrogate or placeholder for another object to control access to it.

### 3. **Behavioral Patterns**:

These patterns are concerned with algorithms and the assignment of responsibilities between objects. They help improve communication between objects.

"Behavioral patterns focus on how objects talk to each other and share responsibilities."

Just think “Behavioral = communication + responsibility.”

**Common Behavioral Patterns**:

* **Observer**: Defines a one-to-many dependency between objects, so when one object changes state, all its dependents are notified and updated automatically.
* **Strategy**: Defines a family of algorithms, encapsulates each one, and makes them interchangeable. It lets the algorithm vary independently from clients that use it.
* **Command**: Encapsulates a request as an object, thereby allowing for parameterization of clients with queues, requests, and operations.
* **Iterator**: Provides a way to access the elements of an aggregate object sequentially without exposing its underlying representation.
* **State**: Allows an object to alter its behavior when its internal state changes, appearing as if it changed its class.
* **Template Method**: Defines the skeleton of an algorithm in a method, deferring some steps to subclasses.

### Summary:

In summary, there are three main types of design patterns: **Creational**, **Structural**, and **Behavioral**. Each type serves a specific purpose and helps solve various design problems in software development, improving code maintainability, readability, and flexibility.

## Explain composite design pattern in simple language. 

The **Composite Design Pattern** is a structural pattern used to treat individual objects and compositions of objects uniformly. It allows you to build complex objects made of multiple smaller objects, where each object (both individual and composite) can be treated the same way.

### Example:

Imagine a company with employees. Some are managers (composites) with their own team (other employees), and others are regular workers. The pattern lets you treat both individual employees and teams as the same "Employee" type when performing actions like calculating salaries.

### Real-world Use:

File systems, where files and folders (which contain other files) are handled similarly.
Here’s a simple example of the **Composite Design Pattern** in C#:

```csharp
using System;
using System.Collections.Generic;

// Component
public abstract class Employee
{
    public abstract void ShowDetails();
}

// Leaf
public class Developer : Employee
{
    private string name;
    public Developer(string name)
    {
        this.name = name;
    }

    public override void ShowDetails()
    {
        Console.WriteLine("Developer: " + name);
    }
}

// Composite
public class Manager : Employee
{
    private List<Employee> subordinates = new List<Employee>();

    public void AddSubordinate(Employee employee)
    {
        subordinates.Add(employee);
    }

    public override void ShowDetails()
    {
        Console.WriteLine("Manager");
        foreach (var employee in subordinates)
        {
            employee.ShowDetails();
        }
    }
}

class Program
{
    static void Main()
    {
        Employee dev1 = new Developer("Alice");
        Employee dev2 = new Developer("Bob");
        Manager manager = new Manager();
        manager.AddSubordinate(dev1);
        manager.AddSubordinate(dev2);
        
        manager.ShowDetails();
    }
}
```

### Explanation:

* **Employee**: Abstract base class.
* **Developer**: Represents a leaf node (individual object).
* **Manager**: A composite that can have multiple subordinates.
* The `ShowDetails` method can be called on both individual employees and the manager, treating both uniformly.

## Can we implement Composite Design Pattern for workspace booking application that allows you to book workspaces of different locations?

Yes, the Composite Design Pattern can be implemented in a workspace booking application that allows you to book workspaces at different locations.

### How It Fits

The Composite Design Pattern is ideal for representing hierarchical structures, such as:

- Individual Workspaces (e.g., desks or rooms).
- Groups of Workspaces (e.g., floors or areas).
- Locations (e.g., offices in different cities).

### Implementation Example

**Classes:**

- **Component (Abstract Class or Interface):** Defines common operations like `Book()` for both individual and composite entities.
- **Leaf (Individual Workspace):** Represents a single workspace.
- **Composite (Group or Location):** Manages a collection of workspaces or other groups.

**C# Code:**
```csharp
using System;
using System.Collections.Generic;

// Component
interface IWorkspace
{
    void Book();
}

// Leaf
class IndividualWorkspace : IWorkspace
{
    public string Name { get; set; }

    public IndividualWorkspace(string name)
    {
        Name = name;
    }

    public void Book()
    {
        Console.WriteLine($"Workspace {Name} booked.");
    }
}

// Composite
class WorkspaceGroup : IWorkspace
{
    public string GroupName { get; set; }
    private List<IWorkspace> workspaces = new();

    public WorkspaceGroup(string groupName)
    {
        GroupName = groupName;
    }

    public void Add(IWorkspace workspace)
    {
        workspaces.Add(workspace);
    }

    public void Remove(IWorkspace workspace)
    {
        workspaces.Remove(workspace);
    }

    public void Book()
    {
        Console.WriteLine($"Booking all workspaces in group: {GroupName}");
        foreach (var workspace in workspaces)
        {
            workspace.Book();
        }
    }
}

// Client
class Program
{
    static void Main(string[] args)
    {
        // Individual workspaces
        var desk1 = new IndividualWorkspace("Desk 1");
        var desk2 = new IndividualWorkspace("Desk 2");

        // Group of workspaces
        var floor1 = new WorkspaceGroup("Floor 1");
        floor1.Add(desk1);
        floor1.Add(desk2);

        // Composite with another group
        var office = new WorkspaceGroup("Office Location");
        office.Add(floor1);
        office.Add(new IndividualWorkspace("Meeting Room"));

        // Booking all
        office.Book();
    }
}
```

**Output:**
```
Booking all workspaces in group: Office Location
Booking all workspaces in group: Floor 1
Workspace Desk 1 booked.
Workspace Desk 2 booked.
Workspace Meeting Room booked.
```

## How is the security related to scaling?

Security and scaling are interconnected. As systems scale, ensuring security becomes more challenging due to the increased number of users, devices, and potential attack vectors. To maintain security at scale, strategies like **load balancing**, **distributed firewalls**, **rate limiting**, **encryption**, and **securing APIs** are essential. Moreover, scaling can sometimes expose weaknesses or vulnerabilities, so continuous monitoring, authentication, and implementing strong access controls are critical to protect larger and more complex systems.



## Why Cannot We Create an Instance of a Static Class in C#?

We cannot create an instance of a static class in C# because static classes are designed to hold static members only, and they are meant to be accessed directly by the class name without creating an instance. Here's why:

### Key Reasons

- **No Instance Members:**  
  A static class can only have static members (methods, fields, properties, etc.). Static members belong to the class itself, not to any object or instance. Since a static class doesn't have instance members, there is no need to create an instance of the class.

- **Single Copy in Memory:**  
  A static class is loaded into memory only once, and all static members are shared across the entire application. Creating an instance would contradict the concept of having only one copy of its members.

- **Design Purpose:**  
  Static classes are intended for utility or helper methods (like Math functions or Configuration settings), where you don't need multiple copies of the class or any state management (like instance variables).

- **Compiler Restriction:**  
  The C# compiler does not allow instantiation of a static class. If you try to create an instance of a static class, the compiler will throw an error.

**Example:**
```csharp
public static class MathHelper
{
    public static int Add(int a, int b)
    {
        return a + b;
    }
}

// Access the static method without creating an instance
int sum = MathHelper.Add(5, 3);
```

In this example, you access the Add method directly using the class name (`MathHelper.Add`), and there's no need to create an object of MathHelper.

**Summary:**  
Static classes can only contain static members, and they are designed to be accessed without creating an instance. Allowing instantiation would go against their purpose and design, which is why you can't create an instance of them.

## Inversion of Control (IoC)

**Inversion of Control (IoC)** is a design principle used in software development to achieve **loose coupling** between components and enhance the flexibility and maintainability of code. It refers to the reversal of the flow of control in a program. Instead of a component controlling the flow of execution, control is inverted and managed by an external entity or framework.

### Key Concepts of IoC:

1. **Dependency Management**:

   * IoC allows for the injection of dependencies rather than having components create their own dependencies. This means that a class will receive its dependencies from an external source, typically through constructor injection, method injection, or property injection.

2. **Loose Coupling**:

   * By relying on abstractions (like interfaces) rather than concrete implementations, classes become less dependent on one another. This promotes a more modular and testable codebase.

3. **Separation of Concerns**:

   * IoC helps to separate the concerns of different parts of an application. For example, a class responsible for business logic does not need to know about data access details; it can rely on an interface to handle that responsibility.

### Common IoC Implementations:

1. **Dependency Injection (DI)**:

   * DI is a specific form of IoC where dependencies are provided (injected) to a class, rather than the class creating its own dependencies. This can be done through:

     * **Constructor Injection**: Dependencies are provided through the constructor.
     * **Property Injection**: Dependencies are set through public properties.
     * **Method Injection**: Dependencies are passed as parameters to methods.

   ```csharp
   public interface IRepository { /* ... */ }
   public class Repository : IRepository { /* ... */ }

   public class Service
   {
       private readonly IRepository _repository;

       // Constructor Injection
       public Service(IRepository repository)
       {
           _repository = repository;
       }
   }
   ```

2. **Service Locator Pattern**:

   * In this pattern, a central registry (service locator) is used to provide dependencies. Classes request their dependencies from the service locator, which can lead to hidden dependencies and is generally considered less favorable compared to DI.

### Benefits of IoC:

* **Testability**: Classes can be easily tested in isolation by injecting mock dependencies.
* **Flexibility**: Implementations can be swapped easily, allowing for different behaviors without modifying the dependent classes.
* **Maintainability**: Changes in dependencies do not require changes in the classes that use them, reducing the impact of changes.

### Summary:

**Inversion of Control (IoC)** is a design principle that allows for better dependency management, promoting loose coupling and separation of concerns in software development. The most common implementation of IoC is **Dependency Injection (DI)**, where dependencies are injected into a class rather than being created within it. This leads to more modular, maintainable, and testable code.


## Explain Strategy design pattern in simple language

The **Strategy Design Pattern** is a behavioral design pattern that enables selecting an algorithm's behavior at runtime. Instead of having a single implementation for an algorithm, the Strategy Pattern allows you to define a family of algorithms, encapsulate each one, and make them interchangeable. This means that the client can choose which algorithm to use without changing the code that uses the algorithm.

### Key Concepts:

1. **Strategy Interface**: An interface that defines a method for the strategy.
2. **Concrete Strategies**: Different implementations of the strategy interface that represent various algorithms or behaviors.
3. **Context**: The class that uses the strategy interface to call the algorithm.

### Example:

Let's say we have a simple program that sorts an array. We can use the Strategy Pattern to define different sorting strategies.

#### Step 1: Define the Strategy Interface

```csharp
public interface ISortStrategy
{
    void Sort(int[] array);
}
```

#### Step 2: Implement Concrete Strategies

```csharp
public class BubbleSort : ISortStrategy
{
    public void Sort(int[] array)
    {
        // Implementation of Bubble Sort
        Console.WriteLine("Sorting using Bubble Sort");
        // Sorting logic...
    }
}

public class QuickSort : ISortStrategy
{
    public void Sort(int[] array)
    {
        // Implementation of Quick Sort
        Console.WriteLine("Sorting using Quick Sort");
        // Sorting logic...
    }
}
```

#### Step 3: Create the Context

```csharp
public class SortContext
{
    private ISortStrategy _sortStrategy;

    // Constructor to set the strategy
    public SortContext(ISortStrategy sortStrategy)
    {
        _sortStrategy = sortStrategy;
    }

    public void SetStrategy(ISortStrategy sortStrategy)
    {
        _sortStrategy = sortStrategy; // Allows changing strategy at runtime
    }

    public void SortArray(int[] array)
    {
        _sortStrategy.Sort(array); // Calls the current strategy
    }
}
```

#### Step 4: Use the Strategy Pattern

```csharp
class Program
{
    static void Main(string[] args)
    {
        int[] array = { 5, 3, 8, 1, 2 };

        // Using Bubble Sort
        SortContext context = new SortContext(new BubbleSort());
        context.SortArray(array);

        // Changing to Quick Sort
        context.SetStrategy(new QuickSort());
        context.SortArray(array);
    }
}
```

### Benefits of Strategy Pattern:

1. **Flexibility**: You can change the algorithm used by the context at runtime.
2. **Separation of Concerns**: The sorting algorithms are separate from the context, making it easier to manage and extend.
3. **Reusability**: Different strategies can be reused in different contexts.

### Summary:

The **Strategy Design Pattern** allows you to define a family of algorithms, encapsulate each one, and make them interchangeable. It promotes flexibility and reusability by allowing clients to choose which algorithm to use without modifying the code that uses it.

## Explain Singleton Design Pattern in Simple Language

The **Singleton Design Pattern** is a creational design pattern that ensures a class has only one instance and provides a global point of access to that instance. This is useful when you want to control access to shared resources, like configuration settings, logging, or database connections.

### Key Characteristics

1. **Single Instance**: The Singleton pattern restricts the instantiation of a class to one single instance. This means that every time you need the instance, you will get the same object.

2. **Global Access**: It provides a way to access the instance from anywhere in your application without having to pass the instance around.

3. **Lazy Initialization**: Often, the instance is created the first time it is needed, which can improve performance if the instance might not be needed right away.

### How It Works

Here's a simple breakdown of how the Singleton pattern is typically implemented:

1. **Private Constructor**: The class constructor is made private to prevent external instantiation.

2. **Static Variable**: A static variable holds the single instance of the class.

3. **Static Method**: A static method (often called `GetInstance` or similar) is provided to access the instance. If the instance does not exist yet, it creates one.

### Example

Here's a simple example of a Singleton class in C#:

```csharp
public class Singleton
{
    // Static variable that holds the single instance of the class
    private static Singleton _instance;

    // Private constructor to prevent instantiation from outside
    private Singleton()
    {
    }

    // Public method to get the instance of the class
    public static Singleton GetInstance()
    {
        // Create the instance if it doesn't exist
        if (_instance == null)
        {
            _instance = new Singleton();
        }
        return _instance;
    }

    // Example method
    public void ShowMessage()
    {
        Console.WriteLine("Hello from the Singleton!");
    }
}
```

### Usage

You can use the Singleton like this:

```csharp
class Program
{
    static void Main(string[] args)
    {
        // Get the single instance of Singleton
        Singleton singleton = Singleton.GetInstance();
        singleton.ShowMessage();

        // Another call to get the instance
        Singleton anotherSingleton = Singleton.GetInstance();
        Console.WriteLine(ReferenceEquals(singleton, anotherSingleton)); // Outputs: True
    }
}
```

### Benefits of the Singleton Pattern

* **Controlled Access**: It restricts the instantiation to one instance, controlling access to the resource.
* **Shared State**: Useful for maintaining a global state or shared resource.
* **Easier Testing**: Since there's a single instance, it can simplify testing and reduce resource usage.

### Drawbacks

* **Global State**: Can introduce global state into an application, making it harder to understand and test.
* **Thread Safety**: In multi-threaded applications, ensuring thread safety can complicate the implementation.
* **Hidden Dependencies**: Classes that depend on the singleton can become tightly coupled, making it harder to manage dependencies.

### Summary

The Singleton Design Pattern is a simple yet effective way to ensure that a class has only one instance while providing a global access point. It's particularly useful for managing shared resources, but care should be taken to avoid potential drawbacks related to global state and testing.

## Interview Question: What is aggregation?

**Aggregation** in object-oriented programming is a relationship between two classes where one class (the whole) contains or is composed of objects of another class (the parts). It represents a "has-a" relationship, where the lifecycle of the "part" can be independent of the "whole."

### Example:

A `Car` class can have an aggregation relationship with the `Engine` class:

```csharp
class Car
{
    public Engine Engine { get; set; }
}

class Engine
{
    // Engine properties
}
```

Here, the `Car` contains an `Engine`, but the `Engine` can exist without the `Car`.

