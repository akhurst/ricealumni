// intialization variables
defaultBGColor = '#D0E7D0';
//global variables
var tempImg = 0;
var tiMerId;
tmpCellName = "";
tmpIlayerName = "";
tmplayerName = "";
tmpBGColor = defaultBGColor;
var tempDisplay = ""
// preload nav images
function preloadTopNav(){
	topnav1on = new Image();
	topnav1on.src = "/images/mcc/LoanCenter/mcc2_topnav1_on.gif";
	topnav2on = new Image();
	topnav2on.src = "/images/mcc/LoanCenter/mcc2_topnav2_on.gif";
	topnav3on = new Image();
	topnav3on.src = "/images/mcc/LoanCenter/mcc2_topnav3_on.gif";
	topnav4on = new Image();
	topnav4on.src = "/images/mcc/LoanCenter/mcc2_topnav4_on.gif";
	topnav5on = new Image();
	topnav5on.src = "/images/mcc/LoanCenter/mcc2_topnav5_on.gif";
	subnav1on = new Image();
	subnav1on.src = "/images/mcc/LoanCenter/mcc2_subnav1_on.gif";
	subnav2on = new Image();
	subnav2on.src = "/images/mcc/LoanCenter/mcc2_subnav2_on.gif";
	subnav3on = new Image();
	subnav3on.src = "/images/mcc/LoanCenter/mcc2_subnav3_on.gif";
	subnav4on = new Image();
	subnav4on.src = "/images/mcc/LoanCenter/mcc2_subnav4_on.gif";
	subnav5on = new Image();
	subnav5on.src = "/images/mcc/LoanCenter/mcc2_subnav5_on.gif";
	  
	topnav1off = new Image();
	topnav1off.src = "/images/mcc/LoanCenter/mcc2_topnav1_off.gif";
	topnav2off = new Image();
	topnav2off.src = "/images/mcc/LoanCenter/mcc2_topnav2_off.gif";
	topnav3off = new Image();
	topnav3off.src = "/images/mcc/LoanCenter/mcc2_topnav3_off.gif";
	topnav4off = new Image();
	topnav4off.src = "/images/mcc/LoanCenter/mcc2_topnav4_off.gif";
	topnav5off = new Image();
	topnav5off.src = "/images/mcc/LoanCenter/mcc2_topnav5_off.gif";
	subnav1off = new Image();
	subnav1off.src = "/images/mcc/LoanCenter/mcc2_subnav1_off.gif";
	subnav2off = new Image();
	subnav2off.src = "/images/mcc/LoanCenter/mcc2_subnav2_off.gif";
	subnav3off = new Image();
	subnav3off.src = "/images/mcc/LoanCenter/mcc2_subnav3_off.gif";
	subnav4off = new Image();
	subnav4off.src = "/images/mcc/LoanCenter/mcc2_subnav4_off.gif";
	subnav5off = new Image();
	subnav5off.src = "/images/mcc/LoanCenter/mcc2_subnav5_off.gif";	
}

function initiateMenus(){
	if (loaded){
		init()
	}
}
// browser detection
function Is() {
  var agent = navigator.userAgent.toLowerCase();
  var is_major = parseInt(navigator.appVersion);
  var is_minor = parseFloat(navigator.appVersion);  
  this.ns = ((agent.indexOf('mozilla')!=-1) && ((agent.indexOf('spoofer')==-1) && (agent.indexOf('compatible') == -1)) && (is_major < 5));
  this.ie = (agent.indexOf("msie") != -1);
  this.opera = (agent.indexOf("opera") != -1);
  this.dom = ((agent.indexOf('mozilla')!=-1) && (agent.indexOf('gecko') != -1) && (is_major >= 5));  
}


// eReference function used for referencing objects
function eReference(eName) {
	if (document.layers) {
	//we want 1 object (not nested)
		//simple layer reference
		if (eReference.arguments.length == 1) {
			var cEl = eval("document.layers['" + eName + "']");
		
		//refer document in layer: document.layers[layername].document
		} else if (eReference.arguments[1] == 'DoC') {// use 'strange' spelling to minimise conflicts with existing objects
			var cEl = eval("document.layers['" + eName + "'].document");
		//refer an image: document.images[imageName]
		} else if (eReference.arguments[1] == 'iMageZ') {
			var cEl = eval("document.images['" + eName + "']");
		
		//nested objects
		} else if (eReference.arguments.length>1) {
			var cEl = "document.layers['" + eReference.arguments[1] + "']";//start the string
			for (var i=2;i<eReference.arguments.length;i++) {//leave the first argument, because that's the one you're after, the second because you used that one to start the string
				if ( (eReference.arguments[i] != 'DoC') && (eReference.arguments[i] != 'iMageZ') && (eReference.arguments[i] != 'ForMz') ) {
					//alert(eReference.arguments[i]);
					cEl = cEl + ".document.layers['" + eReference.arguments[i] + "']";//get the next layer
				}
			}
			if (eReference.arguments[eReference.arguments.length-1] == 'iMageZ')  {
				cEl = cEl + ".document.images['" + eName + "']";//in case you need the images in the layer rather than the layer or doc
			} else if (eReference.arguments[eReference.arguments.length-1] == 'ForMz')  {
				cEl = cEl + ".document.forms['" + eName + "']";//in case you need the form in the layer rather than the layer or doc
			} else {
				cEl = cEl + ".document.layers['" + eReference.arguments[0] + "']";//add first element (the one you're after)
				if (eReference.arguments[eReference.arguments.length-1] == 'DoC') {
				cEl = cEl + ".document";//in case you need the document in the layer rather than the layer itself
				}
			} 
			cEl = eval(cEl);//wrap up and make string into an object
		} else {
			var cEl = eval("document.layers['" + eReference.arguments[1] + "'].document." + eName);
		}
	
	//old IE browsers:
	} else if (document.all) {
		var cEl = eval('document.all.' + eName);
	
	//W3C standard:
	} else {
		var cEl = document.getElementById(eName);
	}
	return cEl;
}

// CHANGE IMAGE FUNCTIONS

//used for the main menu navigation
function changeImg(imgName){
	if (tempImg == 0){
		imgOn(imgName);	
	} else {
		imgOff();
		imgOn(imgName);		
	}	
}
  
function imgOn(imgName) {
document.images[imgName].src = eval(imgName + "on.src");
tempImg = imgName;
}

function imgOff() {
	if (tempImg != 0){
	document.images[tempImg].src = eval(tempImg + "off.src");
	tempImg = 0;      
	}  
}

function chgDivImg(imgName,layerName,imgsrc){
	eReference(imgName,layerName,'iMageZ').src = imgsrc
}

// CHANGE BACKGROUND COLOR FUNCTIONS

function chgBG(cellName,layerName,ilayerName,bColor){
		if (document.layers){
			//document.layers[ilayerName].document.layers[layerName].bgColor = bColor;
			eReference(layerName,ilayerName).bgColor = bColor;
			tmpCellName = cellName;
			tmpIlayerName = ilayerName;		
			tmplayerName = layerName
			tmpBGColor = bColor;
			cellactive = 'yes';
		} else {
			//document.all[cellName].style.backgroundColor = bColor;
			eReference(cellName).style.backgroundColor = bColor;
			tmpCellName = cellName;
			tmpBGColor = bColor;	
			cellactive = 'yes';
	}
}

function restoreBG(){
	if (tmpCellName.length > 0){
		chgBG(tmpCellName,tmplayerName,tmpIlayerName,defaultBGColor)	
		// clean up variables	
		tmpCellName = "";
		tmpIlayerName = "";		
		tmplayerName = "";
		tmpBGColor = "";
	}
}

function tDisplay(id){
   	if(eReference(id).style.display == 'none' || eReference(id).style.display == ''){
    	eReference(id).style.display = 'block';
    } else {
    	eReference(id).style.display = 'none';
    }
}

function myOnloadHandler()
	{
  		displayLayers();		
	}
	
	function restore(){
		if(winW != window.innerWidth || winH != window.innerHeight){
		location.reload(true)
		}

	}
	
	function displayLayers(){	
	var msg = "Length: " + document.layers.length + " Divs: "
	for (i=0; i <= (document.layers.length-5); i++){
		//set layer to visible
		document.layers[i].visibility = 'visible';		
		//check layer name		
		j = document.layers[i].name
		j = j.toString();
		//if it's an answer layer (name includes 'A' in it) Then set layer display='none' else display display='block'.
			if (j.indexOf('A') > -1){
				document.layers[i].display = 'none';
			} else {
				document.layers[i].display = 'block';				
			}	
		msg += " Name: " + j + ", "		
	}
	//window.status = msg
}

