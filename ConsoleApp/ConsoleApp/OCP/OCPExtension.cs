using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.OCPExtension
{
    //Now, let's say you want to introduce a new requirement to provide a special discount for certain customer groups (e.g., "VIP" customers).
    //To maintain the OCP, you can create a new abstraction, such as an interface, to handle this additional behavior:

    interface IDiscountProvider
    {
        decimal GetDiscount();
    }

    class VIPDiscountProvider : IDiscountProvider
    {
        public decimal GetDiscount()
        {
            return 0.25M; // 25% discount for VIP customers
        }
    }

    //Now, you can modify the Product class to accept an instance of IDiscountProvider and use it to calculate discounts:

    class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        private readonly IDiscountProvider _discountProvider;

        public Product(IDiscountProvider discountProvider)
        {
            _discountProvider = discountProvider;
        }

        public decimal CalculateDiscount()
        {
            // Use the provided discount provider to calculate the discount
            decimal discount = _discountProvider.GetDiscount();
            return Price * discount;
        }
    }
}
