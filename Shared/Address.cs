using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BlazingPizza.Shared
{
    public class Address
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required,MaxLength(100)]
        public string Line1 { get; set; }
        [MaxLength(100)]
        public string Line2 { get; set; }
        [Required,MaxLength(50)]
        public string City { get; set; }
        [Required,MaxLength(20)]
        public string Region { get; set; }
        [Required,MaxLength(20)]
        public string Postalcode { get; set; }
    }
}
