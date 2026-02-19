using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class Animal
    {
        public void Eat()
        {
            Console.WriteLine("Eating...");
        }
    }

    public class Dog : Animal
    {
        public void Bark()
        {
            Console.WriteLine("Barking...");
        }
    }

class Program
{
    static void Main()
    {
        Dog dog = new Dog();
        dog.Eat(); // Inherited method
        dog.Bark(); // Dog's own method
    }
}


