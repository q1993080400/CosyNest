
/*如何正确地更新PWA应用
  1.在index.html文件中的script标签，
  将加载serviceWorker的脚本改成以下形式，
  它的意思是不对文件哈希进行缓存
  
  navigator.serviceWorker.register('service-worker.js', { updateViaCache: 'none' });

  2.将service-worker.published.js文件的onFetch方法改成以下形式，
  它的意思是缓存采用网络优先模式，当无法连接网络的时候，才会使用缓存
 
async function onFetch(event) {
    return await fetch(event.request).catch(x => {
        let cachedResponse = null;
        if (event.request.method === 'GET') {
            const shouldServeIndexHtml = event.request.mode === 'navigate';
            const request = shouldServeIndexHtml ? 'index.html' : event.request;
            const cache = await caches.open(cacheName);
            cachedResponse = await cache.match(request);
        }
        return cachedResponse;
    });
}

  3.在manifest.json文件中，递增version属性，
  调用ToolPWA.UpdatePWAVersion方法可以自动完成这个过程*/