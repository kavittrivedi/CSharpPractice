# CSharpPractice

## Explain Encapsulation vs Modularisation

Explain Encapsulation vs Modularisation.  

### 1. Encapsulation

**What it is:**  
Encapsulation is the concept of hiding implementation details and exposing only what is necessary through a controlled interface, like properties or methods. It ensures data security and control over how the data is accessed or modified.

**Key Idea:**  
Hide data and provide access through public methods or properties.

**Example:**  
```csharp
class Employee
{
    private int salary;  // Private field

    public int Salary   // Public property
    {
        get { return salary; }
        set { if (value > 0) salary = value; }  // Control how the value is set
    }
}

var emp = new Employee();
emp.Salary = 5000;  // Access through property
Console.WriteLine(emp.Salary);  // Output: 5000
```

**Use case:**  
Used to protect data and provide controlled access to it.

### 2. Modularization

**What it is:**  
Modularization is the practice of dividing a large system into smaller, self-contained units or modules. Each module performs a specific task and works independently, making the system easier to understand, test, and maintain.

**Key Idea:**  
Break the system into smaller pieces for better organization and reusability.

**Example:**  
```csharp
// Module 1: Handles Employee Data
class EmployeeManager { ... }

// Module 2: Handles Payroll
class PayrollManager { ... }

// Module 3: Handles Reporting
class ReportGenerator { ... }
```

**Use case:**  
Used to make code easier to maintain, reuse, and scale.

### Key Differences

| **Aspect**         | **Encapsulation**                                            | **Modularization**                                            |
| ------------------ | ------------------------------------------------------------ | ------------------------------------------------------------- |
| **Focus**          | Focuses on **hiding data** and controlling access.           | Focuses on **dividing the system** into modules.              |
| **Purpose**        | Ensures **data protection** and consistent access.           | Ensures **organization**, reusability, and scalability.       |
| **Implementation** | Achieved using private fields and public methods/properties. | Achieved by separating code into distinct classes or modules. |
| **Scope**          | Applies at the **class level**.                              | Applies at the **system level**.                              |

### Summary

- **Encapsulation**: Hides the internal details of a class and controls how data is accessed or modified.
- **Modularization**: Breaks the system into smaller, independent units to improve organization and maintainability.