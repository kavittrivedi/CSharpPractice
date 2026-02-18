public partial class Person
{
    partial void OnCreated();

    public Person()
    {
        OnCreated();
    }
}