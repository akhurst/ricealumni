(function ($) {
    $(document).ready(function () {
        $('html').removeClass('no-js');
        if ($.browser.safari || $.browser.webkit) {
            $('body').addClass('webkit');
        }
    });

})(jQuery);