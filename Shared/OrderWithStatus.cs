using BlazingPizza.ComponentsLibrary.Map;
using System;
using System.Collections.Generic;
using System.Text;


namespace BlazingPizza.Shared
{
    public class OrderWithStatus
    {
        // Simular el tiempo de preparación de la orden.
        public readonly static TimeSpan PreparationDuration =
             TimeSpan.FromSeconds(20);
        // Simular el tiempo en que se tarda repartidor en entregar
        // la orden.
        private readonly static TimeSpan DeliveryDuration =
            TimeSpan.FromMinutes(1);

        public Order Order { get; set; }
        public string Statustext { get; set; }
        public List<Marker> MapMarkers { get; set; }

        // Obtener el mensaje de estatus de la orden y los marcadores
        // de posición en el mapa.
        public static OrderWithStatus FromOrder(Order order)
        {
            // Para simular un proceso real en el backend,
            // simularemos cambios en el estatus basándonos en
            // el tiempo transcurrido desde que la orden fue realizada.
            string Message;
            List<Marker> Markers;
            // Tiempo en que se despacha a entrega el pedido.
            var Dispatchtime = order.CreatedTime.Add(PreparationDuration);
            if (DateTime.Now < Dispatchtime)
            {
                Message = "Preparando";
                Markers = new List<Marker>
                {
                    //Marca el destino
                    ToMapMarker("Usted",
                    order.DeliveryLocation, showPopup: true)
                };
            }
            else if (DateTime.Now < Dispatchtime + DeliveryDuration)
            {
                Message = "En camino";
                //Simular la posicion del repartidor
                var StartPosition = ComputeStartPosition(order);
                var ProportionOfDeliveryCompleted = Math.Min(1,
                    (DateTime.Now - Dispatchtime).TotalMilliseconds
                    / DeliveryDuration.TotalMilliseconds);
                var DriverPosition = LatLong.Interpolate(
                    StartPosition, order.DeliveryLocation,
                    ProportionOfDeliveryCompleted);
                Markers = new List<Marker>
                {
                    ToMapMarker("Usetd", order.DeliveryLocation),
                    ToMapMarker("Repartidor", DriverPosition, 
                        showPopup: true)
                };
            }
            else
            {
                Message = "Entregado";
                Markers = new List<Marker>
                {
                    ToMapMarker("Ubicación de entrega",
                    order.DeliveryLocation, showPopup:true)
                };
            }

            return new OrderWithStatus
            {
                Order = order,
                Statustext = Message,
                MapMarkers = Markers
            };
        }


        // Obtener una posición en el mapa.
        static Marker ToMapMarker(string description, LatLong coords,
            bool showPopup = false) => new Marker
            {
                Description=description,
                X= coords.Longitude,
                Y=coords.Latitude,
                ShowPopup=showPopup
            };

        static LatLong ComputeStartPosition(Order order)
        {
            // Obtener una posición aleatoria 
            var Random = new Random(order.OrderId);
            var Distance = 0.01 + Random.NextDouble() * 0.02;
            var Angle = Random.NextDouble() * Math.PI * 2;
            var Offset = (Distance * Math.Cos(Angle),
                Distance * Math.Sin(Angle));
            return new LatLong(
                order.DeliveryLocation.Latitude + Offset.Item1,
                order.DeliveryLocation.Longitude + Offset.Item2);            
        }
    }
}
