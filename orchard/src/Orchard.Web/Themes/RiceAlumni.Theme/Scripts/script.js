(function($) {
	$(document).ready(function(){
		$('html').removeClass('no-js');
		if ($.browser.safari || $.browser.webkit) {
			$('body').addClass('webkit');
		}

		// load the existing photo from location hash
		if (window.location.hash) {
			var start_styling = window.location.hash.replace('#','');
			$('styling-buttons').find('a[href=#' + start_styling + ']').addClass('current');
		}
		
			/*var hash = document.location.hash;
			if (hash) {
				hash = hash.substring(1) - 1;
				var photo = this.$_gallery.find('.photo:eq(' + hash + ')');
				var link = $('.thumbs').find('li:eq(' + hash + ') a');
				TNWGallery.showPhoto(photo, link, false);		
			}*/

		$('.styling-buttons a').click(function(e) {
			$(this).siblings().removeClass('current');
			$(this).addClass('current');
			var new_styling = $(this).attr('href').replace('#','');
			e.preventDefault();
		});

		// https://graph.facebook.com/140239969347795/posts/?access_token=144647715576539|6KPR_qYlwciPZl3cKklibgVdZNg&limit=10&locale=da_DK&date_format=U

	});

})(jQuery);