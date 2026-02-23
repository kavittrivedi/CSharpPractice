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