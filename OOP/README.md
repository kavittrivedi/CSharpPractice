# Object-Oriented Programming (OOP) in C#

Object-Oriented Programming, or OOP, is a programming style where we build software using **objects**.

An object represents a real-world thing or concept. For example, `Car`, `Student`, `Employee`, `BankAccount`, and `Product` can all be objects in a C# program.

OOP helps us write code that is easier to understand, reuse, test, and maintain.

## Class and Object

### Class

A class is a blueprint or template. It defines what data and behavior an object will have.

```csharp
class Student
{
    public string Name { get; set; }
    public int Age { get; set; }

    public void DisplayInfo()
    {
        Console.WriteLine($"Name: {Name}, Age: {Age}");
    }
}
```

Here, `Student` is a class. It has:

- Properties: `Name`, `Age`
- Method: `DisplayInfo()`

### Object

An object is an actual instance of a class.

```csharp
Student student1 = new Student();
student1.Name = "Rahul";
student1.Age = 22;
student1.DisplayInfo();
```

Here, `student1` is an object of the `Student` class.

## Main Concepts of OOP

There are four main concepts of OOP:

1. Encapsulation
2. Abstraction
3. Inheritance
4. Polymorphism

## 1. Encapsulation

Encapsulation means wrapping data and methods together inside a class.

It also means protecting data from direct access. We usually keep fields private and expose them through public properties or methods.

```csharp
class BankAccount
{
    private decimal balance;

    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            balance += amount;
        }
    }

    public decimal GetBalance()
    {
        return balance;
    }
}
```

Usage:

```csharp
BankAccount account = new BankAccount();
account.Deposit(1000);
Console.WriteLine(account.GetBalance());
```

Here, `balance` is private. It cannot be changed directly from outside the class. This protects the data.

Simple meaning: **Encapsulation is data hiding and controlled access.**

## 2. Abstraction

Abstraction means showing only important details and hiding internal implementation.

For example, when you drive a car, you use the steering wheel, brake, and accelerator. You do not need to know all internal engine details.

In C#, abstraction can be achieved using:

- Abstract classes
- Interfaces

### Example Using Abstract Class

```csharp
abstract class Shape
{
    public abstract void Draw();
}

class Circle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing a circle");
    }
}
```

Usage:

```csharp
Shape shape = new Circle();
shape.Draw();
```

Here, `Shape` tells that every shape should have a `Draw()` method, but the actual drawing logic is written in the `Circle` class.

Simple meaning: **Abstraction hides complexity and shows only what is necessary.**

## 3. Inheritance

Inheritance means one class can reuse properties and methods of another class.

The class that gives features is called the **base class** or **parent class**.

The class that receives features is called the **derived class** or **child class**.

```csharp
class Animal
{
    public void Eat()
    {
        Console.WriteLine("Animal is eating");
    }
}

class Dog : Animal
{
    public void Bark()
    {
        Console.WriteLine("Dog is barking");
    }
}
```

Usage:

```csharp
Dog dog = new Dog();
dog.Eat();
dog.Bark();
```

Here, `Dog` inherits from `Animal`, so `Dog` can use the `Eat()` method.

Simple meaning: **Inheritance allows code reuse.**

## 4. Polymorphism

Polymorphism means one thing can have many forms.

In C#, polymorphism is commonly done in two ways:

- Method overloading
- Method overriding

## Method Overloading

Method overloading means having multiple methods with the same name but different parameters.

```csharp
class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }

    public int Add(int a, int b, int c)
    {
        return a + b + c;
    }
}
```

Usage:

```csharp
Calculator calculator = new Calculator();

Console.WriteLine(calculator.Add(10, 20));
Console.WriteLine(calculator.Add(10, 20, 30));
```

Here, the same method name `Add` works with different numbers of parameters.

## Method Overriding

Method overriding means a child class gives its own implementation of a method that already exists in the parent class.

```csharp
class Animal
{
    public virtual void MakeSound()
    {
        Console.WriteLine("Animal makes a sound");
    }
}

class Dog : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine("Dog barks");
    }
}
```

Usage:

```csharp
Animal animal = new Dog();
animal.MakeSound();
```

Output:

```text
Dog barks
```

Simple meaning: **Polymorphism allows the same method name to behave differently.**

## Constructor

A constructor is a special method that runs automatically when an object is created.

```csharp
class Employee
{
    public string Name { get; set; }

    public Employee(string name)
    {
        Name = name;
    }
}
```

Usage:

```csharp
Employee employee = new Employee("Priya");
Console.WriteLine(employee.Name);
```

Here, the constructor sets the employee name when the object is created.

## Access Modifiers

Access modifiers decide where a class member can be accessed from.

| Access Modifier | Meaning |
| --- | --- |
| `public` | Can be accessed from anywhere |
| `private` | Can be accessed only inside the same class |
| `protected` | Can be accessed inside the same class and child classes |
| `internal` | Can be accessed within the same project/assembly |

Example:

```csharp
class Person
{
    public string Name { get; set; }
    private int age;
}
```

Here, `Name` can be accessed from outside the class, but `age` can be accessed only inside the `Person` class.

## Interface

An interface defines a contract. It tells what methods or properties a class must implement.

```csharp
interface IVehicle
{
    void Start();
}

class Car : IVehicle
{
    public void Start()
    {
        Console.WriteLine("Car started");
    }
}
```

Usage:

```csharp
IVehicle vehicle = new Car();
vehicle.Start();
```

Simple meaning: **An interface tells what a class should do, but not how it should do it.**

## Abstract Class vs Interface

| Abstract Class | Interface |
| --- | --- |
| Can have abstract and non-abstract methods | Mostly used to define contracts |
| A class can inherit only one abstract class | A class can implement multiple interfaces |
| Can have fields and constructors | Cannot be directly used to create objects |
| Used when classes are closely related | Used when different classes need common behavior |

## Complete Example

```csharp
using System;

class Person
{
    public string Name { get; set; }

    public Person(string name)
    {
        Name = name;
    }

    public virtual void DisplayRole()
    {
        Console.WriteLine("I am a person");
    }
}

class Teacher : Person
{
    public string Subject { get; set; }

    public Teacher(string name, string subject) : base(name)
    {
        Subject = subject;
    }

    public override void DisplayRole()
    {
        Console.WriteLine($"{Name} teaches {Subject}");
    }
}

class Program
{
    static void Main()
    {
        Person person = new Teacher("Anita", "C#");
        person.DisplayRole();
    }
}
```

Output:

```text
Anita teaches C#
```

This example shows:

- Class and object
- Inheritance
- Constructor
- Method overriding
- Polymorphism

## Summary

| OOP Concept | Simple Meaning |
| --- | --- |
| Class | Blueprint for creating objects |
| Object | Real instance of a class |
| Encapsulation | Hiding data and giving controlled access |
| Abstraction | Hiding complex details and showing only important things |
| Inheritance | Reusing code from another class |
| Polymorphism | Same method name behaving in different ways |

## Key Points to Remember

- OOP is based on classes and objects.
- A class is a blueprint; an object is created from that blueprint.
- Encapsulation protects data.
- Abstraction hides unnecessary details.
- Inheritance helps reuse code.
- Polymorphism makes code flexible.
- C# supports OOP strongly through classes, objects, interfaces, abstract classes, inheritance, and access modifiers.
