$(document).ready(function () {
  
    $.get(BaseUrl + "WebServices/IMediaDetails.asmx/GetGlossaryTerms", function (data) {
        $(data).each(function () {
            var term = this.Term;
            var definition = this.Definition;

            var regex = new RegExp("\\b" + term + "\\b(?![^<]*</[a|span]+>)", "gi");

            var replacedTerm = [];

            $("p:contains(" + term + "), li:contains(" + term + ")").not(".cd-nav li, .accordion li").each(function () {
                var html = $(this).html();

                if ($(this).find("span[title~='" + term + "']").length > 0)
                    return false;

                var html = html.replace(regex, function (match, offset, original) {
                    //return '<span data-tooltip aria-haspopup="true" class="has- tip" data-disable-hover="false" tabindex="1" title="' + definition + '">' + match+'</span>'
                    return "<span data-toggle='tooltip' title='" + definition + "'>" + match + "</span>";
                });

                $(this).html(html);
            });      
            
            $("img[src*='" + term + "']").not(".cd-nav li, .accordion li").each(function () {
                $(this).attr("title", definition);
                $(this).attr("data-toggle", "tooltip");
            });  

        });

        //initToolTips();
	});    

	/*if ('serviceWorker' in navigator) {
		navigator.serviceWorker.register(BaseUrl + "service-worker.js").then(function () {
			console.log("registered service worker")
		}, function () {
			console.log("error registering service worker")
		});
	}*/


	//preloadNavItems();

});

function initToolTips() {    
    if (navigator.appVersion.indexOf("MSIE 8.0") == -1) {        
        $('[data-toggle="tooltip"]').tooltip({ track: true });
    }
    else {
        $('[data-toggle="tooltip"]').addClass('apc-terms');
    }
}

var urlCache = new Array();

window.onpopstate = function (event) {
	writeDocument(event.state);
};

function writeDocument(data) {

	var newDoc = document.open("text/html", "replace");
	newDoc.write(data);
	newDoc.close();

	//$("html").html(data);
}


function preloadNavItems() {

	const storageKey = "urlCache";

	var storedUrlCache = sessionStorage.getItem(storageKey);	

	$(document).on("click", "a", function (event) {
		console.log("clicked");
		event.preventDefault();

		var href = $(this).attr("href");

		storedUrlCache = sessionStorage.getItem(storageKey);

		if (storedUrlCache != null) {
			const urlCacheObj = JSON.parse(storedUrlCache);

			urlCacheObj.map(function (item) {
				if (item.url === href) {

					//console.log($.parseXML(item.cache));

					//var tempDom = $("<output>").append($.parseXML(item.cache))

					//var newHtml = tempDom.find("body");

					//console.log(tempDom);

					//$("#PageContent").html(newHtml.html());


					//$("html").html(item.cache +"<script>alert('ran')</script>");

					//console.log("ran");

					history.pushState(item.cache, '', href);
					writeDocument(item.cache);


					/*$("#PageContent").filter('script').each(function () {
						$.globalEval(this.text || this.textContent || this.innerHTML || '');
					});*/

					//var cloneScripts = $("script").clone();

					//$("head script").remove();	

					//$("#PageContent")[0].outerHTML = newHtml;

					//$("head").append(cloneScripts);
				}
			});
		}
		else {
			console.log("else");
		}
	});

	if (storedUrlCache === null) {		

		$("a").each(function () {

			var url = $(this).attr("href");

			//if (url === undefined) {
			//	url = $(this).attr("src");
			//}


			if (url != undefined && url !== null && url !== "" ) {
				jQuery.ajax({
					url: url,
					success: function (result) {
						var obj = { url: url, cache: result };
						urlCache.push(obj);
					},
					async: false
				});
			}

		});
		
		sessionStorage.setItem(storageKey, JSON.stringify(urlCache));	

		console.log("Saved to storage");
	}	
}