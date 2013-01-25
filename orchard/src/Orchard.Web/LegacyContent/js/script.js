(function($) {
	$(document).ready(function(){
		$('html').removeClass('no-js');
		if ($.browser.safari || $.browser.webkit) {
			$('body').addClass('webkit');
		}

		$('#facebook_wall').facebook_wall({
			id: '70198640179',
			access_token: '144647715576539|6KPR_qYlwciPZl3cKklibgVdZNg',
			limit: 15
		});



		// load the existing photo from location hash
		if (window.location.hash) {
			var start_styling = window.location.hash.replace('#','');
			$('#facebook_wall').removeClass('standard alt1 alt2 alt3').addClass(start_styling);
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
			$('#facebook_wall').removeClass('standard alt1 alt2 alt3').addClass(new_styling);
			e.preventDefault();
		});

		// https://graph.facebook.com/140239969347795/posts/?access_token=144647715576539|6KPR_qYlwciPZl3cKklibgVdZNg&limit=10&locale=da_DK&date_format=U

		$(document).on('click', '#facebook_wall.alt2 li', function() {
			var media_height_img = $(this).find('.media img').outerHeight();
			var media_height_text = $(this).find('.media .media-meta').height();
			if (media_height_img > media_height_text) {
				var media_height = media_height_img;
			} else {
				var media_height = media_height_text;
			}
			$(this).find('.media').css({
				backgroundImage: 'none',
				cursor: 'auto'
			}).animate({
				height: media_height,
				paddingTop: '10px',
				paddingBottom: '10px'
			}, 400, function() {
				$('*', this).animate({
					opacity: '1'
				}, 400);
			});
		});
	});

})(jQuery);