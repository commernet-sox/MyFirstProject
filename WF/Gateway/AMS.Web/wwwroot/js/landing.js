/// <reference path="jquery-3.1.1.intellisense.js" />
/// <reference path="jquery-3.1.1.js" />
(function($, dxbsDemo) {
    var ANIMATION_CLASS = 'animated ',
        ANIMATION_TIMEOUT = 300,
        DEFAULT_ANIMATION = "bottom-to-top",
        PARALAX_ENABLED = false,
        DESKTOP_WIDTH = 991,
        SMALL_SIZE_ANIMATION = "left-to-right";

    $(window).on("load", function() {
        initScrollSpy();
        initAnimation();
        initParallax();
        initNavButtons();
    });

    function enableParallax(enabled) {
        if(PARALAX_ENABLED !== enabled) {
            PARALAX_ENABLED = enabled;
            if (PARALAX_ENABLED) {
                $('#scene').parallax('enable');
            }
            else {
                $('#scene').parallax('disable');
            }
        }
    }

    function animateBlocks(parent, selector) {
        var time = 0;
        var blocks = parent.find(selector).each(function() {
            var $block = $(this);
            setTimeout(function() { animateBlock($block); }, time)
            time += ANIMATION_TIMEOUT;
        });
    }

    function animateBlock(block) {
        var animation;
        if($(window).width() >= DESKTOP_WIDTH)
            animation = block.attr("data-animation") || DEFAULT_ANIMATION;
        else
            animation = SMALL_SIZE_ANIMATION;
        block.removeClass("invisible");
        block.addClass(ANIMATION_CLASS + animation).one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend", function() {
            $(this).removeClass(ANIMATION_CLASS + animation);
        });
    }


    function initNavButtons() {
        var $sidebar = $("#sidebar");
        $("#collapse-button").click(function() {
            $(document.body).toggleClass("show-nav");
        });

        $("#scrollNav a[href]").click(function() {
            var $a = $(this);
            $('html, body').animate({
                scrollTop: $($a.attr('href')).offset().top - 80
            }, 300);
        });
    }

    function initParallax() {
        enableParallax($(window).width() >= DESKTOP_WIDTH);
        $(window).on("resize", function() {
            enableParallax($(window).width() >= DESKTOP_WIDTH);
        });
    }

    function initAnimation() {
        var triggers = $("[data-trigger]").toArray();
        $(window).on("scroll", onScroll.bind(this, triggers));
        onScroll(triggers);
    }

    function onScroll(triggers) {
        if(!triggers.length) return;
        var winHeight = $(window).height(),
            winScrollTop = $(this).scrollTop();
        for(var i = 0, trigger; trigger = triggers[i]; i++) {
            var $trigger = $(trigger);
            var triggerOffset = $trigger.offset().top;
            if(triggerOffset < winHeight + winScrollTop && triggerOffset + $trigger.outerHeight() > winScrollTop + 80) {
                animateBlocks($trigger, "." + $trigger.attr("data-trigger"));
                triggers.splice(i, 1);
                i--;
            }
        }
    }

    function initScrollSpy() {
        $('body').scrollspy({ target: '#scrollNav', offset: 80 });
    }

})(jQuery, window.dxbsDemo || (window.dxbsDemo = {}));