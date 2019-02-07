$(document).ready(function() {
    $(".navbar-nav .nav-link").on("click", function () {
        $(".navbar-nav").find(".current").removeClass("current");
        $(this).parent().addClass("current");
        $(this).closest(".navbar__menu").removeClass("show--menu");
    });

    //$('.nav-link[href*="'+window.location.pathname+'"]').parent().addClass('current');
    $('.nav-link').each(function(){
        var href = $(this).attr("href");
        href = href.replace(window.location.origin, "");
        if (window.location.pathname.indexOf(href) != -1) {
            $(this).parent().addClass("current");
        } 
    });
    
    $(".search__toggle").on('click', function() {
        $(".navbar__search").addClass('show--search');
    });
    
    $(".menu__toggle").on('click', function() {
        $(".navbar__menu").addClass('show--menu');
    });
    
    $('.fa-times').on('click', function() {
        $('.navbar__search, .navbar__menu').removeClass('show--search show--menu');
    });

    var options = {
        url: "/WebServices/Site.asmx/GetSearchList",
        getValue: "SectionTitle",
        list: {
            match: {
                enabled: true
            }
        },

        template: {
            type: "links",
            fields: {
                link: "AbsoluteUrl"
            }
        },

        theme: "square",
        adjustWidth: false
    };

    $(".searchList").easyAutocomplete(options);


    $('.intro__content').shave(90);
    $('.blog__card.large h2').shave(190);
    $('.blog__card.small h2').shave(115);
});


  