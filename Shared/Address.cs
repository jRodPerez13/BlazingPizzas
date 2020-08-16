using System;
using System.Collections.Generic;
using System.Text;

namespace BlazingPizza.Shared
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Postalcode { get; set; }
    }
}
