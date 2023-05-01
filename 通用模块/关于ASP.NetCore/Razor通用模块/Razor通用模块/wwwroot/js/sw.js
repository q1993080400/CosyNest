self.onnotificationclick = function (event) {
    event.notification.close();
    if (typeof event.notification.data === 'string') {
        event.waitUntil(self.clients.openWindow(event.notification.data));
    }
};
self.addEventListener("install", (event) => {
    self.skipWaiting();
});   
