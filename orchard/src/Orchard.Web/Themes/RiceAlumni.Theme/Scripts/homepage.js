$(document).ready(function () {

    // Smooth scrolling to internal anchors
    $('a[href*=#]:not([href=#])').click(function () {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '')
	        || location.hostname == this.hostname) {

            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html,body').animate({
                    scrollTop: target.offset().top
                }, 600);
                return false;
            }
        }
    });

    // ScrollSpy automatically updates nav targets based on scroll position
    // http://twitter.github.com/bootstrap/javascript.html#scrollspy
    $('.menu-main-menu').scrollspy();

    // SelectNav.js is a JavaScript plugin that lets you convert your website navigation into a select drop-down menu. Used together with media queries it helps you to create a space saving, responsive navigation for small screen devices.
    // https://github.com/lukaszfiszer/selectnav.js
    selectnav('nav');


    $("#slider4").responsiveSlides({
        auto: false,
        pager: false,
        nav: true,
        speed: 500,
        namespace: "callbacks",
        before: function () {
            $('.events').append("<li>before event fired.</li>");
        },
        after: function () {
            $('.events').append("<li>after event fired.</li>");
        }
    });


    $("#slider1").responsiveSlides({
        maxwidth: 728,
        speed: 800
    });




    $("#sliderbottom").responsiveSlides({
        maxwidth: 728,
        speed: 800
    });

    $("#medrec").responsiveSlides({
        maxwidth: 300,
        speed: 800
    });




});
