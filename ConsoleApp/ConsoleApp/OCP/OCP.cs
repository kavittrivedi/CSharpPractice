using ConsoleApp.OCPExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.OCP
{

    //Let's say you are working on an e-commerce system, and you have a Product class that represents various products. Initially,
    //you have product types like Book, Electronic, and Clothing. Each of these product types has its own implementation of a
    //CalculateDiscount method.
    class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual decimal CalculateDiscount()
        {
            // Default discount calculation logic
            return Price * 0.1M; // 10% discount by default
        }
    }

    class Book : Product
    {
        public override decimal CalculateDiscount()
        {
            // Calculate discount for books
            return Price * 0.15M; // 15% discount for books
        }
    }

    class Electronic : Product
    {
        public override decimal CalculateDiscount()
        {
            // Calculate discount for electronics
            return Price * 0.05M; // 5% discount for electronics
        }
    }

    class Clothing : Product
    {
        public override decimal CalculateDiscount()
        {
            // Calculate discount for clothing
            return Price * 0.2M; // 20% discount for clothing
        }
    }

    //This initial design adheres to the OCP because you can easily add new product types(e.g., Food, Furniture) by creating new classes
    //that inherit from Product and override the CalculateDiscount method.You don't need to modify the existing Product class to extend its functionality.
}
