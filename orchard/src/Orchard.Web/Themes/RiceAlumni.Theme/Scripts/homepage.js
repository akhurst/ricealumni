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

    $("#slider4").responsiveSlides({
        auto: false,
        pager: false,
        nav: true,
        speed: 500,
        namespace: "callbacks",
        /*before: function () {
            $('.events').append("<li>before event fired.</li>");
        },
        after: function () {
            $('.events').append("<li>after event fired.</li>");
        }*/
    });
    $(".rslides div").css("visibility", "visible");


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
