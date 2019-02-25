var ajaxOptions = {
	homePagePath: "/home/",
	targetElement: "#DynamicContent",
	preloadLinks: false,
	animateIn: function (selector, html) {
		$(selector).each(function (index, el) {
			//$(el).html(html);
			$(el).toggle("fade", 250, function () {
				$(el).html(html);
				//$(el).css("height", "100%");
				// $("#mainNav").effect("fade");
				$(el).toggle("fade", 400);
				//$("body").scrollTop(0);
			});
		});
	}
}

function initAjaxOptions(options) {
	ajaxOptions = options;
}

function loadeData(selector, html) {
	$(".nav-link").parent().removeClass("current");
	$(".searchList").val("");
	ajaxOptions.animateIn(selector, html);
}

function preloadLinks() {

	$("a").each(function () {
		var href = $(this).attr("href");

		if (href == window.location.pathname)
			return;

		ajaxLoadUrl(href, "", function (el, html) {
			console.log("Preloaded: " + href);
		});
	});
}

$(document).ready(function () {

	if (ajaxOptions.preloadLinks) {
		preloadLinks();
	}

	if (window.location.hash != "") {
		var url = window.location.hash.replace("#", "");
		ajaxLoadUrl(url, ajaxOptions.targetElement);
	}
	else {
		ajaxLoadUrl(ajaxOptions.homePagePath + window.location.search, ajaxOptions.targetElement);
	}

	window.onpopstate = function (event) {


		if (event.state != null) {

			loadeData(lastTargetElement, event.state.html);
			updateTitle(event.state.href, event.state.html);

			/*ajaxLoadUrl(event.state.href, lastTargetElement, function (el, bodyHtml) {				
				event.state.html = bodyHtml;
			});*/
		}
	};

	$(document).on("click", "a", function (event) {
		var href = $(this).attr("href");
		var target = $(this).attr("target");

		if ($(this).parents("form").length > 0)
			return;

		if (target != "_blank" && $(this).parents(".field").length == 0 && $(this).parents("#AccessCMSPermissionsPanel").length == 0) {

			var urlSegment = href.split("?")[0];
			var segment = href.replace(window.location.origin, "");

			if (segment != "") {

				var loaded = ajaxLoadUrl(href, "#DynamicContent");

				//console.log(loaded, href, segment, window.location.pathname);

				if (loaded || segment == window.location.pathname || segment == href) {
					event.preventDefault();
				}
			}
		}
	});
});

var lastTargetElement = null;

function updateTitle(href, bodyHtml) {
	var doc = $('<output>').append($.parseHTML(bodyHtml, document, true));
	document.title = doc.find("title").text();

	doc.find("meta").each(function (index, el) {

		var name = $(el).attr("name");
		var content = $(el).attr("content");

		$("meta[name='" + name + "']").attr("content", content);

	});

}

function pushHistory(href, bodyHtml) {
	try {
		window.history.pushState({ href: href, html: bodyHtml }, document.title, href);

	} catch (error) {
		window.title = document.title;
	}
}


function _loadData(href, el, bodyHtml, callBackFunction) {

	if (callBackFunction != undefined && callBackFunction != "" && callBackFunction != null) {
		callBackFunction($(el), bodyHtml);
	}
	else {
		updateTitle(href, bodyHtml);
		pushHistory(href, bodyHtml);

		loadeData($(el), bodyHtml);
	}

	if (el != "") {
		//window.scrollTo(0, 0);
		trackPageView();

		//preloadLinks();
	}
}

function trackPageView() {
	setTimeout(function () {

		if (typeof ga == 'undefined') {
			console.log("Google Analytics Not installed! No PageViews will be tracked!");
			return;
		}

		var trackingId = "";

		ga.getAll().forEach(function (tracker) {
			trackingId = tracker.get("trackingId");
		});

		if (trackingId == "") {
			console.log("Error: Unable to get Google Analytics Tracking ID");
			return;
		}

		console.log("Found Google Analytics Tracking ID ( " + trackingId + " )!");

		if (typeof gtag != 'undefined') {

			console.log("Found gtag");

			gtag('config', trackingId, {
				'page_title': document.title,
				'page_path': document.location.pathname
			});

			console.log("Sent PageView for - " + document.location.pathname);

		}
		else if (typeof ga != 'undefined') {
			console.log("No 'gtag' found, falling back to 'ga'");

			ga('create', trackingId, 'auto');
			ga('set', 'page', location.pathname);
			ga('send', 'pageview');

			//ga('send', 'pageview', location.pathname);

			console.log("Sent PageView for - " + document.location.pathname);
		}
		else {
			console.log("Error tracking no ga or gtag were found, No PageViews will be tracked!");
		}
	}, 1000);
}

var isLoading = false;
var cache = [];

var timer = null;

function block() {
	console.log("Ran block");
	timer = setTimeout(function () {
		$.blockUI({
			message: '<div class="lds-ring"><div></div><div></div><div></div><div></div></div>',
			css: {
				border: 'none',
				background: 'none'
			}
		});
	}, 500);
}

function stopRequests() {
	console.log("Ran stopRequests");

	ajaxRequests.forEach(function (request) {
		request.abort();
	})
}

function unBlock() {
	console.log("Ran unBlock");

	$.unblockUI();
	if (timer != null) {
		clearTimeout(timer);
	}
}

function convertHrefToPath(href) {
	return href.replace(window.location.protocol + "//" + window.location.host, "");
}

var ajaxRequests = [];
function ajaxLoadUrl(href, targetElement, callBackFunction) {

	var urlSegment = href.split("?")[0];

	if ((href != undefined && href != null && href != "" && href.toLowerCase().indexOf("@") == -1 && href.toLowerCase().indexOf("javascript") == -1 && href.indexOf("javascript") == -1 && href.indexOf("tel:") == -1 && href != "/") && (href.indexOf("http") == -1 || (href.indexOf("http") != -1 && href.indexOf(window.location.host) != -1)) || (href.charAt(0) == "/")) {

		pathname = convertHrefToPath(href);

		if (pathname == window.location.pathname)
			return;

		if ((isLoading && targetElement != "") || (urlSegment == window.location.pathname || urlSegment == window.location.href || "/" + urlSegment == window.location.pathname))
			return;

		isLoading = true;

		if (targetElement != "") {
			lastTargetElement = targetElement;
		}

		var el = targetElement;

		if (cache[pathname] != undefined) {
			_loadData(href, el, cache[pathname], callBackFunction);
		}
		else {

			if (targetElement != "") {
				//stopRequests();
				block();
			}

			var ajaxRequest = $.get(href, function (data) {

				var bodyHtml = data;

				var curPath = convertHrefToPath(this.url);

				cache[curPath] = bodyHtml;

				_loadData(href, el, bodyHtml, callBackFunction);

				if (targetElement != "") {
					unBlock();
				}

			});

			ajaxRequests.push(ajaxRequest);
		}

		isLoading = false;

		return true;
	}
	else {
		return false;
	}
}