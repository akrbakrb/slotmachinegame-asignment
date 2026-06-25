const cacheName = "DefaultCompany-SlotMachineGame-1.0";
const contentToCache = [
    "Build/ce1e8d813c18e43c200f7d7c73a274d5.loader.js",
    "Build/78e8b9ae9d58156285fa5d3e7f51b1de.framework.js",
    "Build/5000647c9531698960d1c43639f3434f.data",
    "Build/28812905f9db3381a33fe1839f6c37a3.wasm",
    "TemplateData/style.css"

];

self.addEventListener('install', function (e) {
    console.log('[Service Worker] Install');
    
    e.waitUntil((async function () {
      const cache = await caches.open(cacheName);
      console.log('[Service Worker] Caching all: app shell and content');
      await cache.addAll(contentToCache);
    })());
});

self.addEventListener('fetch', function (e) {
    e.respondWith((async function () {
      let response = await caches.match(e.request);
      console.log(`[Service Worker] Fetching resource: ${e.request.url}`);
      if (response) { return response; }

      response = await fetch(e.request);
      const cache = await caches.open(cacheName);
      console.log(`[Service Worker] Caching new resource: ${e.request.url}`);
      cache.put(e.request, response.clone());
      return response;
    })());
});
