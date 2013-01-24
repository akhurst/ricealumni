
function _compose(e_code) {
window.open("http://alumni.cgi.rice.edu/email/send_message.php?e_code="+e_code,"message_"+e_code,"width=550,height=350,scrollbars=yes,toolbar=no");
}

function doSearch(){

  if (srch.alum.checked == true){
	srch.action="http://www.rice.edu/cgi-bin/subgoogle";
  } else if (srch.rice.checked == true){
	srch.action="http://www.rice.edu/Internet/search/query.php";
  }

  srch.submit();

}

function checkKey(evt){

	srch.search.value=srch.q.value;
	if (evt.keyCode == 13 && srch.search.value != ''){
		doSearch();
	}
	
}


function chkForm(frm) {
  // the name of the form
  f = frm;

  // change msg to the name of any field you want to make required.
  if (f.msg) {
    if (f.msg.value == '') {
      alert('Check all required fields');
      return false;
    }
  }

  // if 'from' address field is used, this checks it to make sure address is valid
  if (f.email) {
    if (f.email.value != '' && (f.email.value.lastIndexOf('.') < 0 || f.email.value.lastIndexOf('@') < 1)) {
      alert('Invalid email address');
      return false;
    }
    else if (f.email.value == '') {
      return confirm('Are you sure you want to send this message without an email address?');
    }
  }
}