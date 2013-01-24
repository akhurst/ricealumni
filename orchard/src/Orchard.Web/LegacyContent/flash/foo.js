function RunFoo()
{
    document.write('<OBJECT CLASSID="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" CODEBASE="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#3,0,0,0" WIDTH="187" HEIGHT="119">');
    document.write('<param NAME="SRC" VALUE="flash/reg_groups.swf">');
    document.write('<embed SRC="flash/reg_groups.swf" PLUGINSPAGE="http://www.macromedia.com/shockwave/download/" TYPE="application/x-shockwave-flash" WIDTH="187" HEIGHT="119"></embed>');
    document.write('<script LANGUAGE="JavaScript">');
    document.write('<!--');
    document.write('var ShockMode = 0;');
    document.write('if (navigator.mimeTypes && navigator.mimeTypes["application/x-shockwave-flash"] && navigator.mimeTypes["application/x-shockwave-flash"].enabledPlugin) {');
    document.write('if (navigator.plugins && navigator.plugins["Shockwave Flash"])');
    document.write('ShockMode = 1;');
    document.write('}');
    document.write('-->');
    document.write('</SCRIPT>');
    document.write('<NOEMBED><a href="regional_groups.html"><img src="images/reg_groups.gif" width="187" height="119" border="0" alt="Regional Groups"></a></NOEMBED>');
    document.write('<NOSCRIPT>');
    document.write('<a href="regional_groups.html"><img src="images/reg_groups.gif" width="187" height="119" border="0" alt="Regional Groups"></a>');
    document.write('</NOSCRIPT>');
    document.write('</OBJECT>\n');
}