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