Delta = @Delta = @Delta ? {}
Delta.Html = Delta.Html ? {}
###
An element entity is either bound to an existing element, or 
###
Delta.Html.ElementEntity = class ElementEntity extends Delta.Entities.BaseEntity
  
  Element: null

  constructor: (element)->
    super()
    if element? then @BindElement(element)

  BindElement: (element)->
    if @Element? then @UnBindElement()
    if element instanceof jQuery
      @$ = element
      @Element = @$.get(0)
    else
      @Element = element
      @$ = $ @Element

    @Fire("Bound",{Element:element})

  UnBindElement: ()->
    if @Element?
      @Fire("UnBound", {Element:@Element})

  onDestroy: ()->
    @UnBindElement()

Delta.Html.InputElement = class InputElement extends Delta.Html.ElementEntity

  meBound: (bound)->
    @$.bind "change", (e)=>@Change?(e)
    @$.bind "click", (e)=>@Click?(e)
    ###
    @$.bind "change", (e)=>@Change(e)
    @$.bind "change", (e)=>@Change(e)
    @$.bind "change", (e)=>@Change(e)
    ###
Delta.Utility.Logger = class Logger extends Delta.Entity
  
  LogContainer: null

  onLog: (log) ->
    # @LogContainer.append("<p>#{log.Text}</p>".replace("\n","</br>"))
    console?.log(log.Text)

  ###
  Cast: (name,data)->
    super
    if not @LogContainer?
      @LogContainer = $("#log > div")
    console?.log("<p><strong>#{name}</strong> #{ ("#{k}=#{v}" for k,v of data).join(", ") }</p>")
  ###