using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlazingPizza.Server.Models;
using BlazingPizza.Shared;

namespace BlazingPizza.Server.Controllers
{
    [Route("notifications")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly PizzaStoreContext Context;
        public NotificationsController(PizzaStoreContext context)
        {
            this.Context = context;
        }

        [HttpPut("subscribe")]
        public async Task<NotificationSubscription> Subscribe(
            NotificationSubscription subscription)
        {
            // Estamos almacenando como máximo una suscripción por usuario,
            // por lo tanto, eliminamos las antiguas.
            // Alternativamente, podemos permitir que el usuario registre
            // varias suscripciones de diferentes navegadores o dispositivos.
            var UserId = GetUserId();
            var OldSubscriptions =
                Context.NotificationSubscriptions.Where(e => e.UserId == UserId);
            Context.NotificationSubscriptions.RemoveRange(OldSubscriptions);

            //Alamacenar la nueva suscripción
            subscription.UserId = UserId;
            Context.NotificationSubscriptions.Attach(subscription);

            await Context.SaveChangesAsync();
            return subscription;
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirst(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        }
    }
}
