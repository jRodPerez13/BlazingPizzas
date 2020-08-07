using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazingPizza.Server.Models;
using BlazingPizza.Shared;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace BlazingPizza.Server.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly PizzaStoreContext Context;

        public OrdersController(PizzaStoreContext context)
        {
            Context = context;
        }

        [HttpPost]
        public async Task<ActionResult<int>> PlaceOrder(Order order)
        {
            order.CreatedTime = DateTime.Now;
            //Establece una ubicacion de envio ficticia
            order.DeliveryLocation= 
                new LatLong(19.043679206924864, -98.19811254438645);
            // Establecer el valor de Pizza.SpecialId y Topping.ToppingId
            // para que no se creen nuevos registros Special y Topping.
            foreach(var pizza in order.Pizzas)
            {
                pizza.SpecialId = pizza.Special.Id;
                pizza.Special = null;
                foreach(var topping in pizza.Toppings)
                {
                    topping.ToppingId = topping.Topping.Id;
                    topping.Topping = null;
                }
            }

            Context.Orders.Attach(order);
            await Context.SaveChangesAsync();
            return order.OrderId;
        }

        //Devolverá la lista de órdenes
        [HttpGet]
        public async Task<ActionResult<List<OrderWithStatus>>> GetOrders()
        {
            var orders = await Context.Orders
                .Include(o => o.DeliveryLocation)
                .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                .Include(o => o.Pizzas).ThenInclude(p => p.Toppings)
                .ThenInclude(t => t.Topping)
                .OrderByDescending(o => o.CreatedTime)
                .ToListAsync();

            return orders.Select(
                o => OrderWithStatus.FromOrder(o)).ToList();
        }
    }
}
