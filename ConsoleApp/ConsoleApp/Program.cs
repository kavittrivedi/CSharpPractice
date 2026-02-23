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