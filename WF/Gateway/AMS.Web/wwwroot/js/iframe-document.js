(function($) {
    $(window).ready(function() {
         $("body").mCustomScrollbar({
             theme: "minimal-dark"
         });
    });
    $(window).on("resize", function() {
        setTimeout(function(){
             $("body").mCustomScrollbar("update");
        }, 100);
    });

})(jQuery);