using System;

public abstract class Shape
{
    public abstract double GetArea();
}

public class Circle : Shape
{
    private double radius;

    public Circle(double radius)
    {
        this.radius = radius;
    }

    public override double GetArea()
    {
        return Math.PI * radius * radius;
    }
}

class Program
{
    static void Main()
    {
        Shape shape = new Circle(5);
        Console.WriteLine(shape.GetArea()); // Output: 78.53981633974483
    }
}
