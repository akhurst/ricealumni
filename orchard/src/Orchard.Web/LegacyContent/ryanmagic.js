$("body").prepend("<div class='thenewhead'></div>");
//$(".thenewhead").prepend("<div id='section-topbar'><div id='topbar-inner'><div class='container'><div class='sixteen columns'><div class='dropdown'><img src='headimages/rice_alumni_logo.png' class='headlogo' /><ul id='nav' class='nav'><li class='menu-item'><a href='#'>Events Calendar</a></li><li class='menu-item'><a href='#'>Alumni Groups</a></li><li class='menu-item'><a href='#'>Young Alumni</a></li><li class='menu-item'><a href='#'>Career Connection</a></li><li class='menu-item'><a href='#'>Volunteer</a></li><li class='menu-item'><a href='#'>Learning & Travel</a></li></div></div></div></div>");
$('body table:first').wrap('<div class="newholder" />');
$("body table:first").css("margin", "0 auto 0 auto");
$("body table:first").css("border-top", "1px solid #eee");
$("body table tbody tr td table tbody tr:first").remove();
$("body table:first").css("width","960px");
$('body table tbody tr td table tr td table tbody tr td').has('form#srch').remove();


var images = [], 
index = 0;

images[0] = "url(headimages/one.jpg)";

images[1] = "url(headimages/two.jpg)";

images[2] = "url(headimages/three.jpg)";

images[3] = "url(headimages/four.jpg)";

index = Math.floor(Math.random() * images.length);

var imgnewnao = images[index];



$(".thenewhead").css("background-image", imgnewnao);

//$(".thenewhead").html(imgnewnao);

//document.write(images[index]);

//$('td ').has('form').remove();
//$('body table tbody tr td table tr td table tbody tr td').has('form').css('background-color', 'red');