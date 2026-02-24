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