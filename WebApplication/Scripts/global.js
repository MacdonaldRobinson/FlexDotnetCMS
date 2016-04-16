function sanatize(str) {
	str = str.replace(/[~!@#$%^*()+=<>.|\[\]{}\",:;`?\\]/g, "");
	str = str.replace(/^\s+|\s+$/g, "");
	return str;
}

function prepairUri(str) {
	str = str.toLowerCase();
	str = str.replace(/\s/g, "-");
	str = sanatize(str);
	str = str.replace(/&/g, "");
	str = str.replace(/\//g, "_");
	str = str.replace(/--/g, "-");
	str = str.replace(/:/g, "");
	str = str.replace(/\'/g, "");
	return str;
}

function clearAllTagged() {
	jQuery(".tagged").attr('value', '');
}

(function ($) {
	var imgList = [];
	$.extend({
		preload: function (imgArr, option) {
			var setting = $.extend({
				init: function (loaded, total) { },
				loaded: function (img, loaded, total) { },
				loaded_all: function (loaded, total) { }
			}, option);
			var total = imgArr.length;
			var loaded = 0;

			setting.init(0, total);
			for (var i in imgArr) {
				imgList.push($("<img />")
					.attr("src", imgArr[i])
					.load(function () {
						loaded++;
						setting.loaded(this, loaded, total);
						if (loaded == total) {
							setting.loaded_all(loaded, total);
						}
					})
				);
			}
		}
	});
})(jQuery);

var colorBoxWidth = "90%";
var colorBoxHeight = "90%";

function GetQueryStringParams(url) {
	var vars = [], hash;
	var q = url.split('?')[1];
	if (q != undefined) {
		q = q.split('&');
		for (var i = 0; i < q.length; i++) {
			hash = q[i].split('=');
			vars.push(hash[1]);
			vars[hash[0]] = hash[1];
		}
	}

	return vars;
}

function initAccordians() {
    if ($(".accordian").length > 0) {
        $(".accordian.opened").accordion({ heightStyle: "content" });

        $(".accordian.closed").accordion({
            active: false,
            collapsible: true,
            heightStyle: "content"
        });
    }
}

$(document).ready(function () {

    initAccordians();

    OnUpdatePanelRefreshComplete(function (event) {
        initAccordians();
    });

	$(document).on("click", ".colorbox.iframe", function () {
		//console.log("ran");
		var dataOnColorboxClose = $(this).attr("data-OnColorboxClose");
		$.colorbox({
			href: $(this).attr("href"), width: colorBoxWidth, height: colorBoxHeight, iframe: true, fixed: true, onClosed: function () {

				if (dataOnColorboxClose != undefined)
					eval(dataOnColorboxClose);
			}
		});
		return false;
	});

	//$(".colorbox.iframe").colorbox({ width: colorBoxWidth, height: colorBoxHeight, iframe: true, fixed: true });
	//$(".colorbox-iframe-refreshOnClose").colorbox({ width: colorBoxWidth, height: colorBoxHeight, iframe: true, onClosed: function () { window.location.reload() }, fixed: true, overlayClose: false });
	//$(".colorbox").colorbox({ fixed: true });
	//$(".colorbox-refreshOnClose").colorbox({ width: colorBoxWidth, height: colorBoxHeight, onClosed: function () { window.location.reload() }, fixed: true });

	//$(".applyToolTip").tipTip();

	$(document).bind('cbox_open', function () {
		$('html').css({ overflow: 'hidden' });
	}).bind('cbox_closed', function () {
		$('html').css({ overflow: '' });
	});

	function resizeColorBox() {
		if ($('#cboxOverlay').is(':visible')) {
			jQuery.colorbox.resize({ width: colorBoxWidth, height: colorBoxWidth });
		}
	}

	//function autoIframeHeight() {
	//    $('iframe').iframeAutoHeight({
	//        minHeight: 20, // Sets the iframe height to this value if the calculated value is less
	//        heightOffset: 10  // Optionally add some buffer to the bottom
	//    });
	//}

	$(window).resize(function () {
		resizeColorBox();
		//autoIframeHeight();
	});

	if (document.addEventListener != undefined) {
		window.addEventListener("orientationchange", resizeColorBox, false);
	}

	//autoIframeHeight();
});