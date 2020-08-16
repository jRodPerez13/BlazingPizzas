using System;
using System.Collections.Generic;
using System.Text;

namespace BlazingPizza.Shared
{
    public class Topping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string GetFormattedPrice() => Price.ToString("0.00");
    }
}
