public static class Counter
{
    public static int Value;

    public static void Increment()
    {
        Value++; // not thread-safe
    }
}