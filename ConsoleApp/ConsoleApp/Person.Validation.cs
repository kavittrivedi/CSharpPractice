public partial class Person
{
    public bool IsValid() => !string.IsNullOrWhiteSpace(Name) && Age >= 0;
}