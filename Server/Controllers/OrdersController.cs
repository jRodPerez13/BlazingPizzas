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
using Microsoft.AspNetCore.Authorization;
using WebPush;
using System.Text.Json;

namespace BlazingPizza.Server.Controllers
{
    [Route("orders")]
    [ApiController]
    [Authorize]
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
            order.DeliveryLocation =
                new LatLong(19.043679206924864, -98.19811254438645);
            // Establecer el valor de Pizza.SpecialId y Topping.ToppingId
            // para que no se creen nuevos registros Special y Topping.

            order.UserId = GetUserId();

            foreach (var pizza in order.Pizzas)
            {
                pizza.SpecialId = pizza.Special.Id;
                pizza.Special = null;
                foreach (var topping in pizza.Toppings)
                {
                    topping.ToppingId = topping.Topping.Id;
                    topping.Topping = null;
                }
            }

            Context.Orders.Attach(order);
            await Context.SaveChangesAsync();

            // En segundo plano, enviar notificaciones push de ser posible
            var Subscription = await Context.NotificationSubscriptions.Where(
                e => e.UserId == GetUserId()).SingleOrDefaultAsync();
            if (Subscription != null)
            {
                _ = TrackAndSendNotificationsAsync(order, Subscription);
            }

            return order.OrderId;
        }

        //Devolverá la lista de órdenes
        [HttpGet]
        public async Task<ActionResult<List<OrderWithStatus>>> GetOrders()
        {
            var orders = await Context.Orders
                .Where(o => o.UserId == GetUserId())
                .Include(o => o.DeliveryLocation)
                .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                .Include(o => o.Pizzas).ThenInclude(p => p.Toppings)
                .ThenInclude(t => t.Topping)
                .OrderByDescending(o => o.CreatedTime)
                .ToListAsync();

            return orders.Select(
                o => OrderWithStatus.FromOrder(o)).ToList();
        }
        //Detalle orden por ID
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderWithStatus>> GetOrderWithStatus(
            int orderId)
        {
            var order = await Context.Orders
                .Where(o => o.UserId == GetUserId())
                .Where(o => o.OrderId == orderId)
                .Include(o => o.DeliveryLocation)
                .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                .Include(o => o.Pizzas).ThenInclude(p => p.Toppings)
                .ThenInclude(t => t.Topping)
                .SingleOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }
            return OrderWithStatus.FromOrder(order);
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirst(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        }

        private static async Task SendNotificationAsync(Order order,
            NotificationSubscription subscription, string message)
        {
            // En una aplicación real puedes generar tus propias llaves en
            // https://tools.reactpwa.com/vapid
            var PublicKey =
                "BLC8GOevpcpjQiLkO7JmVClQjycvTCYWm6Cq_a7wJZlstGTVZvwGFFHMYfXt6Njyvgx_GlXJeo5cSiZ1y4JOx1o";
            var PrivateKey =
                "OrubzSz3yWACscZXjFQrrtDwCKg-TGFuWhluQ2wLXDo";
            var PushSubscription =
            new PushSubscription(
            subscription.Url, subscription.P256dh, subscription.Auth);
            // Aquí puedes colocar tu propio correo en <someone@example.com>
            var VapidDetails =
            new VapidDetails("mailto:someone@example.com",
            PublicKey, PrivateKey);
            var WebPushClient = new WebPushClient();
            try
            {
                var Payload = JsonSerializer.Serialize(new
                {
                    message,
                    url = $"myorders/{order.OrderId}",
                });
                await WebPushClient.SendNotificationAsync(
                PushSubscription, Payload, VapidDetails);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                $"Error al enviar la notificación push: {ex.Message}");
            }
        }

        private static async Task TrackAndSendNotificationsAsync(Order order,
            NotificationSubscription subscription)
        {
            // En un caso real, algún otro proceso de backend rastrearía
            // el progreso de la entrega y nos enviaría notificaciones cuando
            // haya cambios. Como aquí no tenemos tal proceso, lo simularemos.
            await Task.Delay(OrderWithStatus.PreparationDuration);
            await SendNotificationAsync(
                order, subscription, "¡Tu orden ya esta en camino!");
            await Task.Delay(OrderWithStatus.DeliveryDuration);
            await SendNotificationAsync(order, subscription,
                "¡Tu orden ha sido entregada! ¡Disfrutala!");
        }


    }
}
