using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazingPizza.Shared;

namespace BlazingPizza.Server.Models
{
    public class PizzaStoreContext : DbContext
    {
        public DbSet<PizzaSpecial> Specials { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Order> Orders { get; set; }


        public PizzaStoreContext(
            DbContextOptions<PizzaStoreContext>options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definir la llave primaria de la entidad PizzaTopping
            modelBuilder.Entity<PizzaTopping>()
                .HasKey(pst => new { pst.PizzaId, pst.ToppingId });

            // Una Pizza puede tener muchos Toppings.
            modelBuilder.Entity<PizzaTopping>()
                .HasOne<Pizza>().WithMany(ps => ps.Toppings);

            // Un Topping puede estar en muchas Pizzas.
            modelBuilder.Entity<PizzaTopping>()
                .HasOne(pst => pst.Topping).WithMany();

            /*Definir LatLong como un Owned Entity Type de Order.
            Con esto las propiedades de la entidad LatLong se crearán en la tabla Order
            en lugar de crear una nueva tabla y una llave foránea para relacionarlas.*/
            modelBuilder.Entity<Order>()
                .OwnsOne(o => o.DeliveryLocation);
        }
    }
}
