namespace ConsoleApp.LSP
{
    //The Liskov Substitution Principle(LSP) states that objects of a derived class should be able to replace objects of the base class without affecting the correctness of
    //the program.Here's an example in C#:

    //Suppose you have a system that models different types of vehicles, and you have a base class called Vehicle:
    class Vehicle
    {
        public string Model { get; set; }
        public int Year { get; set; }

        public virtual string Start()
        {
            return "The vehicle starts.";
        }

        public virtual string Stop()
        {
            return "The vehicle stops.";
        }
    }

    //Now, you create two derived classes, Car and Motorcycle, which represent specific types of vehicles:
    class Car : Vehicle
    {
        public override string Start()
        {
            return "The car engine starts.";
        }

        public override string Stop()
        {
            return "The car engine stops.";
        }
    }

    class Motorcycle : Vehicle
    {
        public override string Start()
        {
            return "The motorcycle engine starts.";
        }

        public override string Stop()
        {
            return "The motorcycle engine stops.";
        }
    }

    //In this example, both Car and Motorcycle inherit from the Vehicle base class and provide their own implementations of the Start and Stop methods.

    //Now, let's say you have a method that operates on Vehicle objects without knowing their specific derived types:

    class VehicleOperator
    {
        public void OperateVehicle(Vehicle vehicle)
        {
            Console.WriteLine(vehicle.Start());
            Console.WriteLine(vehicle.Stop());
        }
    }


}
