var time="null"
var winValues
var useUrl

function popunderWin(myurl,mywidth,myheight,link) {
useUrl = myurl + "&url=" + link
winValues = "scrollbars=no,width=" + mywidth + ",height=" + myheight;
time = setTimeout("loadPop()",5000);
//alert('<font color=red><b>'+ popunderWin)

}


function popunderWin2(myurl,mywidth,myheight,link,time) {
	useUrl = myurl + "&url=" + link
	winValues = "scrollbars=yes,toolbar=no,location=no,menubar=no,resizable=yes,status=no,width=" + mywidth + ",height=" + myheight + ",left=50,top=50";
	time = setTimeout("loadPop()",time);
}

function newWin(myurl) {
winValues = "scrollbars=yes,toolbar=yes,location=yes,toobar=yes,menubar=yes,width=750,height=500,resizable=yes";
winName = popupWin(myurl,'order',winValues);
self.close();
}

function loadPop(){
	var winName
	var MainWindow
	MainWindow = self
	winName = window.open(useUrl,'popunder',winValues);
	if (winName){self.focus()}
	//if (winName = null) {winName="null"}
	//if (winName && !winName.closed()){
	
	//if (winName == null){
//	self.focus()
	
//	}else if winName != null{
//	winName.blur();
//	}
}

//reuse opening window and close popup
function recycleWin(myurl){
	window.opener.location=myurl
	window.close()
}

// special delay popunder onLoad funcs
var useUrl1 = null
var value1 = null

function popunderWin1(url,name,value) {
useUrl1 = url
value1 = value
popUnderWin1=window.open(useUrl1,'_pop',value1);
time = setTimeout("blurPop()",800);
//}
//function blurPop(){
//if (popUnderWin1 != null){
	popUnderWin1.blur();
//}
}

function popunderWin2(url,name,value) {
useUrl1 = url
value1 = value
time = setTimeout("blurPop()",5000);
}
function blurPop(){
popUnderWin2=window.open(useUrl1,'_pop',value1);
	if (popUnderWin2){
		self.focus();
	}
}

function popupWin(url,name,value) {
popUpWin=window.open(url,name,value);
}

var exitvariable = true;
var exit=true;
function exitWin(url,name,value) {
	if (exitvariable){
		popUpWin=window.open(url,name,value);
	}
}
function exitUnderWin(url,name,value) {
	if (exitvariable){
		popUnderWin=window.open(url,name,value);
		if (popUnderWin){
			self.focus();
		}
	}
}
function exitWindow3(myurl,myvalues,link)
{
	if (exitvariable)
 	{
		passURL = myurl + "&url=" + link
		if(exit)
		{
			winName = window.open(passURL,'exitwin',myvalues);
			if (winName){
				window.open(passURL,'exitwin',myvalues);
			}
		}
	}
}
//names parent window to reference from popup window
function altWinOpener(url,popname,value,winname) {
self.name = winname;
popUpWin=window.open(url,popname,value,winname);
}
function resizeWindow()
	{
		self.moveTo(0,0);
		self.resizeTo(screen.availWidth,screen.availHeight);
	}

function popUnder(url,name,value)
{
	popUnder=window.open(url,name,value);
	popUnder.blur();
}