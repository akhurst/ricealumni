###
Mechanics: Impulses
Science Project (http://scienceproject.codeplex.com)
P. Hurst 2011-2012
###

# Namespaces 
Delta = @Delta = @Delta ? {}
Science = @Science = @Science ? {}
Delta.Namespace("Science.Impulses")

Delta.Require("Delta.Entities")

###
TODO: Should be part of a Delta.Ajax library?
###
Science.Impulses.HtmlLoader = class HtmlLoader extends Delta.Entities.BaseEntity

###
Hijacks impulse links
TODO: Would be nice to wrap this up using Delta to wire things
###
Science.Impulses.ImpulseHijacker = class ImpulseHijacker extends Delta.Entities.BaseEntity
  onInitialize: (init)->
    me = @
    $("a[data-impulse]").live("click", (e)->
      me.ImpulseClicked($(@), e)
    )
    $("a[data-impulse]").live("mousedown", (e)->
      me.ImpulseMouseDown($(@), e)
    )
    $("a[data-impulse]").live("mouseup", (e)->
      me.ImpulseMouseUp($(@), e)
    )
  ImpulseMouseDown: ($impulse, click)->
  ImpulseMouseUp: ($impulse, click)->
  ImpulseClicked: ($impulse, click)->
    impulse = @ImpulseFromEntity($impulse)
    actuate = @Raise("ActuateImpulse", impulse)
    if actuate.Actuated
      click.preventDefault()
      click.stopPropagation()

  ImpulseFromEntity: ($impulse)->
    impulse = 
      ImpulseName: $impulse.attr("data-impulse-name")
    impulse
  ###
  Triggered whenever HTML is loaded into DOM
  ###
  onLoadedHtml: (html)->
    
    $html = html.$
    me = @
    $html.find("a[data-impulse]").each( ->
      $impulse = $ @
      me.PrimeImpulse($impulse,$html)
    )

  PrimeImpulse: ($impulse,$html)->
    # Don't need to do anything right now

Science.Impulses.ImpulseHandler = class ImpulseHandler extends Delta.Entities.BaseEntity    

  ImpulseName: ""

  onActuateImpulse: (actuate)->
    if actuate.ImpulseName == @ImpulseName
      if @Actuate(actuate) then actuate.Actuated = true
    actuate.Actuated

  Actuate: (actuate)->
    return false

Science.Impulses.Defaults = Science.Impulses.Defaults ? {}

Science.Impulses.Defaults.DeleteConnectorImpulseHandler = class DeleteConnectorImpulseHandler extends Science.Impulses.ImpulseHandler

  ImpulseName: "DeleteConnector"
  
  # TODO: Would be useful to get an onActuated event which triggers *after* a successful call to the server  
  Actuate: (actuate)->
    
    # Return false so we continue with server call
    false
    
Delta.Ready ->
  
  CloseMenu = ()->
    $("[data-impulses-context-menu-open=true]").attr("data-impulses-context-menu-open",null);
    $("[data-impulses-context-menu]").remove()

  $("*").live "click", (e)->
    switch (e.which)
      when 1 # left button
        # Check if we're clicking on a menu
        menu = $(this).parents("[data-impulses-context-menu]")
        if (menu.length)
          e.preventDefault()
          # Trigger impulse
          $.impulses.trigger(this)

        else
          # Close a menu if it's open
          CloseMenu()

      # when 2 # middle button
      when 3 # right click
        impulseParent = $(this).parents("[data-impulses]").first()
        if (impulseParent)
          e.preventDefault()
          impulseParent.attr("data-impulse-context-menu-open","true")
          # Loop through data attributes and discover any available menus