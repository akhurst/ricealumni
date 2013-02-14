###
Strand v0.1
(C)2012 P. Hurst

Preliminary draft work - just seeing how it plays out

Definitions:
  Strand:  
  Weave:    A node where two or more strands intersect; can contain further content of any type
  Thread:  
  Web:      The complete world of weaves

###

Strand =
  Network: {}
  Data: {}
  Html: {}
  Xml: {}

class Content extends HtmlEntity
  
  Html:
    TagName: "article"

  Header: null
  Content: null
  Footer: null

class StrandView extends HtmlEntity
    
  Html:
    TagName: "div"

Capture: (site)->
  $.get

Strand.Data.Parser = 
  
Strand.Data.Rss = 
  BuildParser: (parser)->
    parser.On("Xml")
      .Root("rss",(root)=>
      ))