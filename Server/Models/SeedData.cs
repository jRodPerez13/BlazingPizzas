using BlazingPizza.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingPizza.Server.Models
{
    public static class SeedData
    {
        public static void Initialize(PizzaStoreContext context)
        {
            var Specials = new PizzaSpecial[]{

                new PizzaSpecial
                {

                    Name = "Pizza clásica de queso",

                    Description = "Es de queso y delicioso. ¿Por qué no querrías una?",

                    BasePrice = 189.99m,

                    ImageUrl = "images/pizzas/cheese.jpg"
                },
                new PizzaSpecial()
                {

                    Name = "Tocinator",

                    Description = "Tiene TODO tipo de tocino",

                    BasePrice = 227.99m,ImageUrl = "images/pizzas/bacon.jpg",
                },
                new PizzaSpecial(){

                    Name = "Clásica de pepperoni",

                    Description = "Es la pizza con la que creciste, ¡pero ardientemente deliciosa!",

                    BasePrice = 199.50m,

                    ImageUrl = "images/pizzas/pepperoni.jpg",
                },
                new PizzaSpecial(){

                    Name = "Pollo búfalo",

                    Description = "Pollo picante, salsa picante y queso azul, garantizado que entrarás en calor",

                    BasePrice = 228.75m,

                    ImageUrl = "images/pizzas/meaty.jpg",
                },
                new PizzaSpecial(){

                    Name = "Amantes del champiñón",

                    Description = "Tiene champiñones. ¿No es obvio?",

                    BasePrice = 209.00m,

                    ImageUrl = "images/pizzas/mushroom.jpg",
                },
                new PizzaSpecial(){

                    Name = "Hawaiana",

                    Description = "De piña, jamón y queso...",

                    BasePrice = 190.25m,

                    ImageUrl = "images/pizzas/hawaiian.jpg",
                },
                new PizzaSpecial(){

                    Name = "Delicia vegetariana",

                    Description = "Es como una ensalada, pero en una pizza",

                    BasePrice = 218.50m,

                    ImageUrl = "images/pizzas/salad.jpg",
                },
                new PizzaSpecial()
                {

                    Name = "Margarita",

                    Description = "Pizza italiana tradicional con tomates y albahaca",

                    BasePrice = 189.99m,

                    ImageUrl = "images/pizzas/margherita.jpg",
                }
            };
            //Agregando datos al contexto almacenados en memoria
            context.Specials.AddRange(Specials);
            //Guardado los datos en disco
            context.SaveChanges();
        }
    }
}
