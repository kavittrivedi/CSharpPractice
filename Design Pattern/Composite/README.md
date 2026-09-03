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