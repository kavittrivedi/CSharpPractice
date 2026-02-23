using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.LSP
{
    //This principle states that objects of a derived class should be able to replace objects of the base class without affecting the correctness of the program.
    //In other words, a subclass should adhere to the contract established by its base class.

    //Example in C#:
    //If you have a base class Bird and a derived class Penguin, the Penguin class should be able to replace instances of Bird
    //without causing issues.
    //Both should be able to fly or not fly consistently.

    class Bird
    {
        public virtual void Fly()
        {
            Console.WriteLine("Bird can fly");
        }
    }

    class Penguin : Bird
    {
        public override void Fly()
        {
            Console.WriteLine("Penguin can't fly");
        }
    }

}
