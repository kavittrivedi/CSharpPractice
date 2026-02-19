using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Person
    {
        private string name; // Private variable

        // Public method to set the name
        public void SetName(string newName)
        {
            name = newName;
        }

        // Public method to get the name
        public string GetName()
        {
            return name;
        }
    }

    class Program
    {
        static void Main()
        {
            Person person = new Person();
            person.SetName("Alice");
            Console.WriteLine(person.GetName()); // Output: Alice
        }
    }

}
