const CACHE_NAME = "site-cache";

self.addEventListener('install', function (event) {
	self.skipWaiting();
});

function fetchAndCache(event, skipCache) {

	return fetch(event.request).then(function (response) {

		if (skipCache) {
			//console.log("Skipped cache", response);
			return response;
		}

		return saveToCacheAndReturn(event, response);

	});
}

function saveToCacheAndReturn(event, response) {

	if (response.status !== 200 && response.status !== 0)
		return response;

	var cacheResponse = response.clone();

	return caches.open(CACHE_NAME).then(function (cache) {
		//console.log("Putting data into cache", cacheResponse);

		cache.put(event.request, cacheResponse);

		return response;
	})
}

self.addEventListener("fetch", function (event) {

	event.respondWith(

		caches.match(event.request).then(function (response) {			

			var skipCache = false;
			var url = event.request.url.toLowerCase();

			const isAdminRequest = (url.indexOf("/admin") > -1 || url.indexOf("/login") > -1);

			if (isAdminRequest) {				
				skipCache = true;
			}			

			if (!skipCache && response) {		

				fetchAndCache(event, skipCache);
				return response;
			}

			return fetchAndCache(event, skipCache);

		})
	);
});

self.addEventListener("activate", function (event) {
	//console.log("activate ran");

	//event.respondWith(
	//	caches.keys().map(function (cacheName) {
	//		console.log("deting cache with name", cacheName);
	//		return caches.delete(cacheName);
	//	})
	//);
});