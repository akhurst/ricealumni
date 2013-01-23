<?PHP echo(IsSet($_POST['R1_Name']))?'<hr>
<h3>Registrant 1:&nbsp; '.$_POST['R1_Name'].'</h3><hr size="1">
':"";

if(isset($_POST['R1_Class1'])){
echo "<br/>
FRIDAY<br/>
<br/><b>Class 1 — 1:45&ndash;3:15 p.m.</b><br/>
";
if($_POST['R1_Class1']=="1JL"){
	echo "<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>Understanding Modern Central Asia, <i>Jonathan Ludwig</i><br>
	";
} else if ($_POST['R1_Class1']=="1CK"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>The Art of Comedy, <i>Christina Keefe</i><br>
	');
} else if ($_POST['R1_Class1']=="1ES"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>Gender, Feminism, and Islam, <i>Elora Shehabuddin</i><br>
');
}
	echo('<p>
	');
}

if(isset($_POST['R1_Class2'])){
echo "<b>Class 2 — 3:30&ndash;5 p.m.</b><br/>
";
 if($_POST['R1_Class2']=="2DS"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>Sweatshops and Global Wa.m.ng, <i>Doug Schuler</i><br>
	');
} else if ($_POST['R1_Class2']=="2JC"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>Shakespeare&rsquo;s Troubled Genres, <i>Joseph Campana</i><br>
');
} else if ($_POST['R1_Class2']=="2DST"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>Gender, Economic Well-Being, and Global Justice, <i>Diana Strassmann</i><br>
');
}
	echo('
	<p>
	');
}

if(isset($_POST['R1_Class3'])){
echo "<br/>
SATURDAY MORNING<br/>
<br/><b>Class 3 — 9&ndash;9:40 a.m.</b><br/>
";
 if($_POST['R1_Class3']=="3AM"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20><i>Allen Matusow</i>, interim dean of the School of Humanities<br>
');
} else if ($_POST['R1_Class3']=="3SW"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20><i>Sarah Whiting</i>, dean of the School of Architecture<br>
');
}
	echo('
	<p>
	');
}

if(isset($_POST['R1_Class4'])){
echo "<b>Class 4 — 9:50&ndash;10:30 a.m.</b><br/>
";
 if($_POST['R1_Class4']=="4WG"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20><i>William Glick</i>, dean of the Jones Graduate School of Business<br>
	');
} else if ($_POST['R1_Class4']=="4LR"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20><i>Lyn Ragsdale</i>, dean of the School of Social Sciences<br>
');
}
	echo('
	<p>
	');
}

if(isset($_POST['R1_Class5'])){
echo "<b>Class 5 — 10:40&ndash;11:20 a.m.</b><br/>
";
 if($_POST['R1_Class5']=="5DC"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20><i>Daniel Carson</i>, dean of the Wiess School of Natural Sciences<br>
');
} else if ($_POST['R1_Class5']=="5MM"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20><i>Mary McIntire</i>, dean of the Glasscock School of Continuing Studies<br>
');
}
	echo('
<p>
	');
}

if(isset($_POST['R1_Class6'])){
echo "<b>Class 6 — 11:30 a.m. &ndash;12:10 p.m.</b><br/>
";
 if($_POST['R1_Class6']=="6SAK"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20><i>Sallie Ann Keller</i>, dean of the Brown School of Engineering<br>
');
} else if ($_POST['R1_Class6']=="6RY"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20><i>Robert Yekovich</i>, dean of the Shepherd School of Music<br>
');
}
	echo('
<p>
	');
}

if(isset($_POST['R1_Class7'])){
echo"SATURDAY AFTERNOON<br/>
<br/>
<b>Class 7 — 2&ndash;3:30 p.m.</b><br/>
";
 if($_POST['R1_Class7']=="7VM"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>Health Tourism, <i>Vikas Mittal</i><br>
');
} else if ($_POST['R1_Class7']=="7CD"){
	echo('<img src="http://alumni.rice.edu/images/clear.gif" width="15" height="20" />The World is Yours: Genre as Network in Global Cinema, <i>Charles Dove</i><br />
');
} else if ($_POST['R1_Class7']=="7RH"){
	echo('<img src="http://alumni.rice.edu/images/clear.gif" width="15" height="20" />Grassroots Women&rsquo;s Leadership in Mexico, <i>Rosemary Hennessy</i><br />
');
} else if ($_POST['R1_Class7']=="7RS"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>Why Are There Pirates Off Somalia?, <i>Richard Stoll</i><br>
');
}
	echo('
<p>
	');
}
if(isset($_POST['R1_Class8'])){
echo"<b>Class 8 — 3:45&ndash;5:15 p.m.</b><br/>
";
 if($_POST['R1_Class8']=="8JF"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>America Through French Eyes, <i>Julie Fette</i><br>
');
} else if ($_POST['R1_Class8']=="8CH"){
	echo('<img src="http://alumni.rice.edu/images/clear.gif" width="15" height="20" />Anthropological Perspectives on Gender and Sexuality, <i>Cymene Howe</i><br />
');
}  else if ($_POST['R1_Class8']=="8MC"){
	echo('<img src="http://alumni.rice.edu/images/clear.gif" width="15" height="20" />Women in Business: Opportunities and Obstacles, <i>Margaret Cording</i><br />
');
} else if ($_POST['R1_Class8']=="8SS"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>Entrepreneurism in the Houston Medical Device Industry, <i>Sean Self</i><br>
');
}
	echo('
<p>
	');
}
if(isset($_POST['R1_Class9'])){
echo "<br/>
SUNDAY<br/>
<br/>
<b>Class 9 — 9&ndash;10:30 a.m.</b><br/>
";
 if($_POST['R1_Class9']=="9CA"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>Ancient Nanotechnology and Modern Nanoscience, <br />
    <i>Robert Curl and Wade Adams</i><br>
');
} else if ($_POST['R1_Class9']=="9HH"){
	echo('<img src="http://alumni.rice.edu/images/clear.gif" width="15" height="20" />Sex and Gender in the American Family, <i>Holly Heard</i><br />
');
} else if ($_POST['R1_Class9']=="9JF"){
	echo('<img src="http://alumni.rice.edu/images/clear.gif" width="15" height="20" />Goods and Globalization, <i>James Faubion</i><br />
');
} else if ($_POST['R1_Class9']=="9DD"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20>Comedy in Opera, <i>Debra Dickinson</i><br>
');
}
	echo('
<p>
	');
}

if(isset($_POST['R1_Class10'])){
echo "<b>Class 10 — 10:45 a.m. &ndash;12:15 p.m.</b><br/>
";
 if($_POST['R1_Class10']=="10DL"){
	echo('<img src=http://alumni.rice.edu/images/clear.gif width=15 height=20><i>David Leebron</i>, president of Rice University<br>
');
}
	echo('');
}