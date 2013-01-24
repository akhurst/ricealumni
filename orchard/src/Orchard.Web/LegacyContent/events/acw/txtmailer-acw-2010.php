<?PHP

// Default settings. Change these!
$address = 'roquestrew@rice.edu';		// Default address you want mail sent to
$subj = '';					// Default subject to give to emails
$returnto = '';					// Default page people redirected to

//////////////
// Override defaults
$address = ($_POST['sendto'])?$_POST['sendto']:$address;
$subj = ($_POST['subject'])?$_POST['subject']:$subj;
$returnto = ($_POST['returnto'])?$_POST['returnto']:$returnto;

// if email address isn't present or is invalid, use TO address as FROM address.
if (eregi('^.+@.+\.[a-z]{2,6}',$_POST['email'])) {
  $email = $_POST['email'];
  $message = "Auto Mail from ".$email."\n\n";
}
else {
  $email = $address;
  $message = "Auto Mail from Anonymous\n\n";
}
foreach ($_POST as $key => $val) {
  if ($key != 'returnto' && $key != 'email' && $key != 'subject' && $key != 'sendto') {
    $message .= $key.': '.$val."\n";
  }
}
$message = stripslashes($message);
if (@mail($address, $subj, $message,"From: ".$email)) {
  if ($returnto) {
    header('Location: '.$returnto);
  }
  else {
?>
<html><body>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr valign="top">
    <td rowspan="2"><a href="http://alumni.rice.edu/"><img src="http://alumni.rice.edu/images/logo.gif" width="221" height="94" border="0"></a><br>
        <a href="http://alumni.rice.edu/alumcoll.html"><img src="http://alumni.rice.edu/images/alumnicollege/subpic_acw2010.jpg" alt="Alumni College Weekend 2010" width="526" height="56" border="0"></a><br/>
      <table width="400" border="0" cellspacing="0" cellpadding="0">
        <tr> 
          <td valign="top"><img src="http://alumni.rice.edu/images/clear.gif" width="19" height="14" alt="spacer"></td>
          <td valign="middle"><p><br/>
                <font face="verdana" size="1" color="#666666">		
                Thank you for registering for Alumni College Weekend.<br/>
              You have registered for the classes below. </font></p>
            <table cellpadding="10">
              <tr>
                <td bgcolor="#F1F2D5"><font face="verdana" size="1" color="#666666"><a href="https://online.alumni.rice.edu/default.aspx?Page=EVNTEventDetail&EventID=1172" target="_blank">If you have not yet paid, please click here.</a></font></td>
              </tr>
            </table>
            <p>
              <font face="Verdana, Arial, Helvetica, sans-serif" size="1" color="#666666">
              <?PHP
if(isset($_POST['R1_Name'])){
	if(isset($_POST['R1_Name'])) include "reg1_2010.php";
}
if(isset($_POST['R2_Name'])){
	if(isset($_POST['R2_Name'])) include "reg2_2010.php";
} elseif($_POST['R2_Name'] == "") {
	print "X";
}
?>
            </font></p>
            <p><font face="Verdana, Arial, Helvetica, sans-serif" size="1" color="#666666"><a href="http://alumni.rice.edu/alumcoll.html">Back</a></font></p></td>
        </tr></table>
</td></tr></table>
</body></html>
<?PHP
}
} else {
	echo "<html><body><center><font color='red'><b>there was a problem sending the mail</b></font><p><a href='JavaScript:history.back()'>Back</a></center></body></html>";
}
?>