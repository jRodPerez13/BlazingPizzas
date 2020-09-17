//Instalar el Service Worker
self.addEventListener('install', async event => {
    console.log('Instalando el Service Worker...');
    self.skipWaiting();
});

self.addEventListener('fetch', event => {
    // Podemos agregar aquí lógica personalizada para controlar
    // si se pueden utilizar los datos en caché cuando la aplicación
    // se ejecuta fuera de línea.
    // La siguiente línea hace que las solicitudes vayan directamente
    // a la red como de costumbre.
    return null
});

self.addEventListener('push', event => {
    const payload = event.data.json();
    event.waitUntil(
        self.registration.showNotification('Blazing Pizza', {
            body: payload.message,
            icon: 'images/icon-512.png',
            vibrate: [100, 50, 100],
            data: { url: payload.url }
        })
    );
});

self.addEventListener('notificationclick', event => {
    event.notification.close();
    event.waitUntil(clients.openWindow(event.notification.data.url));
});