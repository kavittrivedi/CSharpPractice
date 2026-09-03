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