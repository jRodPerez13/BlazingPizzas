using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using BlazingPizza.Shared;
using System.Net.Http;
using System.Net.Http.Json;

namespace BlazingPizza.Client.Services
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState>
            GetAuthenticationStateAsync()
        {
            //// Por el momento sólo devolvemos datos falsos
            //// Posteriormente devolveremos datos del servidor
            //var Claim = new Claim(ClaimTypes.Name, "Usuario falso");
            //var Identity = new ClaimsIdentity(new[] { Claim }, "serverauth");
            //return new AuthenticationState(new ClaimsPrincipal(Identity));
            var UserInfo = await HttpClient.GetFromJsonAsync<UserInfo>("user");
            var Identity = UserInfo.IsAuthenticated
                ? new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name,UserInfo.Name)
                    }, "serverauth")
                : new ClaimsIdentity();
            return new AuthenticationState(new ClaimsPrincipal(Identity));
        }

        private readonly HttpClient HttpClient;
        public ServerAuthenticationStateProvider(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }
    }
}
