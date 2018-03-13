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

        initToolTips();
    });    

});

function initToolTips() {    
    if (navigator.appVersion.indexOf("MSIE 8.0") == -1) {        
        $('[data-toggle="tooltip"]').tooltip({ track: true });
    }
    else {
        $('[data-toggle="tooltip"]').addClass('apc-terms');
    }
}