﻿function clearAllTagged() {
	jQuery(".tagged").attr('value', '');
}

var colorBoxWidth = "95%";
var colorBoxHeight = "95%";

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

function RefreshUpdatePanel(UpdatePanelClientId, OnAfterRefreshFunction) {

    if (OnAfterRefreshFunction != undefined) {
        var OnCompleteFunction = function () {
            OnAfterRefreshFunction();
        }

        OnUpdatePanelRefreshComplete(OnCompleteFunction);
    }

    __doPostBack(UpdatePanelClientId, '');
}

function OnUpdatePanelRefreshComplete(OnUpdatePanelRefreshCompleteFunction) {
    if (typeof (Sys) != "undefined" && Sys.WebForms.PageRequestManager != null && Sys.WebForms.PageRequestManager != undefined)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(OnUpdatePanelRefreshCompleteFunction);
    }
}

$(document).ready(function () {        

    initAccordians();

    OnUpdatePanelRefreshComplete(function (event) {
        initAccordians();
    });

    $(document).on("click", ".colorbox.iframe", function () {
        
        var dataOnColorboxClose = $(this).attr("data-OnColorboxClose");

        var width = $(this).attr("data-width");
        if (width != undefined)
            colorBoxWidth = width;

        var height = $(this).attr("data-height");
        if (height != undefined)
            colorBoxHeight = height;

		$.colorbox({
			href: $(this).attr("href"), width: colorBoxWidth, height: colorBoxHeight, iframe: true, fixed: true, onClosed: function () {
				if (dataOnColorboxClose != undefined)
					eval(dataOnColorboxClose);
			}
		});
		return false;
	});

	$(document).bind('cbox_open', function () {
		$('html').css({ overflow: 'hidden' });
	}).bind('cbox_closed', function () {
		$('html').css({ overflow: '' });
	});

	function resizeColorBox() {
		if ($('#cboxOverlay').is(':visible')) {
            jQuery.colorbox.resize({ width: colorBoxWidth, height: colorBoxHeight });
		}
	}

	$(window).resize(function () {
		resizeColorBox();
	});

	if (document.addEventListener != undefined) {
		window.addEventListener("orientationchange", resizeColorBox, false);
	}
});