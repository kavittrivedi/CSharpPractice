using System.Data;

public static class MathHelpers
{
    public static int Add(int a, int b) => a + b;
    public static double Pi => 3.141592653589793;
}

public static class Person
{
   public static string name;
    static Person()
    {
        name = "John Doe";
    }

    public static string GetName()
    {
        return name;
    }
}