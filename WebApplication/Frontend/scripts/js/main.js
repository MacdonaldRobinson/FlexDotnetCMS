$(document).ready(function() {

  // init sticky containers via sticky-js
  var sticky = new Sticky('[data-sticky]');

  sticky.update();

  // displaying an unordered list in two columns - script backup for legacy browsers
  (function($) {
    var initialContainer = $('.columns'),
      columnItems = $('.columns li'),
      columns = null,
      column = 1; // account for initial column
    function updateColumns() {
      column = 0;
      columnItems.each(function(idx, el) {
        if (idx !== 0 && idx > (columnItems.length / columns.length) + (column * idx)) {
          column += 1;}
        $(columns.get(column)).append(el);
      });
    }

    function setupColumns() {
      columnItems.detach();
      while (column++ < initialContainer.data('columns')) {
        initialContainer.clone().insertBefore(initialContainer);
        column++;
      }
      columns = $('.columns');
    }

    $(function() {
      setupColumns();
      updateColumns();
    });
  })(jQuery);


  // bootstrap mobile nav - dropdowns
  if ($(window).width() <= 1080) {
    $(".has-children").each(function() {
      var parentAnchor = $(this).find(">a");
      var cloneParentAnchor = parentAnchor.clone();

      parentAnchor.attr("href", "#");

      var firstChildLi = $(this).find("ul li").first().clone();

      firstChildLi.find("a").attr("href", cloneParentAnchor.attr("href"));
      firstChildLi.find("a").text(cloneParentAnchor.text());

      $(this).find("ul").prepend(firstChildLi);
    });
    $(".has-children").click(function() {
      $(this).find('.dropdown-menu').toggle();
    });
  }

 // smooth scroll to top of pages

    $(window).scroll(function(){
        if ($(this).scrollTop() > 50) {
            $('#backToTop').fadeIn('slow');
        } else {
            $('#backToTop').fadeOut('slow');
        }
    });
    $('#backToTop').click(function(){
        $("html, body").animate({ scrollTop: 0 }, 600);
        return false;
    });


});
