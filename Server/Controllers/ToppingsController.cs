using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazingPizza.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazingPizza.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazingPizza.Server.Controllers
{
    [Route("toppings")]
    [ApiController]
    public class ToppingsController : ControllerBase
    {
        private readonly PizzaStoreContext Context;

        public ToppingsController(PizzaStoreContext context)
        {
            Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Topping>>> GetToppings()
        {
            return await Context.Toppings.
                OrderByDescending(t => t.Name).ToListAsync();
        }
    }
}
