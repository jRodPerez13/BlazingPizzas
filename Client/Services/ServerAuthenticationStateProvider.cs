using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazingPizza.Client.Services
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> 
            GetAuthenticationStateAsync()
        {
            // Por el momento sólo devolvemos datos falsos
            // Posteriormente devolveremos datos del servidor
            var Claim = new Claim(ClaimTypes.Name, "Usuario falso");
            var Identity = new ClaimsIdentity(new[] { Claim }, "serverauth");
            return new AuthenticationState(new ClaimsPrincipal(Identity));
        }
    }
}
