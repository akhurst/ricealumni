using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RiceAlumni.Theme.Helpers;

namespace RiceAlumni.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			var events = PciEventCalendarHelper.GetEvents();
		}

		[TestMethod]
		public void UpdatePage()
		{
			var pages = new List<string>
			            {

			            };

			foreach (var page in pages)
			{
				try
				{
					var pathFormat = @"C:\workspaces\ricealumni\orchard\src\Orchard.Web\LegacyContent\{0}{1}";
					var oldPath = string.Format(pathFormat, page,"");
					var newPath = string.Format(pathFormat, page, ".bak");

					var index = 0;
					while (File.Exists(newPath))
					{
						newPath = string.Format(pathFormat, page, "." + ++index + ".bak");
					}
					File.Copy(oldPath,newPath);

					var text = File.ReadAllText(oldPath);
					text = text.Replace("</body>", "      <!-- ADDING RYAN'S STUFF --> <div id=\"section-subfooter\"> <div class=\"container\"> <div class=\"zone zone-sub-footer\"> <article class=\"widget-sub-footer widget-menu-widget widget\"> <nav> <ul class=\"menu menu-footer-menu\"> <li class=\"dropdown first\">			<div class=\"four columns\"> <h4>Alumni Resources</h4> <ul> <li><a href=\"http://alumni.cgi.rice.edu/events/calendar.php\">Events Calendar</a> </li> <li><a href=\"/alumni_groups.html\">Alumni Groups</a> </li> <li><a href=\"/youngalum_stds.html\">Young Alumni</a> </li> </ul> </div> </li> <li class=\"dropdown\">			<div class=\"four columns\"> <h4>Online Services</h4> <ul> <li><a href=\"https://online.alumni.rice.edu/directory/detailsearch.asp\">Alumni Directory</a> </li> <li><a href=\"/career_connection.html\">Career Network</a> </li> <li><a href=\"http://alumni.cgi.rice.edu/bizfinder/\">Business Finder</a> </li> </ul> </div> </li> <li class=\"dropdown\">			<div class=\"four columns\"> <h4>Opportunities</h4> <ul> <li><a href=\"/career_connection.html\">Career Connection</a> </li> <li><a href=\"/learning_trav.html\">Learning &amp;amp; Travel</a> </li> <li><a href=\"/volunteer.html\">Volunteer</a> </li> </ul> </div> </li> <li class=\"dropdown last\">			<div class=\"four columns\"> <h4>Alumni Association</h4> <ul> <li><a href=\"http://online.alumni.rice.edu\">Login</a> </li> <li><a href=\"/about_alumassoc.html\">About the ARA</a> </li> <li><a href=\"/#\">Contact Us</a> </li> </ul> </div> </li> </ul> </nav> </article></div> <div class=\"clear\"></div> </div> </div> <!-- // .container --> <!-- // #section-footer --> <div id=\"section-footer\"> <div class=\"container\"> <div class=\"eight columns\"> <p id=\"copytext\">Copyright &copy; <a href=\"http://alumni.rice.edu/\" title=\"\">Rice University Alumni</a> 2013</p> </div> <!-- // .eight --> <div class=\"clear\"></div> </div> <!-- // .container --> </div> <!-- // #section-footer --> <script src=\"http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js\"></script> <script src=\"js/selectnav.min.js\"></script> <script src=\"js/bootstrap-scrollspy.js\"></script> <script src=\"js/jquery.custom.js\"></script> <script src=\"js/tabcontent.js\" type=\"text/javascript\"></script> <script src=\"js/rotate.js\"></script> <script src=\"js/nav/tinynav.js\" type=\"text/javascript\"></script> <script src=\"js/nav/superfish.js\" type=\"text/javascript\"></script> <script src=\"ryanmagic.js\"></script> <!-- /ADDING RYAN'S STUFF -->");

					text = text.Replace("<body onKeyPress=\"checkKey(event);\" bgcolor=\"#FFFFFF\" text=\"#000000\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\">", "<body onKeyPress=\"checkKey(event);\" bgcolor=\"#FFFFFF\" text=\"#000000\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\"><!-- ADDING RYAN'S STUFF --> <div id=\"fb-root\"></div> <script>(function (d, s, id) {var js, fjs = d.getElementsByTagName(s)[0]; if (d.getElementById(id)) return; js = d.createElement(s); js.id = id; js.src = \"//connect.facebook.net/en_US/all.js#xfbml=1&appId=285205008268336\"; fjs.parentNode.insertBefore(js, fjs); }(document, 'script', 'facebook-jssdk')); </script> <!-- Header  --> <header class=\"head\"> <div class=\"row container\"> <div class=\"five columns\"> <a href=\"index.html\"> <img src=\"images/rice_alumni_logo.png\" alt=\"Logo\" class=\"logo\" /> </a> </div> <!--End Logo --> <!-- nav  --> <nav class=\"eleven columns last\"> <ul id=\"menu\" class=\"sf-menu\"> <li> <a href=\"#\">Alumni Resources <span class=\"carrot\">&#728;</span></a> <ul> <li> <a href=\"events/calendar.php\">Events Calendar</a> <ul> <li><a href=\"http://alumni.cgi.rice.edu/events/calendar.php#upmajevents\">Upcoming Major Events</a></li> <li><a href=\"http://alumni.cgi.rice.edu/events/calendar.php#araevents\">ARA Events and Activities</a></li> </ul> </li> <li> <a href=\"volunteer.html\">Volunteer</a> <ul> <li><a href=\"alumni_groups.html#specialinterest\">Alumni Special Interest Groups</a></li> <li><a href=\"http://futureowls.rice.edu/futureowls/rava.asp\">Admission Office (RAVA)</a></li> <li><a href=\"vol_athsports.html\">Athletics & Sports</a></li> <li><a href=\"Content.aspx?id=231\">Philanthropy</a></li> <li><a href=\"vol_araboard.html\">ARA Board & Committees</a></li> <li><a href=\"vol_businterests.html\">Business Interests</a></li> <li><a href=\"vol_scienceeng.html\">Science & Engineering</a></li> <li><a href=\"vol_artshum.html\">Arts & Humanities</a></li> <li><a href=\"vol_homecoming.html\">Homecoming & Reunion</a></li> <li><a href=\"vol_suppstud.html\">Supporting Students</a></li> <li><a href=\"vol_othinterests.html\">Other Interests</a></li> <li><a href=\"vol_noliveinhst.html\">What if I don't live in Houston?</a></li> </ul> </li> <li> <a href=\"alumni_groups.html\">Alumni Groups</a> <ul> <li><a href=\"alumni_groups.html#specialinterest\">Special Interest Groups</a></li> <li><a href=\"regional_groups.html\">Regional Groups</a></li> <li><a href=\"alumni_groups.html#schoolbased\">School-Based Groups</a></li> <li><a href=\"grad_deg_alumni.html\">Graduate-Degree Alumni</a></li> <li><a href=\"alumni_groups.html#friends\">Friends Groups</a></li> </ul> </li> <li> <a href=\"youngalum_stds.html\">Young Alumni</a> <ul> <li><a href=\"ban_info.html\">Builder's Award</a></li> <li><a href=\"ya_houston.html\">Houston Young Alumni</a></li> <li><a href=\"youngalum_stds.html#students\">Students</a></li> </ul> </li> <li> <a href=\"career_connection.html\">Career Connection</a> <ul> <li><a href=\"career_connection.html#netwkop\">Networking Opportunities</a></li> <li><a href=\"career_connection.html#recruiting\">Recruiting at Rice</a></li> </ul> </li> <li> <a href=\"learning_trav.html\">Learning & Travel</a> <ul> <li><a href=\"http://alumni.rice.edu/alumnicollege/\">Alumni College</a></li> <li><a href=\"http://online.alumni.rice.edu/directory/library.asp\">Rice Alumni Digital Library</a></li> <li><a href=\"travel.html\">Travel Program</a></li> <li><a href=\"learning_scholar.html\">Scholarships & Fellowships</a></li> </ul> </li> </ul> </li> <li> <a href=\"learning_trav.html\">Online Services</a> <ul> <li><a href=\"http://alumni.cgi.rice.edu/events/calendar.php\">Find Events/RSVP</a></li> <li><a href=\"https://online.alumni.rice.edu/directory/detailsearch.asp\">Alumni Directory</a></li> <li><a href=\"https://online.alumni.rice.edu/directory/profile.asp\">Update Address</a></li> <li><a href=\"https://online.alumni.rice.edu/Online/emailforwarding/eforwarding.asp\">Email Forwarding</a></li> <li><a href=\"https://online.alumni.rice.edu/Career/default.asp\">Career Network</a></li> <li><a href=\"http://alumni.cgi.rice.edu/bizfinder/\">Business Finder</a></li> <li><a href=\"https://online.alumni.rice.edu/directory/remotelibrary.asp?type=8\">Online Classnotes</a></li> <li><a href=\"https://online.alumni.rice.edu/directory/library.asp\">Digital Library</a></li> </ul> </li> <li> <a href=\"#\">About the ARA</a></li> <li> <a href=\"#\">Contact Us</a></li> <li> <a href=\"#\">Log In</a></li> </ul> </nav> <!-- End Nav  --> </div> <div class=\"clearfix\"></div> </header> <!-- End Header --> <!-- /ADDING RYAN'S STUFF -->");

					text = text.Replace("<img src=\"images/gray2_bg.gif\"", "<img src=\"images/gray2_bg.gif\" style=\"height: 1px\"");

					File.Delete(oldPath);
					File.WriteAllText(oldPath, text);
				}catch(Exception)
				{}
			}

		}
	}
}
