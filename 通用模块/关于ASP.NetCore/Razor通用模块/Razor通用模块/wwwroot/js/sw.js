//请将本文件复制到wwwroot/js文件夹，以启用通知功能
//你可以自定义本文件

self.onnotificationclick = function (event) {
    event.notification.close();
    if (typeof event.notification.data === 'string') {
        event.waitUntil(self.clients.openWindow(event.notification.data));
    }
};

self.oninstall = function (event) {
    self.skipWaiting();
};   
