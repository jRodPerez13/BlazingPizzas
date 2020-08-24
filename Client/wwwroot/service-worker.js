//Instalar el Service Worker
self.addEventListener('install', async event => {
    console.log('Instalando el Service Worker...');
    self.skipWaiting();
})