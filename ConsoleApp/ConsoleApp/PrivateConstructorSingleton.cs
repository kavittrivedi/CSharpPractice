//5.Private constructor
//Prevents external instantiation (e.g., singleton or static class).
class PrivateConstructorSingleton
{
    private PrivateConstructorSingleton() { }
    public static PrivateConstructorSingleton Instance { get; } = new PrivateConstructorSingleton();
}