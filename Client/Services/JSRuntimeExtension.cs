using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazingPizza.Client.Services
{
    public static class JSRuntimeExtension
    {
       public static ValueTask<bool> Confirm(this IJSRuntime jSRuntime, 
           string message)
        {
            return jSRuntime.InvokeAsync<bool>("confirm", message);
        }
    }
}
