var ajaxOptions = {
	targetElement: "#DynamicContent",
	omitElementSelector: ".not-ajax",
	preloadLinks: false,
	onLoad: function () { },
	block: function () {
		$.blockUI({ message: "Loading ..." });
		console.log("Ran block")
	},
	unBlock: function () {
		$.unblockUI();
		console.log("Ran unblockUI")
	},
	animateOut: function (el) {
		$(el).fadeOut();
		console.log("Ran animateOut");
	},
	animateIn: function (el) {
		$(el).fadeIn();
		console.log("Ran animateIn");
	}
}

function initAjaxOptions(options) {
	ajaxOptions = options;
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

	trackPageView();

	if (ajaxOptions.preloadLinks) {
		preloadLinks();
	}

	/*if (window.location.hash != "") {
		var url = window.location.hash.replace("#", "");
		ajaxLoadUrl(url, ajaxOptions.targetElement);
	}
	else {
		ajaxLoadUrl(ajaxOptions.homePagePath + window.location.search, ajaxOptions.targetElement);
	}*/

	window.onpopstate = function (event) {

		if (event.state != null) {
			console.log("Ran window.onpopstate");

			_loadData(event.state.href, lastTargetElement, event.state.html, function () { }, false);
			/*ajaxLoadUrl(event.state.href, lastTargetElement, function (el, bodyHtml) {				
				event.state.html = bodyHtml;
			});*/
		}
	};

	$(document).on("click", "a:not(" + ajaxOptions.omitElementSelector + ")", function (event) {

		if ($(this).closest(ajaxOptions.omitElementSelector).length > 0)
			return;

		var href = $(this).attr("href");
		var target = $(this).attr("target");

		if (href != undefined && target != "_blank" && !$(this).hasClass("edit") && href.indexOf("javascript:") == -1 && $(this).parents("#AccessCMSPermissionsPanel").length == 0 && href.charAt(0) != "#") {

			var urlSegment = href.split("?")[0];
			var segment = href.replace(window.location.origin, "");

			if (segment != "") {

				if (href.indexOf("://") != -1 && href.indexOf(window.location.host) == -1)
					return;

				var loaded = ajaxLoadUrl(href, ajaxOptions.targetElement);

				if (loaded || (segment == window.location.pathname || (segment == href && segment.indexOf("mailto:") == -1 && segment.indexOf("tel:") == -1))) {
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


function _loadData(href, el, bodyHtml, callBackFunction, addToHistory) {

	if (addToHistory == undefined)
		addToHistory = true;

	var dynamicContent = bodyHtml;

	var targetSelector = ajaxOptions.targetElement;
	var targetName = ajaxOptions.targetElement.replace("#", "");

	if ($(targetSelector).length > 0 && bodyHtml.indexOf(targetName) != -1) {
		ajaxOptions.animateOut(el);

		var doc = $('<output>').append($.parseHTML(bodyHtml, document, true));
		//dynamicContent = doc.find(targetSelector).html();		

		var foundElements = doc.find(targetSelector);

		foundElements.each(function () {
			var id = $(this).attr("id");

			var currentElem = $("#" + id);

			var currentElemHtml = currentElem.html();
			var newHtml = $(this).html();

			if (currentElemHtml != newHtml) {
				currentElem.replaceWith(this);
			}
		});

		$(window).scrollTop(0);

		if (callBackFunction != undefined && callBackFunction != "" && callBackFunction != null) {
			callBackFunction($(el), bodyHtml);
		}

		updateTitle(href, bodyHtml);

		if (addToHistory) {
			pushHistory(href, dynamicContent);
		}

		if (el != "") {
			trackPageView();
			//preloadLinks();
		}

		ajaxOptions.onLoad(bodyHtml);
		ajaxOptions.animateIn(el);
	}

}

function trackPageView() {

	setTimeout(function () {

		if (typeof ga == 'undefined') {
			console.log("Google Analytics Not installed! No PageViews will be tracked!");
			return;
		}

		var trackingId = "";

		ga(function () {
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
		});

	}, 1000);
}

var isLoading = false;
var cache = [];

function stopRequests() {
	console.log("Ran stopRequests");

	ajaxRequests.forEach(function (request) {
		request.abort();
	})
}

function convertHrefToPath(href) {
	return href.replace(window.location.protocol + "//" + window.location.host, "");
}

var ajaxRequests = [];
function ajaxLoadUrl(href, targetElement, callBackFunction) {

	var querySeperator = "?";

	if (href.indexOf("?") != -1) {
		querySeperator = "&";
	}

	//if (href.indexOf("homePagePath") == -1) {
	//	href = href + querySeperator + "homePagePath=" + ajaxOptions.homePagePath;
	//}

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
				ajaxOptions.block();
			}

			var ajaxRequest = $.get(href, function (data) {

				var bodyHtml = data;

				var curPath = convertHrefToPath(this.url);

				cache[curPath] = bodyHtml;

				_loadData(href, el, bodyHtml, callBackFunction);

				if (targetElement != "") {
					ajaxOptions.unBlock();
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