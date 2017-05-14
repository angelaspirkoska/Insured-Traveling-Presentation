$(document).ready(function() {

    // menu helpers
    // - height helper
    var resizeTimer;
    var resizeHelper = function() {
      if ( $(window).width() < 768 ) {
        $('.my-dropdown-big').css({
          'height': 'auto',
          'overflow-y': 'hidden'
        });
      } else {
        $('.my-dropdown-big').css({
          'height': ( $(window).height() - 120 ) + 'px',
          'overflow-y': 'auto'
        });
      }
    }
    // - on resize(end)
    $(window).on('resize', function(e) {
      clearTimeout(resizeTimer);
      resizeTimer = setTimeout(resizeHelper, 350);
    });
    // - initial
    resizeHelper();
    
    // - group toggle
    $('ul.dropdown-menu [data-toggle=dropdown]').on('click', function(e) {
        e.preventDefault();
        e.stopPropagation();

        var t$ = $(this);

        var m$ = t$.parent().find('ul');

        // slideup all other open
        $('ul.my-dropdown-big ul.so-dropdown-submenu').each(function() {
            if ( $(this).attr('id') !== m$.attr('id') ) {
                $(this).slideUp();
                $(this).parent().find('a').removeClass('dropped');
            }
        });

        // toggle submenu
        m$.slideToggle(function() {
          t$.toggleClass('dropped');
        });
        // $('#auto-submenu').slideToggle();

        //$(this).parent().siblings().removeClass('open');
        //$(this).parent().toggleClass('open');

        resizeHelper();

    });

    // - close menu
    var dropCloseTimer = null;
    var dropClose = function() {
        $('.dropdown.open .dropdown-toggle').dropdown('toggle');
        $('ul.dropdown-menu ul.so-dropdown-submenu').hide();
        $('ul.dropdown-menu [data-toggle="dropdown"]').removeClass('dropped');
    }
    $('ul.my-dropdown-big').hover(
        function() {
            clearTimeout(dropCloseTimer);
        },
        function() {
            //console.log('Maus OUT', new Date());
            //dropCloseTimer = setTimeout( dropClose, 500 );
            if ( $('#vguard').is(':visible') ) {} else { dropClose(); }
        }
    );

// FLOATING DIV PRICE
/*$(window).scroll(function(){
  $("#insu-sum").stop().animate({"marginTop": ($(window).scrollTop()) + "px", "marginLeft":($(window).scrollLeft()) + "px"}, "slow" );
});*/

});

// FLOATING DIV PRICE
(function($) {
    var element = $('.follow-scroll'),
        originalY = element.offset().top;
    
    // Space between element and top of screen (when scrolling)
    var topMargin = 120;
    
    // Should probably be set in CSS; but here just for emphasis
    element.css('position', 'relative');
    
    $(window).on('scroll', function(event) {
        var scrollTop = $(window).scrollTop();
        
        element.stop(false, false).animate({
            top: scrollTop < originalY
                    ? 0
                    : scrollTop - originalY + topMargin
        }, 300);
    });
})(jQuery);

